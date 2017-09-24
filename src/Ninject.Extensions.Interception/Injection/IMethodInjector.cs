// -------------------------------------------------------------------------------------------------
// <copyright file="IMethodInjector.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Injection
{
    using System.Reflection;

    /// <summary>
    /// Provides a way to inject one or more values into the specified member.
    /// </summary>
    public interface IMethodInjector : IInjector<MethodInfo>
    {
        /// <summary>
        /// Calls the method associated with the injector.
        /// </summary>
        /// <param name="target">The instance on which to call the method.</param>
        /// <param name="arguments">The arguments to pass to the method.</param>
        /// <returns>The return value of the method.</returns>
        object Invoke(object target, object[] arguments);
    }
}