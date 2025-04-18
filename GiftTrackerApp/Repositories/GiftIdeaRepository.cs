using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GiftTrackerApp.Models;

namespace GiftTrackerApp.Repositories
{
    public class GiftIdeaRepository
    {
        private readonly string _filePath;

        public GiftIdeaRepository(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, string.Empty);
        }

        // Load all gift ideas from file.
        public List<GiftIdea> LoadAll() =>
            File.ReadAllLines(_filePath)
                .Where(l => l.StartsWith("GIFT|"))
                .Select(ParseLine)
                .ToList();

        // Overwrite file with given ideas.
        public void SaveAll(IEnumerable<GiftIdea> ideas)
        {
            var lines = ideas.Select(i => i.ToString());
            File.WriteAllLines(_filePath, lines);
        }

        // Append a single idea to file.
        public void Append(GiftIdea idea)
        {
            File.AppendAllText(_filePath, idea + Environment.NewLine);
        }

        // Convert a file line into a GiftIdea object.
        private GiftIdea ParseLine(string line)
        {
            var parts = line.Split('|');
            return new GiftIdea
            {
                Id = int.Parse(parts[1]),
                Timestamp = DateTime.Parse(parts[2]),
                Title = parts[3],
                Description = parts[4],
                Notes = parts[5],
                GiftFor = parts[6],
                Price = float.Parse(parts[7])
            };
        }
    }
}
