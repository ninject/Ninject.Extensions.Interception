// -------------------------------------------------------------------------------------------------
// <copyright file="DynamicInjectorFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !NO_LCG

namespace Ninject.Extensions.Interception.Injection.Dynamic
{
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Creates instances of injectors that use generated <see cref="DynamicMethod"/> objects
    /// for invocation.
    /// </summary>
    public class DynamicInjectorFactory : InjectorFactoryBase
    {
        /// <summary>
        /// Creates a new method injector.
        /// </summary>
        /// <param name="method">The method that the injector will invoke.</param>
        /// <returns>A new injector for the method.</returns>
        protected override IMethodInjector CreateInjector(MethodInfo method)
        {
            return new DynamicMethodInjector(method);
        }
    }
}

#endif //!NO_LCG