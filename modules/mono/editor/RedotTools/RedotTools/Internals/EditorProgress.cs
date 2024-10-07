using System;
using System.Runtime.CompilerServices;
using Redot;
using Redot.NativeInterop;

namespace RedotTools.Internals
{
    public class EditorProgress : IDisposable
    {
        public string Task { get; }

        public EditorProgress(string task, string label, int amount, bool canCancel = false)
        {
            Task = task;
            using redot_string taskIn = Marshaling.ConvertStringToNative(task);
            using redot_string labelIn = Marshaling.ConvertStringToNative(label);
            Internal.redot_icall_EditorProgress_Create(taskIn, labelIn, amount, canCancel);
        }

        ~EditorProgress()
        {
            // Should never rely on the GC to dispose EditorProgress.
            // It should be disposed immediately when the task finishes.
            GD.PushError("EditorProgress disposed by the Garbage Collector");
            Dispose();
        }

        public void Dispose()
        {
            using redot_string taskIn = Marshaling.ConvertStringToNative(Task);
            Internal.redot_icall_EditorProgress_Dispose(taskIn);
            GC.SuppressFinalize(this);
        }

        public void Step(string state, int step = -1, bool forceRefresh = true)
        {
            using redot_string taskIn = Marshaling.ConvertStringToNative(Task);
            using redot_string stateIn = Marshaling.ConvertStringToNative(state);
            Internal.redot_icall_EditorProgress_Step(taskIn, stateIn, step, forceRefresh);
        }

        public bool TryStep(string state, int step = -1, bool forceRefresh = true)
        {
            using redot_string taskIn = Marshaling.ConvertStringToNative(Task);
            using redot_string stateIn = Marshaling.ConvertStringToNative(state);
            return Internal.redot_icall_EditorProgress_Step(taskIn, stateIn, step, forceRefresh);
        }
    }
}
