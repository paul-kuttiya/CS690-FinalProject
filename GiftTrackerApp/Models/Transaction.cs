using System;

namespace GiftTrackerApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string GiftCardType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
