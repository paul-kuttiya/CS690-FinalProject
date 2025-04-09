using System;
using System.IO;
using GiftTrackerApp.Services;

namespace GiftTrackerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter User Name or type \"Quit\" to exit: ");
            string userName = Console.ReadLine() ?? string.Empty;

            if (userName.Equals("Quit", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Directory.CreateDirectory("Data");
            string dataFilePath = Path.Combine("Data", $"{userName}.txt");

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
                Console.WriteLine("5. Simulate Gift Card Purchase");
                Console.WriteLine("6. View Transaction History");
                Console.WriteLine("7. Analyze Spending Trends");
                Console.WriteLine("8. Log Out");
                Console.WriteLine("9. Quit");
                Console.Write("Enter your option (1-9): ");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        GiftIdeaService.AddGiftIdea(dataFilePath);
                        break;
                    case "2":
                        GiftIdeaService.EditGiftIdea(dataFilePath);
                        break;
                    case "3":
                        GiftIdeaService.DeleteGiftIdea(dataFilePath);
                        break;
                    case "4":
                        GiftIdeaService.SearchGiftIdeas(dataFilePath);
                        break;
                    case "5":
                        TransactionService.SimulateGiftCardPurchase(dataFilePath);
                        break;
                    case "6":
                        TransactionService.ViewTransactionHistory(dataFilePath);
                        break;
                    case "7":
                        TransactionService.AnalyzeSpendingTrends(dataFilePath);
                        break;
                    case "8":
                        Console.WriteLine("Logging out...");
                        Main(args);
                        return;
                    case "9":
                        Console.WriteLine("Exiting the application...");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }
}
