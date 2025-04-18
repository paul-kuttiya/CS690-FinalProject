using System;

namespace GiftTrackerApp.Models
{
    public class GiftIdea
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string GiftFor { get; set; } = string.Empty;
        public float Price { get; set; }

        public override string ToString()
        {
            return string.Join("|", "GIFT", Id, Timestamp, Title, Description, Notes, GiftFor, Price);
        }
    }
}
