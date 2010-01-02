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

using System;
using System.Reflection;
using Ninject.Activation;

#endregion

namespace Ninject.Extensions.Interception.Request
{
    /// <summary>
    /// Describes a method call on a proxied contextualized instance.
    /// </summary>
    public interface IProxyRequest
    {
        /// <summary>
        /// Gets the kernel that created the target instance.
        /// </summary>
        IKernel Kernel { get; }

        /// <summary>
        /// Gets the context in which the target instance was activated.
        /// </summary>
        IContext Context { get; }

        /// <summary>
        /// Gets the target instance.
        /// </summary>
        object Target { get; }

        /// <summary>
        /// Gets the method that will be called on the target instance.
        /// </summary>
        MethodInfo Method { get; }

        /// <summary>
        /// Gets the arguments to the method.
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// Gets the generic type arguments for the method.
        /// </summary>
        Type[] GenericArguments { get; }

        /// <summary>
        /// Gets a value indicating whether the request has generic arguments.
        /// </summary>
        bool HasGenericArguments { get; }
    }
}