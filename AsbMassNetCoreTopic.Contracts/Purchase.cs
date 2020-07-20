using System;
using System.Collections.Generic;

namespace AsbMassNetCoreTopic.Contracts
{
    public class Purchase
    {
        public Guid PurchaseId { get; set; }
        public string PublicPurchaseId { get; set; }

        public DateTime Timestamp { get; set; }

        public IEnumerable<PurchaseItem> PurchaseItems { get; set; }
    }
}
