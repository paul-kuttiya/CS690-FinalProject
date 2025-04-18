using System;
using System.IO;
using System.Linq;
using GiftTrackerApp.Models;
using GiftTrackerApp.Repositories;
using Xunit;

namespace GiftTrackerApp.Tests
{
    public class GiftIdeaRepositoryTests
    {
        private string GetTempFile()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, string.Empty);
            return path;
        }

        [Fact]
        public void Append_AndLoadAll_ShouldPersistData()
        {
            var file = GetTempFile();
            var repo = new GiftIdeaRepository(file);
            var now = DateTime.Now;
            var idea = new GiftIdea
            {
                Id = 1,
                Timestamp = now,
                Title = "T",
                Description = "D",
                Notes = "N",
                GiftFor = "F",
                Price = 1.2f
            };

            repo.Append(idea);
            var list = repo.LoadAll();

            Assert.Single(list);
            Assert.Equal(1, list.First().Id);
            Assert.Equal("T", list.First().Title);
        }

        [Fact]
        public void SaveAll_OverwritesExistingFile()
        {
            var file = GetTempFile();
            var repo = new GiftIdeaRepository(file);
            repo.Append(new GiftIdea { Id = 1, Timestamp = DateTime.Now, Title = "X", Price = 0f });

            var newList = new[] { new GiftIdea { Id = 2, Timestamp = DateTime.Now, Title = "Y", Price = 0f } };
            repo.SaveAll(newList);
            var list = repo.LoadAll();

            Assert.Single(list);
            Assert.Equal(2, list.First().Id);
        }
    }
}
