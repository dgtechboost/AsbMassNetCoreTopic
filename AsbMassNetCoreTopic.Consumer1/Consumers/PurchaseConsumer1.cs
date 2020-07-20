using System.Threading.Tasks;
using AsbMassNetCoreTopic.Contracts;
using MassTransit;

namespace AsbMassNetCoreTopic.Consumer1.Consumers
{
    public class PurchaseConsumer1 
        : IConsumer<Purchase>
    {
        public Task Consume(ConsumeContext<Purchase> context)
        {
            System.Threading.Thread.Sleep(60000);//60000 one minnute

            return Task.CompletedTask;
        }
    }
}
