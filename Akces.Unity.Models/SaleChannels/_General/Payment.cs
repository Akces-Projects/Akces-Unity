using System;

namespace Akces.Unity.Models.SaleChannels
{
    public class Payment
    {
        public string PaymentMethod { get; set; }
        public string TransactionNumber { get; set; }
        public string Currency { get; set; }
        public decimal Value { get; set; }
        public DateTime? TimeLimit { get; set; }
    }
}
