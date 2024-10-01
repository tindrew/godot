using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Redot.SourceGenerators
{
    [Generator]
    public class RedotPluginsInitializerGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.IsRedotToolsProject() || context.IsRedotSourceGeneratorDisabled("RedotPluginsInitializer"))
                return;

            string source =
                @"using System;
using System.Runtime.InteropServices;
using Redot.Bridge;
using Redot.NativeInterop;

namespace RedotPlugins.Game
{
    internal static partial class Main
    {
        [UnmanagedCallersOnly(EntryPoint = ""Redotsharp_game_main_init"")]
        private static Redot_bool InitializeFromGameProject(IntPtr RedotDllHandle, IntPtr outManagedCallbacks,
            IntPtr unmanagedCallbacks, int unmanagedCallbacksSize)
        {
            try
            {
                DllImportResolver dllImportResolver = new RedotDllImportResolver(RedotDllHandle).OnResolveDllImport;

                var coreApiAssembly = typeof(global::Redot.RedotObject).Assembly;

                NativeLibrary.SetDllImportResolver(coreApiAssembly, dllImportResolver);

                NativeFuncs.Initialize(unmanagedCallbacks, unmanagedCallbacksSize);

                ManagedCallbacks.Create(outManagedCallbacks);

                ScriptManagerBridge.LookupScriptsInAssembly(typeof(global::RedotPlugins.Game.Main).Assembly);

                return Redot_bool.True;
            }
            catch (Exception e)
            {
                global::System.Console.Error.WriteLine(e);
                return false.ToRedotBool();
            }
        }
    }
}
";

            context.AddSource("RedotPlugins.Game.generated",
                SourceText.From(source, Encoding.UTF8));
        }
    }
}
