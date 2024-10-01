using System.Diagnostics.CodeAnalysis;
using System.IO;
using Redot;
using Redot.NativeInterop;
using RedotTools.Core;
using static RedotTools.Internals.Globals;

namespace RedotTools.Internals
{
    public static class RedotSharpDirs
    {
        public static string ResMetadataDir
        {
            get
            {
                Internal.Redot_icall_RedotSharpDirs_ResMetadataDir(out Redot_string dest);
                using (dest)
                    return Marshaling.ConvertStringToManaged(dest);
            }
        }

        public static string MonoUserDir
        {
            get
            {
                Internal.Redot_icall_RedotSharpDirs_MonoUserDir(out Redot_string dest);
                using (dest)
                    return Marshaling.ConvertStringToManaged(dest);
            }
        }

        public static string BuildLogsDirs
        {
            get
            {
                Internal.Redot_icall_RedotSharpDirs_BuildLogsDirs(out Redot_string dest);
                using (dest)
                    return Marshaling.ConvertStringToManaged(dest);
            }
        }

        public static string DataEditorToolsDir
        {
            get
            {
                Internal.Redot_icall_RedotSharpDirs_DataEditorToolsDir(out Redot_string dest);
                using (dest)
                    return Marshaling.ConvertStringToManaged(dest);
            }
        }


        public static string CSharpProjectName
        {
            get
            {
                Internal.Redot_icall_RedotSharpDirs_CSharpProjectName(out Redot_string dest);
                using (dest)
                    return Marshaling.ConvertStringToManaged(dest);
            }
        }

        [MemberNotNull("_projectAssemblyName", "_projectSlnPath", "_projectCsProjPath")]
        public static void DetermineProjectLocation()
        {
            _projectAssemblyName = (string?)ProjectSettings.GetSetting("dotnet/project/assembly_name");
            if (string.IsNullOrEmpty(_projectAssemblyName))
            {
                _projectAssemblyName = CSharpProjectName;
                ProjectSettings.SetSetting("dotnet/project/assembly_name", _projectAssemblyName);
            }

            string? slnParentDir = (string?)ProjectSettings.GetSetting("dotnet/project/solution_directory");
            if (string.IsNullOrEmpty(slnParentDir))
                slnParentDir = "res://";
            else if (!slnParentDir.StartsWith("res://", System.StringComparison.Ordinal))
                slnParentDir = "res://" + slnParentDir;

            // The csproj should be in the same folder as project.Redot.
            string csprojParentDir = "res://";

            _projectSlnPath = Path.Combine(ProjectSettings.GlobalizePath(slnParentDir),
                string.Concat(_projectAssemblyName, ".sln"));

            _projectCsProjPath = Path.Combine(ProjectSettings.GlobalizePath(csprojParentDir),
                string.Concat(_projectAssemblyName, ".csproj"));
        }

        private static string? _projectAssemblyName;
        private static string? _projectSlnPath;
        private static string? _projectCsProjPath;

        public static string ProjectAssemblyName
        {
            get
            {
                if (_projectAssemblyName == null)
                    DetermineProjectLocation();
                return _projectAssemblyName;
            }
        }

        public static string ProjectSlnPath
        {
            get
            {
                if (_projectSlnPath == null)
                    DetermineProjectLocation();
                return _projectSlnPath;
            }
        }

        public static string ProjectCsProjPath
        {
            get
            {
                if (_projectCsProjPath == null)
                    DetermineProjectLocation();
                return _projectCsProjPath;
            }
        }

        public static string ProjectBaseOutputPath
        {
            get
            {
                if (_projectCsProjPath == null)
                    DetermineProjectLocation();
                return Path.Combine(Path.GetDirectoryName(_projectCsProjPath)!, ".Redot", "mono", "temp", "bin");
            }
        }

        public static string LogsDirPathFor(string solution, string configuration)
            => Path.Combine(BuildLogsDirs, $"{solution.Md5Text()}_{configuration}");

        public static string LogsDirPathFor(string configuration)
            => LogsDirPathFor(ProjectSlnPath, configuration);
    }
}
