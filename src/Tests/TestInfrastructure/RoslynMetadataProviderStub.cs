using System;
using System.Linq;
using EnvDTE;
using Microsoft.CodeAnalysis.MSBuild;
using Typewriter.Configuration;
using Typewriter.Metadata.Interfaces;
using Typewriter.Metadata.Providers;
using Typewriter.Metadata.Roslyn;

namespace Typewriter.Tests.TestInfrastructure
{
    public sealed class RoslynMetadataProviderStub
        : IMetadataProvider,
            IDisposable
    {
        private readonly Microsoft.CodeAnalysis.Workspace _workspace;

        public RoslynMetadataProviderStub(DTE dte)
        {
            var solutionPath = dte.Solution.FullName;
            var msBuildWorkspace = MSBuildWorkspace.Create();
            msBuildWorkspace.OpenSolutionAsync(solutionPath).GetAwaiter().GetResult();
            _workspace = msBuildWorkspace;
        }

        public IFileMetadata GetFile(string path, Settings settings, Action<string[]> requestRender)
        {
            var document = _workspace.CurrentSolution.GetDocumentIdsWithFilePath(path).FirstOrDefault();
            if (document != null)
            {
                return new RoslynFileMetadata(_workspace.CurrentSolution.GetDocument(document), settings, requestRender);
            }

            return null;
        }

        public void Dispose()
        {
            _workspace?.Dispose();
        }
    }
}
