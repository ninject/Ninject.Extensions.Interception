// -------------------------------------------------------------------------------------------------
// <copyright file="IInjectorFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Injection
{
    using System.Reflection;
    using Ninject.Components;

    /// <summary>
    /// Creates instances of injectors.
    /// </summary>
    public interface IInjectorFactory : INinjectComponent
    {
        /// <summary>
        /// Gets an injector for the specified method.
        /// </summary>
        /// <param name="method">The method that the injector will invoke.</param>
        /// <returns>A new injector for the method.</returns>
        IMethodInjector GetInjector(MethodInfo method);
    }
}