using System;
using System.IO;
using GiftTrackerApp.Helpers;
using Xunit;

namespace GiftTrackerApp.Tests
{
    public class ConsoleHelperTests
    {
        [Fact]
        public void ReadString_ReturnsInput_WhenProvided()
        {
            var input = "hello\n";
            Console.SetIn(new StringReader(input));
            var result = ConsoleHelper.ReadString("Prompt: ");
            Assert.Equal("hello", result);
        }

        [Fact]
        public void ReadString_ReturnsDefault_WhenEmpty()
        {
            var input = "\n";
            Console.SetIn(new StringReader(input));
            var result = ConsoleHelper.ReadString("Prompt: ", "def");
            Assert.Equal("def", result);
        }

        [Fact]
        public void ReadInt_ReturnsParsedValue()
        {
            var input = "42\n";
            Console.SetIn(new StringReader(input));
            var value = ConsoleHelper.ReadInt("Prompt: ");
            Assert.Equal(42, value);
        }

        [Fact]
        public void ReadInt_UsesDefault_OnEmpty()
        {
            var input = "\n";
            Console.SetIn(new StringReader(input));
            var value = ConsoleHelper.ReadInt("Prompt: ", 7);
            Assert.Equal(7, value);
        }

        [Fact]
        public void ReadFloat_ReturnsParsedValue()
        {
            var input = "3.14\n";
            Console.SetIn(new StringReader(input));
            var value = ConsoleHelper.ReadFloat("Prompt: ");
            Assert.Equal(3.14f, value);
        }

        [Fact]
        public void ReadFloat_UsesDefault_OnEmpty()
        {
            var input = "\n";
            Console.SetIn(new StringReader(input));
            var value = ConsoleHelper.ReadFloat("Prompt: ", 2.5f);
            Assert.Equal(2.5f, value);
        }
    }
}
