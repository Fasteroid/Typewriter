using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Typewriter.Tests.LongPaths
{
    public class FileWriterTest
    {
        [Fact]
        public void ShouldCreateFileInDirectory()
        {
            var path = @"C:\Dev\SomeInnerPath\Github\Project\src\innerpath\another-subpath\A.Very.Long.Project.Name.Like.Dotnet.People.Like\packages\some-random-scope\some-package-name\src\lib\events\a-very-very-very-very-very-very-very-very-very-very-very-very-very-very-long-file-name.ts";
            var modifiedPath = path.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase)
            ? $@"\\?\UNC\{path.Substring(2, path.Length - 2)}"
                : $@"\\?\{path}";
            Action action = () =>
            {
                var dir = Path.GetDirectoryName(modifiedPath);
                if (!Directory.Exists(dir) && dir != null)
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(modifiedPath, "ok");
            };

            action.Should().NotThrow();
        }
    }
}