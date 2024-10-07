using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Redot.NativeInterop;

namespace Redot
{
    /// <summary>
    /// Callable is a first class object which can be held in variables and passed to functions.
    /// It represents a given method in an Object, and is typically used for signal callbacks.
    /// </summary>
    /// <example>
    /// <code>
    /// public void PrintArgs(object ar1, object arg2, object arg3 = null)
    /// {
    ///     GD.PrintS(arg1, arg2, arg3);
    /// }
    ///
    /// public void Test()
    /// {
    ///     // This Callable object will call the PrintArgs method defined above.
    ///     Callable callable = new Callable(this, nameof(PrintArgs));
    ///     callable.Call("hello", "world"); // Prints "hello world null".
    ///     callable.Call(Vector2.Up, 42, callable); // Prints "(0, -1) 42 Node(Node.cs)::PrintArgs".
    ///     callable.Call("invalid"); // Invalid call, should have at least 2 arguments.
    /// }
    /// </code>
    /// </example>
    public readonly partial struct Callable
    {
        private readonly RedotObject _target;
        private readonly StringName _method;
        private readonly Delegate _delegate;
        private readonly unsafe delegate* managed<object, NativeVariantPtrArgs, out redot_variant, void> _trampoline;

        /// <summary>
        /// Object that contains the method.
        /// </summary>
        public RedotObject Target => _target;

        /// <summary>
        /// Name of the method that will be called.
        /// </summary>
        public StringName Method => _method;

        /// <summary>
        /// Delegate of the method that will be called.
        /// </summary>
        public Delegate Delegate => _delegate;

        /// <summary>
        /// Trampoline function pointer for dynamically invoking <see cref="Callable.Delegate"/>.
        /// </summary>
        public unsafe delegate* managed<object, NativeVariantPtrArgs, out redot_variant, void> Trampoline
            => _trampoline;

        /// <summary>
        /// Constructs a new <see cref="Callable"/> for the method called <paramref name="method"/>
        /// in the specified <paramref name="target"/>.
        /// </summary>
        /// <param name="target">Object that contains the method.</param>
        /// <param name="method">Name of the method that will be called.</param>
        public unsafe Callable(RedotObject target, StringName method)
        {
            _target = target;
            _method = method;
            _delegate = null;
            _trampoline = null;
        }

        private unsafe Callable(Delegate @delegate,
            delegate* managed<object, NativeVariantPtrArgs, out redot_variant, void> trampoline)
        {
            _target = @delegate?.Target as RedotObject;
            _method = null;
            _delegate = @delegate;
            _trampoline = trampoline;
        }

        private const int VarArgsSpanThreshold = 10;

        /// <summary>
        /// Calls the method represented by this <see cref="Callable"/>.
        /// Arguments can be passed and should match the method's signature.
        /// </summary>
        /// <param name="args">Arguments that will be passed to the method call.</param>
        /// <returns>The value returned by the method.</returns>
        public unsafe Variant Call(params Variant[] args)
        {
            using redot_callable callable = Marshaling.ConvertCallableToNative(this);

            int argc = args.Length;

            Span<redot_variant.movable> argsStoreSpan = argc <= VarArgsSpanThreshold ?
                stackalloc redot_variant.movable[VarArgsSpanThreshold] :
                new redot_variant.movable[argc];

            Span<IntPtr> argsSpan = argc <= VarArgsSpanThreshold ?
                stackalloc IntPtr[VarArgsSpanThreshold] :
                new IntPtr[argc];

            fixed (redot_variant* varargs = &MemoryMarshal.GetReference(argsStoreSpan).DangerousSelfRef)
            fixed (IntPtr* argsPtr = &MemoryMarshal.GetReference(argsSpan))
            {
                for (int i = 0; i < argc; i++)
                {
                    varargs[i] = (redot_variant)args[i].NativeVar;
                    argsPtr[i] = new IntPtr(&varargs[i]);
                }

                redot_variant ret = NativeFuncs.redotsharp_callable_call(callable,
                    (redot_variant**)argsPtr, argc, out redot_variant_call_error vcall_error);
                ExceptionUtils.DebugCheckCallError(callable, (redot_variant**)argsPtr, argc, vcall_error);
                return Variant.CreateTakingOwnershipOfDisposableValue(ret);
            }
        }

        /// <summary>
        /// Calls the method represented by this <see cref="Callable"/> in deferred mode, i.e. during the idle frame.
        /// Arguments can be passed and should match the method's signature.
        /// </summary>
        /// <param name="args">Arguments that will be passed to the method call.</param>
        public unsafe void CallDeferred(params Variant[] args)
        {
            using redot_callable callable = Marshaling.ConvertCallableToNative(this);

            int argc = args.Length;

            Span<redot_variant.movable> argsStoreSpan = argc <= VarArgsSpanThreshold ?
                stackalloc redot_variant.movable[VarArgsSpanThreshold] :
                new redot_variant.movable[argc];

            Span<IntPtr> argsSpan = argc <= VarArgsSpanThreshold ?
                stackalloc IntPtr[VarArgsSpanThreshold] :
                new IntPtr[argc];

            fixed (redot_variant* varargs = &MemoryMarshal.GetReference(argsStoreSpan).DangerousSelfRef)
            fixed (IntPtr* argsPtr = &MemoryMarshal.GetReference(argsSpan))
            {
                for (int i = 0; i < argc; i++)
                {
                    varargs[i] = (redot_variant)args[i].NativeVar;
                    argsPtr[i] = new IntPtr(&varargs[i]);
                }

                NativeFuncs.redotsharp_callable_call_deferred(callable, (redot_variant**)argsPtr, argc);
            }
        }

        /// <summary>
        /// <para>
        /// Constructs a new <see cref="Callable"/> using the <paramref name="trampoline"/>
        /// function pointer to dynamically invoke the given <paramref name="delegate"/>.
        /// </para>
        /// <para>
        /// The parameters passed to the <paramref name="trampoline"/> function are:
        /// </para>
        /// <list type="number">
        ///    <item>
        ///        <term>delegateObj</term>
        ///        <description>The given <paramref name="delegate"/>, upcast to <see cref="object"/>.</description>
        ///    </item>
        ///    <item>
        ///        <term>args</term>
        ///        <description>Array of <see cref="redot_variant"/> arguments.</description>
        ///    </item>
        ///    <item>
        ///        <term>ret</term>
        ///        <description>Return value of type <see cref="redot_variant"/>.</description>
        ///    </item>
        ///</list>
        /// <para>
        /// The delegate should be downcast to a more specific delegate type before invoking.
        /// </para>
        /// </summary>
        /// <example>
        /// Usage example:
        ///
        /// <code>
        ///     static void Trampoline(object delegateObj, NativeVariantPtrArgs args, out redot_variant ret)
        ///     {
        ///         if (args.Count != 1)
        ///             throw new ArgumentException($&quot;Callable expected {1} arguments but received {args.Count}.&quot;);
        ///
        ///         TResult res = ((Func&lt;int, string&gt;)delegateObj)(
        ///             VariantConversionCallbacks.GetToManagedCallback&lt;int&gt;()(args[0])
        ///         );
        ///
        ///         ret = VariantConversionCallbacks.GetToVariantCallback&lt;string&gt;()(res);
        ///     }
        ///
        ///     var callable = Callable.CreateWithUnsafeTrampoline((int num) =&gt; &quot;foo&quot; + num.ToString(), &amp;Trampoline);
        ///     var res = (string)callable.Call(10);
        ///     Console.WriteLine(res);
        /// </code>
        /// </example>
        /// <param name="delegate">Delegate method that will be called.</param>
        /// <param name="trampoline">Trampoline function pointer for invoking the delegate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Callable CreateWithUnsafeTrampoline(Delegate @delegate,
            delegate* managed<object, NativeVariantPtrArgs, out redot_variant, void> trampoline)
            => new(@delegate, trampoline);
    }
}
