// -------------------------------------------------------------------------------------------------
// <copyright file="IProxyRequest.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Request
{
    using System;
    using System.Reflection;
    using Ninject.Activation;

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
        /// Gets or sets the proxy instance.
        /// </summary>
        object Proxy { get; set; }

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