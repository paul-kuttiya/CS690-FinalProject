using System;

namespace GiftTrackerApp.Models
{
    public class GiftIdea
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AdditionalNotes { get; set; } = string.Empty;
        public string GiftFor { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Price { get; set; }
    }
}
