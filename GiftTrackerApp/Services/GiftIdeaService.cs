using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace GiftTrackerApp.Services
{
    public static class GiftIdeaService
    {
        public static void AddGiftIdea(string dataFilePath)
        {
            Console.Write("Enter Gift Idea Title: ");
            string title = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter Gift Description: ");
            string description = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter Additional Notes (optional): ");
            string notes = Console.ReadLine() ?? string.Empty;

            int newId = GetNextGiftIdeaId(dataFilePath);
            string entry = $"GIFT|{newId}|{DateTime.Now}|{title}|{description}|{notes}|New";
            File.AppendAllText(dataFilePath, entry + Environment.NewLine);
            Console.WriteLine("Gift idea added successfully.");
        }

        public static void EditGiftIdea(string dataFilePath)
        {
            Console.Write("Enter Gift Idea ID: ");
            string idInput = Console.ReadLine() ?? string.Empty;
            if (!int.TryParse(idInput, out int editId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var lines = File.ReadAllLines(dataFilePath).ToList();
            bool found = false;

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("GIFT|"))
                {
                    var parts = lines[i].Split('|');
                    if (parts.Length >= 7 && int.TryParse(parts[1], out int currentId) && currentId == editId)
                    {
                        found = true;
                        Console.WriteLine("Current Details:");
                        Console.WriteLine($"Title: {parts[3]}");
                        Console.WriteLine($"Description: {parts[4]}");
                        Console.WriteLine($"Additional Notes: {parts[5]}");

                        Console.Write("Enter new Title (press Enter to keep current): ");
                        string newTitle = Console.ReadLine() ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(newTitle))
                            newTitle = parts[3];

                        Console.Write("Enter new Description (press Enter to keep current): ");
                        string newDescription = Console.ReadLine() ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(newDescription))
                            newDescription = parts[4];

                        Console.Write("Enter new Additional Notes (press Enter to keep current): ");
                        string newNotes = Console.ReadLine() ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(newNotes))
                            newNotes = parts[5];

                        string updatedLine = $"GIFT|{parts[1]}|{parts[2]}|{newTitle}|{newDescription}|{newNotes}|{parts[6]}";
                        lines[i] = updatedLine;
                        Console.WriteLine("Gift idea updated successfully.");
                        break;
                    }
                }
            }

            if (!found)
                Console.WriteLine("Gift Idea not found.");
            else
                File.WriteAllLines(dataFilePath, lines);
        }

        public static void DeleteGiftIdea(string dataFilePath)
        {
            Console.Write("Enter Gift Idea ID: ");
            string idInput = Console.ReadLine() ?? string.Empty;
            if (!int.TryParse(idInput, out int deleteId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var lines = File.ReadAllLines(dataFilePath).ToList();
            bool found = false;

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("GIFT|"))
                {
                    var parts = lines[i].Split('|');
                    if (parts.Length >= 7 && int.TryParse(parts[1], out int currentId) && currentId == deleteId)
                    {
                        Console.Write($"Are you sure you want to delete Gift Idea {deleteId} with Title '{parts[3]}'? [Y/N]: ");
                        string confirmation = Console.ReadLine() ?? string.Empty;
                        if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            lines.RemoveAt(i);
                            Console.WriteLine("Gift idea deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Deletion cancelled.");
                        }
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                Console.WriteLine("Gift Idea not found.");
            else
                File.WriteAllLines(dataFilePath, lines);
        }

        public static void SearchGiftIdeas(string dataFilePath)
        {
            Console.Write("Enter search keyword: ");
            string keyword = Console.ReadLine() ?? string.Empty;

            var lines = File.ReadAllLines(dataFilePath);
            bool anyFound = false;
            Console.WriteLine("Matching Gift Ideas:");
            Console.WriteLine("ID\tTimestamp\t\tTitle\tDescription\tAdditional Notes\tStatus");

            foreach (var line in lines)
            {
                if (line.StartsWith("GIFT|"))
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 7)
                    {
                        string id = parts[1];
                        string timestamp = parts[2];
                        string title = parts[3];
                        string description = parts[4];
                        string notes = parts[5];
                        string status = parts[6];

                        if (title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            notes.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.WriteLine($"{id}\t{timestamp}\t{title}\t{description}\t{notes}\t{status}");
                            anyFound = true;
                        }
                    }
                }
            }

            if (!anyFound)
                Console.WriteLine("No matching gift ideas found.");
        }

        private static int GetNextGiftIdeaId(string dataFilePath)
        {
            int maxId = 0;
            if (File.Exists(dataFilePath))
            {
                var lines = File.ReadAllLines(dataFilePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("GIFT|"))
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 2 && int.TryParse(parts[1], out int id))
                        {
                            maxId = Math.Max(maxId, id);
                        }
                    }
                }
            }
            return maxId + 1;
        }
    }
}
