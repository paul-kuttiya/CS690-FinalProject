using System;

namespace GiftTrackerApp.Helpers
{
    public static class ConsoleHelper
    {
        // Read a line of text, with an optional default value.
        public static string ReadString(string prompt, string? defaultValue = null)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) && defaultValue != null)
                return defaultValue;
            return input ?? string.Empty;
        }

        // Read an integer, repeat until valid or use default.
        public static int ReadInt(string prompt, int? defaultValue = null)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) && defaultValue.HasValue)
                    return defaultValue.Value;
                if (int.TryParse(input, out var value))
                    return value;
                Console.WriteLine("Invalid input. Enter a numeric value.");
            }
        }

        // Read a floating number, repeat until valid or use default.
        public static float ReadFloat(string prompt, float? defaultValue = null)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) && defaultValue.HasValue)
                    return defaultValue.Value;
                if (float.TryParse(input, out var value))
                    return value;
                Console.WriteLine("Invalid input. Enter a numeric value.");
            }
        }

        // Pause until the user presses Enter.
        public static void Pause()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
