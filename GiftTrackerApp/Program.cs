using System;
using System.IO;
using System.Linq;
using GiftTrackerApp.Services;

namespace GiftTrackerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.CreateDirectory("Data");

            var userFiles = Directory.GetFiles("Data", "*.txt");
            string userName = string.Empty;
            if (userFiles.Any())
            {
                Console.WriteLine("Select an existing user by number, or press Enter to create a new user:");
                int i = 1;
                foreach (var file in userFiles)
                {
                    string existingUser = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine($"{i}. {existingUser}");
                    i++;
                }
                Console.Write("Your choice (number) or press Enter for new user: ");
                string selection = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(selection))
                {
                    Console.Write("Enter new user name: ");
                    userName = Console.ReadLine() ?? string.Empty;
                }
                else if (int.TryParse(selection, out int option) && option > 0 && option <= userFiles.Length)
                {
                    userName = Path.GetFileNameWithoutExtension(userFiles[option - 1]);
                    Console.WriteLine($"User '{userName}' selected.");
                }
                else
                {
                    Console.Write("Invalid selection. Enter new user name: ");
                    userName = Console.ReadLine() ?? string.Empty;
                }
            }
            else
            {
                Console.Write("No existing users found. Enter new user name: ");
                userName = Console.ReadLine() ?? string.Empty;
            }

            if (userName.Equals("Quit", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string dataFilePath = Path.Combine("Data", $"{userName.ToLower()}.txt");
            if (!File.Exists(dataFilePath))
            {
                File.WriteAllText(dataFilePath, string.Empty);
                Console.WriteLine($"New user file created for {userName}.");
            }
            else
            {
                Console.WriteLine($"User data loaded for {userName}.");
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Add Gift Idea");
                Console.WriteLine("2. Edit Gift Idea");
                Console.WriteLine("3. Delete Gift Idea");
                Console.WriteLine("4. Search Gift Ideas");
                Console.WriteLine("5. View Gifts");
                Console.WriteLine("6. View Summary");
                Console.WriteLine("7. Log Out");
                Console.WriteLine("8. Quit");
                Console.Write("Enter your option (1-8): ");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        GiftIdeaService.AddGiftIdea(dataFilePath);
                        PauseBeforeMainMenu();
                        break;
                    case "2":
                        GiftIdeaService.EditGiftIdea(dataFilePath);
                        PauseBeforeMainMenu();
                        break;
                    case "3":
                        GiftIdeaService.DeleteGiftIdea(dataFilePath);
                        PauseBeforeMainMenu();
                        break;
                    case "4":
                        GiftIdeaService.SearchGiftIdeas(dataFilePath);
                        PauseBeforeMainMenu();
                        break;
                    case "5":
                        GiftIdeaService.ViewGiftIdeas(dataFilePath);
                        PauseBeforeMainMenu();
                        break;
                    case "6":
                        GiftIdeaService.ViewSummary(dataFilePath);
                        PauseBeforeMainMenu();
                        break;
                    case "7":
                        Console.WriteLine("Logging out...");
                        Main(args);
                        return;
                    case "8":
                        Console.WriteLine("Exiting the application...");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        PauseBeforeMainMenu();
                        break;
                }
            }
        }

        private static void PauseBeforeMainMenu()
        {
            Console.WriteLine("\nPress Enter to return to Main Menu...");
            Console.ReadLine();
        }
    }
}
