using System;

namespace AsbMassNetCoreTopic.Contracts
{
    public class PurchaseItem
    {
        public Guid PurchaseItemId { get; set; }
        public DateTime Timestamp { get; set; }

        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}