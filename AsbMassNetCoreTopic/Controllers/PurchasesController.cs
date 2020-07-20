using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsbMassNetCoreTopic.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AsbMassNetCoreTopic.Sender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly Random _random;

        public PurchasesController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;

            _random = new Random();
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewPurchase()
        {
            var purchaseItems = new List<PurchaseItem>
            {
                new PurchaseItem
                {
                    PurchaseItemId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow,
                    Name = "Bus Mass Transformer High Spec",
                    Amount = 1,
                    Price = 100.00m
                }
            };

            await _publishEndpoint
                        .Publish(
                                    new Purchase
                                    {
                                        PurchaseId = Guid.NewGuid(),
                                        PublicPurchaseId = $"Id_{_random.Next(1, 999)}",
                                        Timestamp = DateTime.UtcNow,
                                        PurchaseItems = purchaseItems
                                    }
                                    );

            return Ok();
        }
    }
}
