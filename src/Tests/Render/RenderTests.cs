using Microsoft.IO;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Should;
using System.Linq;
using Typewriter.Generation;
using Typewriter.Tests.Render.WebApiControllerTests;
using Typewriter.Tests.TestInfrastructure;
using Xunit;

namespace Typewriter.Tests.Render
{
    [Trait(nameof(Render), "Roslyn"), Collection(nameof(RoslynFixture))]
    public class RoslynRenderTests : RenderTests
    {
        public RoslynRenderTests(RoslynFixture fixture, GlobalServiceProvider sp)
            : base(fixture, sp)
        {
        }
    }

    public abstract class RenderTests : TestInfrastructure.TestBase
    {
        protected RenderTests(ITestFixture fixture, GlobalServiceProvider sp)
            : base(fixture, sp)
        {
        }

        private void Assert<TClass>()
        {
            var type = typeof(TClass);
            var nsParts = type.FullName.Remove(0, 11).Split('.');

            var path = string.Join(@"\", nsParts);
            var tt = GetProjectItem("Tests\\Render\\WebApiController\\SingleFile.tstemplate");
            var t2 = GetProjectItem("Tests\\Render\\WebApiController\\SingleFile.tstemplate");
            var template = new Template(GetProjectItem(path + ".tstemplate"));
            var file = GetFile(path + ".cs");
            var result = GetFileContents(path + ".result");

            var output = template.Render(file, out var success);

            success.ShouldBeTrue();
            output.ShouldEqual(result);
        }

        [Fact]
        public void Expect_SingleFileMode_To_Render_Correctly()
        {
            var ts = Path.Combine(nameof(Tests), nameof(Render), "WebApiControllerTests", "SingleFile.tstemplate");
            var projectItem = GetProjectItem(ts);

            var pathModels = Path.Combine(nameof(Tests), "Render", "WebApiControllerTests", "SingleFileModels");

            pathModels = Path.Combine(SolutionDirectory, pathModels);

            var models = Directory.GetFiles(pathModels, "*.cs");

            var files = models.Select(x => GetFile(Path.Combine(pathModels, Path.GetFileName(x)))).ToArray();

            var template = new Template(projectItem);

            template.Settings.IsSingleFileMode.ShouldBeTrue();

            var tpl = template.Render(files, out var success);

            var resultFile = Path.Combine(nameof(Tests), nameof(Render), "WebApiControllerTests", Path.GetFileNameWithoutExtension(ts) + ".result");
            var content = GetFileContents(resultFile);
            success.ShouldBeTrue();

            tpl.ShouldEqual(content);
        }
    }
}
