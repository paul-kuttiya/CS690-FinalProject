using System;
using System.IO;
using Xunit;
using GiftTrackerApp;

namespace GiftTrackerApp.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Main_CreatesUserFile_AndDisplaysMenu()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var originalDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(tempDir);

            Console.SetIn(new StringReader("bob\n7\n"));
            var output = new StringWriter();
            Console.SetOut(output);

            Program.Main(new string[0]);

            Directory.SetCurrentDirectory(originalDir);

            var outStr = output.ToString();
            Assert.Contains("Gift Tracker for bob", outStr);
            var userFile = Path.Combine(tempDir, "Data", "bob.txt");
            Assert.True(File.Exists(userFile));
        }
    }
}
