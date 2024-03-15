using MassTransit;
using Common.Models.Models;

namespace Inventory.Consumer
{
    public class OrderConsumer : IConsumer<TblOrder>
    {
        public Task Consume(ConsumeContext<TblOrder> context)
        {
            TblOrder OrderRecord = context.Message;
            return Task.CompletedTask;
        }
    }
}
