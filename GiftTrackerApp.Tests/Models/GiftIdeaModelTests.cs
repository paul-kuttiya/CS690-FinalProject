using System;
using GiftTrackerApp.Models;
using Xunit;

namespace GiftTrackerApp.Tests
{
    public class GiftIdeaModelTests
    {
        [Fact]
        public void ToString_IncludesAllFieldsSeparatedByPipe()
        {
            var idea = new GiftIdea
            {
                Id = 5,
                Timestamp = new DateTime(2025, 1, 2, 3, 4, 5),
                Title = "T",
                Description = "D",
                Notes = "N",
                GiftFor = "F",
                Price = 9.5f
            };
            var s = idea.ToString();
            var parts = s.Split('|');
            Assert.Equal("GIFT", parts[0]);
            Assert.Equal("5", parts[1]);
            Assert.Equal("2025-01-02T03:04:05", DateTime.Parse(parts[2]).ToString("s"));
            Assert.Equal("T", parts[3]);
            Assert.Equal("D", parts[4]);
            Assert.Equal("N", parts[5]);
            Assert.Equal("F", parts[6]);
            Assert.Equal("9.5", parts[7]);
        }
    }
}
