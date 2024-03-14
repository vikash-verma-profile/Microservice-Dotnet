using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Models;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly EshoppingContext _context;
        private readonly IBus _bus;

        public OrdersController(EshoppingContext context,IBus bus)
        {
            _context = context;
            _bus = bus;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblOrder>>> GetTblOrders()
        {
          if (_context.TblOrders == null)
          {
              return NotFound();
          }
            return await _context.TblOrders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblOrder>> GetTblOrder(int id)
        {
          if (_context.TblOrders == null)
          {
              return NotFound();
          }
            var tblOrder = await _context.TblOrders.FindAsync(id);

            if (tblOrder == null)
            {
                return NotFound();
            }

            return tblOrder;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblOrder(int id, TblOrder tblOrder)
        {
            if (id != tblOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblOrder>> PostTblOrder(TblOrder tblOrder)
        {
          if (_context.TblOrders == null)
          {
              return Problem("Entity set 'EshoppingContext.TblOrders'  is null.");
          }
            //_context.TblOrders.Add(tblOrder);
            //_context.SaveChanges();
            Uri uri = new Uri("rabbitmq:localhost/OrderQueue");
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(tblOrder);
            return CreatedAtAction("GetTblOrder", new { id = tblOrder.Id }, tblOrder);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblOrder(int id)
        {
            if (_context.TblOrders == null)
            {
                return NotFound();
            }
            var tblOrder = await _context.TblOrders.FindAsync(id);
            if (tblOrder == null)
            {
                return NotFound();
            }

            _context.TblOrders.Remove(tblOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblOrderExists(int id)
        {
            return (_context.TblOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
