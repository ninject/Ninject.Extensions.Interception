// -------------------------------------------------------------------------------------------------
// <copyright file="ReflectionInjectorFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Injection.Reflection
{
    using System.Reflection;

    /// <summary>
    /// Creates instances of injectors that use reflection for invocation.
    /// </summary>
    public class ReflectionInjectorFactory : InjectorFactoryBase
    {
        /// <summary>
        /// Creates a new method injector.
        /// </summary>
        /// <param name="method">The method that the injector will invoke.</param>
        /// <returns>A new injector for the method.</returns>
        protected override IMethodInjector CreateInjector(MethodInfo method)
        {
            return new ReflectionMethodInjector(method);
        }
    }
}