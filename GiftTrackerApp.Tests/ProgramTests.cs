using System;
using System.IO;
using GiftTrackerApp;
using Xunit;

namespace GiftTrackerApp.Tests
{
    public class ProgramTests : IDisposable
    {
        private readonly string _tempDir;
        public ProgramTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "GiftTrackerAppTest_" + Guid.NewGuid());
            Directory.CreateDirectory(_tempDir);
            Directory.SetCurrentDirectory(_tempDir);
        }

        [Fact]
        public void Main_ExitsWhenOption8Chosen()
        {
            string input = "NoExistingUser" + Environment.NewLine + "8" + Environment.NewLine;
            var originalIn = Console.In;
            var originalOut = Console.Out;
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                Program.Main(Array.Empty<string>());
                var output = sw.ToString();
                Assert.Contains("Exiting the application", output);
            }
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        [Fact]
        public void Main_DisplaysMainMenu()
        {
            string input = "NewUser" + Environment.NewLine + "8" + Environment.NewLine;
            var originalIn = Console.In;
            var originalOut = Console.Out;
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                Program.Main(Array.Empty<string>());
                var output = sw.ToString();
                Assert.Contains("--- Main Menu ---", output);
                Assert.Contains("1. Add Gift Idea", output);
                Assert.Contains("2. Edit Gift Idea", output);
                Assert.Contains("3. Delete Gift Idea", output);
                Assert.Contains("4. Search Gift Ideas", output);
                Assert.Contains("5. View Gifts", output);
                Assert.Contains("6. View Summary", output);
                Assert.Contains("7. Log Out", output);
                Assert.Contains("8. Quit", output);
                Assert.Contains("Enter your option (1-8):", output);
            }
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        public void Dispose()
        {
            try
            {
                Directory.SetCurrentDirectory(Path.GetTempPath());
                if (Directory.Exists(_tempDir))
                    Directory.Delete(_tempDir, true);
            }
            catch { }
        }
    }
}
