using System;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Locator;

namespace RedotTools.ProjectEditor
{
    public sealed class MSBuildProject
    {
        internal ProjectRootElement Root { get; set; }

        public bool HasUnsavedChanges { get; set; }

        public void Save() => Root.Save();

        public MSBuildProject(ProjectRootElement root)
        {
            Root = root;
        }
    }

    public static class ProjectUtils
    {
        public static void MSBuildLocatorRegisterLatest(out Version version, out string path)
        {
            var instance = MSBuildLocator.QueryVisualStudioInstances()
                .OrderByDescending(x => x.Version)
                .First();
            MSBuildLocator.RegisterInstance(instance);
            version = instance.Version;
            path = instance.MSBuildPath;
        }

        public static void MSBuildLocatorRegisterMSBuildPath(string msbuildPath)
            => MSBuildLocator.RegisterMSBuildPath(msbuildPath);

        public static MSBuildProject? Open(string path)
        {
            var root = ProjectRootElement.Open(path);
            return root != null ? new MSBuildProject(root) : null;
        }

        public static void MigrateToProjectSdksStyle(MSBuildProject project, string projectName)
        {
            var origRoot = project.Root;

            if (!string.IsNullOrEmpty(origRoot.Sdk))
                return;

            project.Root = ProjectGenerator.GenGameProject(projectName);
            project.Root.FullPath = origRoot.FullPath;
            project.HasUnsavedChanges = true;
        }

        public static void EnsureRedotSdkIsUpToDate(MSBuildProject project)
        {
            var root = project.Root;
            string RedotSdkAttrValue = ProjectGenerator.RedotSdkAttrValue;

            if (!string.IsNullOrEmpty(root.Sdk) &&
                root.Sdk.Trim().Equals(RedotSdkAttrValue, StringComparison.OrdinalIgnoreCase))
                return;

            root.Sdk = RedotSdkAttrValue;
            project.HasUnsavedChanges = true;
        }
    }
}
