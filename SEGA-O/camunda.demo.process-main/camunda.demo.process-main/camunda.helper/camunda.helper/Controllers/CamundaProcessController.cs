﻿using camunda.helper.Models;
using Camunda.Api.Client;
using Camunda.Api.Client.ProcessDefinition;
using Camunda.Api.Client.UserTask;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace camunda.helper.Controllers
{
    public class CamundaProcessController : ControllerBase
    {
        private readonly ILogger<CamundaProcessController> _logger;
        private CamundaClient _client;
        public CamundaProcessController(ILogger<CamundaProcessController> logger)
        {
            _logger = logger;
            _client = CamundaClient.Create("http://127.0.0.1:6060/engine-rest");
        }

        [HttpGet("startProcess")]
        public async Task<IActionResult> StartProcess(MyBPMNProcess myBPMNProcess)
        {
            _logger.LogInformation("Starting the sample Camunda process...");
            try
            {
                Random random = new Random();

                //Creating process parameters
                StartProcessInstance processParams;
                if (myBPMNProcess.Equals(MyBPMNProcess.Process_Transaction_Saga_Orchestrator))
                {
                    OrderPostModel orderPostModel = new OrderPostModel
                    {
                        orders = new List<OrderData>()
                    };
                    //Creating random products for ordering
                    for (int i = 0; i < random.Next(1, 3); i++)
                    {
                        orderPostModel.orders.Add(new OrderData
                        {
                            productId = random.Next(121, 125),
                            unitPrice = random.Next(50, 500),
                            units = random.Next(1, 10)
                        });
                    }
                    // 0 => no exception
                    // 1=> exception in 'Create Order'
                    // 2=> exception in 'Process Payment'
                    // 3=> exception in 'Update Inventory'
                    // 4=> exception in 'Deliver Order'
                    processParams = new StartProcessInstance()
                       .SetVariable("exceptionInProcess", random.Next(0, 4))
                       .SetVariable("OrderPostModel", JsonConvert.SerializeObject(orderPostModel.orders));

                    _logger.LogInformation($"Camunda process to demonstrate Saga based orchestrator started..........");
                }
                else
                {
                    int numberOfCups = random.Next(1, 10);
                    processParams = new StartProcessInstance()
                        .SetVariable("numberOfCups", numberOfCups);

                    _logger.LogInformation($"Camunda process to prepare tea started. Preaparing {numberOfCups} cup(s) of tea.........");
                }
                
                //Startinng the process
                var proceStartResult = await _client.ProcessDefinitions.ByKey(myBPMNProcess.ToString())
                    .StartProcessInstance(processParams);

                

                return Ok(proceStartResult.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"error occured!! error messge: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetUserTasks(MyBPMNProcess myBPMNProcess)
        {
            _logger.LogInformation($"Fetching user tasks for {myBPMNProcess}...");
            try
            {
                var userTaskQuery = new TaskQuery { ProcessDefinitionKey = myBPMNProcess.ToString() };
                var userTasks = await _client.UserTasks.Query(userTaskQuery).List();

                _logger.LogInformation($"user tasks fetched for {myBPMNProcess}....");

                return Ok(userTasks.Select(x=>x.Id).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"error occured!! error messge: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("completeUserTask/{id}")]
        public async Task<IActionResult> CompleteUserTask(string id) 
        {
            _logger.LogInformation($"Marking the manual task {id} complete...");
            try
            {
                Random random = new Random();

                //Fetch the manual task to be marked as complete
                var task = await _client.UserTasks[id].Get();
                var completeTask = new CompleteTask()
                    // Setting variable numberOfCups between 1-7 so that avaialability clause go through 
                    .SetVariable("numberOfCups", random.Next(1, 7)); 
                //Mark the manual task as complete
                await _client.UserTasks[id].Complete(completeTask);

                _logger.LogInformation($"Manual task {id} completed. Inventory is full now.......");

                return Ok($"Manual task {id} completed. Inventory is full now.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"error occured!! error messge: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }

    public enum MyBPMNProcess
    {
        Process_Prepare_Tea,
        Process_Check_Items_Availability,
        Process_Transaction_Saga_Orchestrator
    }
}
