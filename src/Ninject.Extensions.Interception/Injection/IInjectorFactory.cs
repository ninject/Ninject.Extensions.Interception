#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System.Reflection;
using Ninject.Components;

#endregion

namespace Ninject.Extensions.Interception.Injection
{
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
        IMethodInjector GetInjector( MethodInfo method );
    }
}