using System;
using FluentAssertions;
using Microsoft.IO;
using Typewriter.LongPaths;
using Xunit;

namespace Typewriter.Tests.LongPaths
{
    public class FileWriterTest
    {
        [Fact]
        public void ShouldCreateLongPath()
        {
            var tempPath = Path.GetTempPath();
            var filename = $"{new string('a', 250)}.txt";
            Action action = () => FileWriter.WriteAllText(
                Path.Combine(tempPath, filename),
                "Hello World!");
            action.Should().NotThrow();
        }
    }
}