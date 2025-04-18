using System;
using System.IO;
using System.Linq;
using GiftTrackerApp.Models;
using GiftTrackerApp.Repositories;
using GiftTrackerApp.Services;
using Xunit;

namespace GiftTrackerApp.Tests
{
    public class GiftIdeaServiceTests
    {
        private string GetTempFile()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, string.Empty);
            return path;
        }

        [Fact]
        public void Add_AssignsIdAndTimestamp_AndSaves()
        {
            var file = GetTempFile();
            var service = new GiftIdeaService(new GiftIdeaRepository(file));

            var idea = new GiftIdea
            {
                Title = "Test",
                Description = "Desc",
                Notes = "Notes",
                GiftFor = "Friend",
                Price = 10.5f
            };
            var result = service.Add(idea);

            Assert.Equal(1, result.Id);
            Assert.NotEqual(default, result.Timestamp);
            var all = service.GetAll();
            Assert.Single(all);
            Assert.Equal("Test", all.First().Title);
        }

        [Fact]
        public void Update_ReturnsFalse_WhenNotFound()
        {
            var file = GetTempFile();
            var service = new GiftIdeaService(new GiftIdeaRepository(file));
            var fake = new GiftIdea { Id = 99 };
            Assert.False(service.Update(fake));
        }

        [Fact]
        public void Update_ModifiesExistingIdea()
        {
            var file = GetTempFile();
            var service = new GiftIdeaService(new GiftIdeaRepository(file));
            var added = service.Add(new GiftIdea { Title = "A", Price = 1f });
            added.Title = "B";

            var updated = service.Update(added);
            Assert.True(updated);
            var all = service.GetAll();
            Assert.Equal("B", all.First().Title);
        }

        [Fact]
        public void Delete_RemovesIdea()
        {
            var file = GetTempFile();
            var service = new GiftIdeaService(new GiftIdeaRepository(file));
            var added = service.Add(new GiftIdea { Title = "A", Price = 1f });

            Assert.True(service.Delete(added.Id));
            Assert.Empty(service.GetAll());
        }

        [Fact]
        public void Search_FindsMatchingItem()
        {
            var file = GetTempFile();
            var service = new GiftIdeaService(new GiftIdeaRepository(file));
            service.Add(new GiftIdea { Title = "Hello world", Price = 1f });
            service.Add(new GiftIdea { Title = "Other", Price = 1f });

            var results = service.Search("hello");
            Assert.Single(results);
            Assert.Contains("Hello world", results.First().Title);
        }

        [Fact]
        public void GetSummary_ReturnsCorrectCountAndTotal()
        {
            var file = GetTempFile();
            var service = new GiftIdeaService(new GiftIdeaRepository(file));
            service.Add(new GiftIdea { Title = "A", Price = 2f });
            service.Add(new GiftIdea { Title = "B", Price = 3f });

            var (count, total) = service.GetSummary();
            Assert.Equal(2, count);
            Assert.Equal(5f, total);
        }
    }
}
