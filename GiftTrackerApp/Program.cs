using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using GiftTrackerApp.Helpers;
using GiftTrackerApp.Models;
using GiftTrackerApp.Repositories;
using GiftTrackerApp.Services;

namespace GiftTrackerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Prepare data directory.
            Directory.CreateDirectory("Data");

            // Select or create user.
            var userFiles = Directory.GetFiles("Data", "*.txt");
            string userName;
            if (userFiles.Any())
            {
                Console.WriteLine("Select an existing user by number, or press Enter to create a new user:");
                for (int i = 0; i < userFiles.Length; i++)
                {
                    var existing = Path.GetFileNameWithoutExtension(userFiles[i]);
                    Console.WriteLine($"{i + 1}. {existing}");
                }
                var sel = ConsoleHelper.ReadString("Your choice (number) or press Enter for new user: ");
                if (int.TryParse(sel, out int idx) && idx > 0 && idx <= userFiles.Length)
                {
                    userName = Path.GetFileNameWithoutExtension(userFiles[idx - 1]);
                    Console.WriteLine($"User '{userName}' selected.");
                }
                else
                {
                    userName = ConsoleHelper.ReadString("Enter new user name: ");
                }
            }
            else
            {
                userName = ConsoleHelper.ReadString("No existing users found. Enter new user name: ");
            }

            var dataFilePath = Path.Combine("Data", $"{userName.ToLower()}.txt");
            var repo = new GiftIdeaRepository(dataFilePath);
            var service = new GiftIdeaService(repo);

            bool exit = false;
            while (!exit)
            {
                // Display a menu
                Console.WriteLine($"\n--- Gift Tracker for {userName} ---");
                Console.WriteLine("1. Add\n2. Edit\n3. Delete\n4. Search\n5. View All\n6. Summary\n7. Quit");
                var choice = ConsoleHelper.ReadString("Select option: ");
                switch (choice)
                {
                    // Add a new idea.
                    case "1":
                        var newIdea = new GiftIdea
                        {
                            Title = ConsoleHelper.ReadString("Title: "),
                            Description = ConsoleHelper.ReadString("Description: "),
                            Notes = ConsoleHelper.ReadString("Notes: "),
                            GiftFor = ConsoleHelper.ReadString("Gift For: "),
                            Price = ConsoleHelper.ReadFloat("Price: ")
                        };
                        service.Add(newIdea);
                        Console.WriteLine("Added.");
                        ConsoleHelper.Pause();
                        Console.Clear();
                        break;
                    // Edit an existing idea.
                    case "2":
                        var editId = ConsoleHelper.ReadInt("ID to edit: ");
                        Console.Clear();
                        var toEdit = service.GetAll().FirstOrDefault(i => i.Id == editId);
                        if (toEdit == null)
                            Console.WriteLine("Not found.");
                        else
                        {
                            Console.WriteLine("Current Details:");
                            Console.WriteLine(toEdit);
                            toEdit.Title = ConsoleHelper.ReadString("New Title (Enter to keep): ", toEdit.Title);
                            toEdit.Description = ConsoleHelper.ReadString("New Description (Enter to keep): ", toEdit.Description);
                            toEdit.Notes = ConsoleHelper.ReadString("New Notes (Enter to keep): ", toEdit.Notes);
                            toEdit.GiftFor = ConsoleHelper.ReadString("New Gift For (Enter to keep): ", toEdit.GiftFor);
                            toEdit.Price = ConsoleHelper.ReadFloat("New Price (Enter to keep): ", toEdit.Price);
                            Console.WriteLine(service.Update(toEdit) ? "Updated." : "Update failed.");
                        }
                        ConsoleHelper.Pause();
                        Console.Clear();
                        break;
                    // Delete an idea
                    case "3":
                        var delId = ConsoleHelper.ReadInt("ID to delete: ");
                        Console.Clear();
                        Console.WriteLine(service.Delete(delId) ? "Deleted." : "Delete failed.");
                        ConsoleHelper.Pause();
                        Console.Clear();
                        break;
                    // Search for ideas
                    case "4":
                        var keyword = ConsoleHelper.ReadString("Keyword: ");
                        Console.Clear();
                        var results = service.Search(keyword);
                        if (results.Any())
                            results.ForEach(i => Console.WriteLine(i));
                        else
                            Console.WriteLine("No matching gift ideas found.");
                        ConsoleHelper.Pause();
                        Console.Clear();
                        break;
                    // Show all ideas
                    case "5":
                        Console.Clear();
                        var all = service.GetAll();
                        if (all.Any())
                            all.ForEach(i => Console.WriteLine(i));
                        else
                            Console.WriteLine("No gift ideas found.");
                        ConsoleHelper.Pause();
                        Console.Clear();
                        break;
                    // Show a summary
                    case "6":
                        Console.Clear();
                        var summary = service.GetSummary();
                        Console.WriteLine("Gift Summary:");
                        Console.WriteLine($"Overall Count: {summary.count}");
                        Console.WriteLine($"Overall Total Price: {summary.total}");
                        ConsoleHelper.Pause();
                        Console.Clear();
                        break;
                    // Exit
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        ConsoleHelper.Pause();
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
