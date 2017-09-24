// -------------------------------------------------------------------------------------------------
// <copyright file="DynamicMethodInjector.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !NO_LCG

namespace Ninject.Extensions.Interception.Injection.Dynamic
{
    using System.Reflection;

    /// <summary>
    /// A method injector that uses a dynamically-generated <see cref="Invoker"/> for invocation.
    /// </summary>
    public class DynamicMethodInjector : InjectorBase<MethodInfo>, IMethodInjector
    {
        private Invoker invoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicMethodInjector"/> class.
        /// </summary>
        /// <param name="member">The method that will be injected.</param>
        public DynamicMethodInjector(MethodInfo member)
            : base(member)
        {
        }

        /// <summary>
        /// Calls the method associated with the injector.
        /// </summary>
        /// <param name="target">The instance on which to call the method.</param>
        /// <param name="arguments">The arguments to pass to the method.</param>
        /// <returns>The return value of the method.</returns>
        public object Invoke(object target, params object[] arguments)
        {
            if (this.invoker == null)
            {
                this.invoker = DynamicMethodFactory.CreateInvoker(this.Member);
            }

            return this.invoker.Invoke(target, arguments);
        }
    }
}

#endif //!NO_LCG