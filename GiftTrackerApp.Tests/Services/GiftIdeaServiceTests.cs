using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GiftTrackerApp.Services;
using Xunit;

namespace GiftTrackerApp.Tests
{
    public class GiftIdeaServiceTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _dataFilePath;
        public GiftIdeaServiceTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "GiftIdeaServiceTest_" + Guid.NewGuid());
            Directory.CreateDirectory(_tempDir);
            _dataFilePath = Path.Combine(_tempDir, "testData.txt");
            if (!File.Exists(_dataFilePath))
            {
                File.WriteAllText(_dataFilePath, string.Empty);
            }
        }

        [Fact]
        public void AddGiftIdea_CreatesValidEntry()
        {
            string input = string.Join(Environment.NewLine, new string[]
            {
                "Birthday Gift",
                "For John's birthday",
                "Buy online if possible",
                "Friend",
                "50"
            });
            var originalIn = Console.In;
            var originalOut = Console.Out;
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                GiftIdeaService.AddGiftIdea(_dataFilePath);
            }
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);

            var lines = File.ReadAllLines(_dataFilePath);
            Assert.Single(lines);

            var parts = lines[0].Split('|');
            Assert.Equal(8, parts.Length);
            Assert.Equal("GIFT", parts[0]);
            Assert.Matches(new Regex(@"^\d+$"), parts[1]);
            DateTime.Parse(parts[2]);
            Assert.Equal("Birthday Gift", parts[3]);
            Assert.Equal("For John's birthday", parts[4]);
            Assert.Equal("Buy online if possible", parts[5]);
            Assert.Equal("Friend", parts[6]);
            Assert.Equal("50", parts[7]);
        }

        [Fact]
        public void EditGiftIdea_UpdatesExistingEntry()
        {
            string initialLine = $"GIFT|1|{DateTime.Now}|Original Title|Original Description|Original Notes|Friend|50";
            File.WriteAllText(_dataFilePath, initialLine + Environment.NewLine);

            string input = string.Join(Environment.NewLine, new string[]
            {
                "1",
                "New Title",
                "", 
                "Updated Notes",
                "",
                ""
            });
            var originalIn = Console.In;
            var originalOut = Console.Out;
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                GiftIdeaService.EditGiftIdea(_dataFilePath);
            }
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);

            var lines = File.ReadAllLines(_dataFilePath);
            Assert.Single(lines);
            var parts = lines[0].Split('|');
            Assert.Equal("1", parts[1]);
            Assert.Equal("New Title", parts[3]);
            Assert.Equal("Original Description", parts[4]);
            Assert.Equal("Updated Notes", parts[5]);
            Assert.Equal("Friend", parts[6]);
            Assert.Equal("50", parts[7]);
        }

        [Fact]
        public void DeleteGiftIdea_DeletesEntry_WhenConfirmed()
        {
            string initialLine = $"GIFT|1|{DateTime.Now}|Gift Title|Description|Notes|Friend|50";
            File.WriteAllText(_dataFilePath, initialLine + Environment.NewLine);

            string input = string.Join(Environment.NewLine, new string[]
            {
                "1",
                "Y"
            });
            var originalIn = Console.In;
            var originalOut = Console.Out;
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                GiftIdeaService.DeleteGiftIdea(_dataFilePath);
            }
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);

            var lines = File.ReadAllLines(_dataFilePath);
            Assert.Empty(lines.Where(l => l.StartsWith("GIFT|")));
        }

        [Fact]
        public void DeleteGiftIdea_DoesNotDeleteEntry_WhenCancelled()
        {
            string initialLine = $"GIFT|1|{DateTime.Now}|Gift Title|Description|Notes|Friend|50";
            File.WriteAllText(_dataFilePath, initialLine + Environment.NewLine);

            string input = string.Join(Environment.NewLine, new string[]
            {
                "1",
                "N"
            });
            var originalIn = Console.In;
            var originalOut = Console.Out;
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                GiftIdeaService.DeleteGiftIdea(_dataFilePath);
            }
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);

            var lines = File.ReadAllLines(_dataFilePath);
            Assert.Single(lines.Where(l => l.StartsWith("GIFT|")));
        }

        [Fact]
        public void SearchGiftIdeas_DisplaysMatchingEntry()
        {
            string entry = $"GIFT|1|{DateTime.Now}|Birthday Cake|A delicious cake|Notes|Family|30";
            File.WriteAllText(_dataFilePath, entry + Environment.NewLine);

            string input = "cake" + Environment.NewLine;
            var originalIn = Console.In;
            var originalOut = Console.Out;
            string output;
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);
                GiftIdeaService.SearchGiftIdeas(_dataFilePath);
                output = sw.ToString();
            }
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);

            Assert.Contains("Matching Gift Ideas", output);
            Assert.Contains("Birthday Cake", output);
        }

        [Fact]
        public void ViewGiftIdeas_DisplaysAllEntries()
        {
            string entry1 = $"GIFT|1|{DateTime.Now}|Gift One|Desc One|Notes One|Friend|20";
            string entry2 = $"GIFT|2|{DateTime.Now}|Gift Two|Desc Two|Notes Two|Family|40";
            File.WriteAllText(_dataFilePath, entry1 + Environment.NewLine + entry2 + Environment.NewLine);

            var originalOut = Console.Out;
            string output;
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                GiftIdeaService.ViewGiftIdeas(_dataFilePath);
                output = sw.ToString();
            }
            Console.SetOut(originalOut);

            Assert.Contains("Gift Ideas:", output);
            Assert.Contains("Gift One", output);
            Assert.Contains("Gift Two", output);
        }

        [Fact]
        public void ViewSummary_DisplaysCorrectTotals()
        {
            string entry1 = $"GIFT|1|{DateTime.Now}|Gift One|Desc One|Notes One|Friend|20";
            string entry2 = $"GIFT|2|{DateTime.Now}|Gift Two|Desc Two|Notes Two|Friend|40";
            string entry3 = $"GIFT|3|{DateTime.Now}|Gift Three|Desc Three|Notes Three|Family|30";
            File.WriteAllText(_dataFilePath, string.Join(Environment.NewLine, new[] { entry1, entry2, entry3 }));

            var originalOut = Console.Out;
            string output;
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                GiftIdeaService.ViewSummary(_dataFilePath);
                output = sw.ToString();
            }
            Console.SetOut(originalOut);

            Assert.Contains("Gift Summary", output);
            Assert.Contains("Friend", output);
            Assert.Contains("Family", output);
            Assert.Contains("Overall Total Gifts: 3", output);
            Assert.Contains("Overall Total Price: 90", output);
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_tempDir))
                    Directory.Delete(_tempDir, true);
            }
            catch { }
        }
    }
}
