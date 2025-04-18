using System.Collections.Generic;
using System.Linq;
using GiftTrackerApp.Models;
using GiftTrackerApp.Repositories;

namespace GiftTrackerApp.Services
{
    public class GiftIdeaService
    {
        private readonly GiftIdeaRepository _repo;

        public GiftIdeaService(GiftIdeaRepository repo) => _repo = repo;

        // Add a new idea
        public GiftIdea Add(GiftIdea idea)
        {
            var all = _repo.LoadAll();
            idea.Id = all.Any() ? all.Max(i => i.Id) + 1 : 1;
            idea.Timestamp = System.DateTime.Now;
            _repo.Append(idea);
            return idea;
        }

        // Update an existing idea.
        public bool Update(GiftIdea updated)
        {
            var all = _repo.LoadAll();
            var idx = all.FindIndex(i => i.Id == updated.Id);
            if (idx < 0) return false;
            updated.Timestamp = all[idx].Timestamp;
            all[idx] = updated;
            _repo.SaveAll(all);
            return true;
        }

        // Remove an idea
        public bool Delete(int id)
        {
            var all = _repo.LoadAll();
            var item = all.FirstOrDefault(i => i.Id == id);
            if (item == null) return false;
            all.Remove(item);
            _repo.SaveAll(all);
            return true;
        }

        // Search ideas by keyword
        public List<GiftIdea> Search(string keyword) =>
            _repo.LoadAll()
                .Where(i => i.Title.Contains(keyword, System.StringComparison.OrdinalIgnoreCase)
                         || i.Description.Contains(keyword, System.StringComparison.OrdinalIgnoreCase)
                         || i.Notes.Contains(keyword, System.StringComparison.OrdinalIgnoreCase)
                         || i.GiftFor.Contains(keyword, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

        // Get all ideas.
        public List<GiftIdea> GetAll() => _repo.LoadAll();

        // Return the summary of ideas
        public (int count, float total) GetSummary()
        {
            var all = _repo.LoadAll();
            return (all.Count, all.Sum(i => i.Price));
        }
    }
}
