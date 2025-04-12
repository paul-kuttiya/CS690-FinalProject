using System;
using GiftTrackerApp.Models;
using Xunit;

namespace GiftTrackerApp.Tests.Models
{
    public class GiftIdeaTests
    {
        [Fact]
        public void Default_Properties_ShouldBeInitialized()
        {
            var gift = new GiftIdea();
            Assert.Equal(0, gift.Id);
            Assert.Equal(string.Empty, gift.Title);
            Assert.Equal(string.Empty, gift.Description);
            Assert.Equal(string.Empty, gift.AdditionalNotes);
            Assert.Equal(string.Empty, gift.GiftFor);
            Assert.Equal(default(DateTime), gift.CreatedAt);
            Assert.Equal(0, gift.Price);
        }

        [Fact]
        public void Can_Assign_Properties()
        {
            var now = DateTime.Now;
            var gift = new GiftIdea
            {
                Id = 1,
                Title = "Test Gift",
                Description = "This is a test gift idea.",
                AdditionalNotes = "Some additional notes.",
                GiftFor = "Mom",
                CreatedAt = now,
                Price = 100
            };
            Assert.Equal(1, gift.Id);
            Assert.Equal("Test Gift", gift.Title);
            Assert.Equal("This is a test gift idea.", gift.Description);
            Assert.Equal("Some additional notes.", gift.AdditionalNotes);
            Assert.Equal("Mom", gift.GiftFor);
            Assert.Equal(now, gift.CreatedAt);
            Assert.Equal(100, gift.Price);
        }
    }
}
