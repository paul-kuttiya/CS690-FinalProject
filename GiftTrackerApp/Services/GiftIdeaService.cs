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

            Console.Write("Enter Gift For (e.g., Mom, Friend, etc.): ");
            string giftFor = Console.ReadLine() ?? string.Empty;

            int giftPrice;
            while (true)
            {
                Console.Write("Enter Gift Price: ");
                string priceInput = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(priceInput, out giftPrice))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please enter a numeric value for price.");
            }

            int newId = GetNextGiftIdeaId(dataFilePath);
            string entry = $"GIFT|{newId}|{DateTime.Now}|{title}|{description}|{notes}|{giftFor}|{giftPrice}";
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
                    if (parts.Length >= 8 && int.TryParse(parts[1], out int currentId) && currentId == editId)
                    {
                        found = true;
                        Console.WriteLine("Current Details:");
                        Console.WriteLine($"Title: {parts[3]}");
                        Console.WriteLine($"Description: {parts[4]}");
                        Console.WriteLine($"Additional Notes: {parts[5]}");
                        Console.WriteLine($"Gift For: {parts[6]}");
                        Console.WriteLine($"Price: {parts[7]}");

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

                        Console.Write("Enter new Gift For (press Enter to keep current): ");
                        string newGiftFor = Console.ReadLine() ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(newGiftFor))
                            newGiftFor = parts[6];

                        int newPrice;
                        while (true)
                        {
                            Console.Write("Enter new Gift Price (press Enter to keep current): ");
                            string priceInput = Console.ReadLine() ?? string.Empty;
                            if (string.IsNullOrWhiteSpace(priceInput))
                            {
                                newPrice = int.Parse(parts[7]);
                                break;
                            }
                            if (int.TryParse(priceInput, out newPrice))
                            {
                                break;
                            }
                            Console.WriteLine("Invalid input. Please enter a numeric value for price.");
                        }

                        string updatedLine = $"GIFT|{parts[1]}|{parts[2]}|{newTitle}|{newDescription}|{newNotes}|{newGiftFor}|{newPrice}";
                        lines[i] = updatedLine;
                        Console.WriteLine("Gift idea updated successfully.");
                        break;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("Gift Idea not found.");
            }
            else
            {
                File.WriteAllLines(dataFilePath, lines);
            }
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
                    if (parts.Length >= 8 && int.TryParse(parts[1], out int currentId) && currentId == deleteId)
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
            Console.WriteLine("{0,-5} {1,-22} {2,-15} {3,-20} {4,-25} {5,-15} {6,-6}", "ID", "Timestamp", "Title", "Description", "Additional Notes", "Gift For", "Price");

            foreach (var line in lines)
            {
                if (line.StartsWith("GIFT|"))
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 8)
                    {
                        string id = parts[1];
                        string timestamp = Convert.ToDateTime(parts[2]).ToString("MM/dd/yyyy HH:mm:ss");
                        string title = parts[3];
                        string description = parts[4];
                        string notes = parts[5];
                        string giftFor = parts[6];
                        string price = parts[7];

                        if (title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            notes.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            giftFor.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.WriteLine("{0,-5} {1,-22} {2,-15} {3,-20} {4,-25} {5,-15} {6,-6}", id, timestamp, title, description, notes, giftFor, price);
                            anyFound = true;
                        }
                    }
                }
            }

            if (!anyFound)
                Console.WriteLine("No matching gift ideas found.");
        }

        public static void ViewGiftIdeas(string dataFilePath)
        {
            Console.WriteLine("Gift Ideas:");
            Console.WriteLine("{0,-5} {1,-22} {2,-15} {3,-20} {4,-25} {5,-15} {6,-6}", "ID", "Timestamp", "Title", "Description", "Additional Notes", "Gift For", "Price");
            var lines = File.ReadAllLines(dataFilePath);
            bool anyFound = false;
            foreach (var line in lines)
            {
                if (line.StartsWith("GIFT|"))
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 8)
                    {
                        string id = parts[1];
                        string timestamp = Convert.ToDateTime(parts[2]).ToString("MM/dd/yyyy HH:mm:ss");
                        string title = parts[3];
                        string description = parts[4];
                        string notes = parts[5];
                        string giftFor = parts[6];
                        string price = parts[7];

                        Console.WriteLine("{0,-5} {1,-22} {2,-15} {3,-20} {4,-25} {5,-15} {6,-6}", id, timestamp, title, description, notes, giftFor, price);
                        anyFound = true;
                    }
                }
            }
            if (!anyFound)
                Console.WriteLine("No gift ideas found.");
        }

        public static void ViewSummary(string dataFilePath)
        {
            var lines = File.ReadAllLines(dataFilePath);
            int overallCount = 0;
            int overallTotalPrice = 0;

            var groupSummary = new Dictionary<string, (int count, int totalPrice)>(StringComparer.OrdinalIgnoreCase);

            foreach (var line in lines)
            {
                if (line.StartsWith("GIFT|"))
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 8 && int.TryParse(parts[7], out int giftPrice))
                    {
                        overallCount++;
                        overallTotalPrice += giftPrice;

                        string giftFor = parts[6].Trim();
                        if (groupSummary.ContainsKey(giftFor))
                        {
                            var current = groupSummary[giftFor];
                            groupSummary[giftFor] = (current.count + 1, current.totalPrice + giftPrice);
                        }
                        else
                        {
                            groupSummary[giftFor] = (1, giftPrice);
                        }
                    }
                }
            }

            Console.WriteLine("Gift Summary:");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("{0,-15} {1,-10} {2,-10}", "Gift For", "Count", "Total Price");
            Console.WriteLine("-------------------------------------------------------------");
            foreach (var entry in groupSummary)
            {
                Console.WriteLine("{0,-15} {1,-10} {2,-10}", entry.Key, entry.Value.count, entry.Value.totalPrice);
            }
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("Overall Total Gifts: " + overallCount);
            Console.WriteLine("Overall Total Price: " + overallTotalPrice);
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
