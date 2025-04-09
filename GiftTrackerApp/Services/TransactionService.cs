using System;
using System.IO;

namespace GiftTrackerApp.Services
{
    public static class TransactionService
    {
        public static void SimulateGiftCardPurchase(string dataFilePath)
        {
            Console.Write("Enter gift card type (e.g., Birthday, Wedding, etc.): ");
            string type = Console.ReadLine() ?? string.Empty;
            string transactionId = Guid.NewGuid().ToString();
            string amount = "50";
            string transactionRecord = $"TRANSACTION|{DateTime.Now}|{transactionId}|{type}|{amount}";
            File.AppendAllText(dataFilePath, transactionRecord + Environment.NewLine);
            Console.WriteLine($"Simulated {type} gift card purchase complete.");
            Console.WriteLine($"Transaction ID: {transactionId}, Amount: ${amount}");
        }

        public static void ViewTransactionHistory(string dataFilePath)
        {
            Console.WriteLine("Transaction History:");
            if (File.Exists(dataFilePath))
            {
                string[] lines = File.ReadAllLines(dataFilePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("TRANSACTION"))
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            else
            {
                Console.WriteLine("No transaction records found.");
            }
        }

        public static void AnalyzeSpendingTrends(string dataFilePath)
        {
            if (!File.Exists(dataFilePath))
            {
                Console.WriteLine("No transaction records found.");
                return;
            }

            string[] lines = File.ReadAllLines(dataFilePath);
            decimal totalSpending = 0;
            int transactionCount = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("TRANSACTION"))
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 5 && decimal.TryParse(parts[4], out decimal amount))
                    {
                        totalSpending += amount;
                        transactionCount++;
                    }
                }
            }

            if (transactionCount == 0)
            {
                Console.WriteLine("No transaction records found.");
            }
            else
            {
                Console.WriteLine($"Total Transactions: {transactionCount}");
                Console.WriteLine($"Total Spending: ${totalSpending}");
                if (totalSpending > 100)
                    Console.WriteLine("Alert: Spending exceeds the threshold!");
            }
        }
    }
}
