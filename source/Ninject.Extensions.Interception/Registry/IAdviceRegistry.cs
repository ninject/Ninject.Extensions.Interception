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
using System.Collections.Generic;
using Ninject.Components;
using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Registry
{
    /// <summary>
    /// Collects advice defined for methods.
    /// </summary>
    public interface IAdviceRegistry : INinjectComponent
    {
        /// <summary>
        /// Gets a value indicating whether dynamic advice has been registered.
        /// </summary>
        bool HasDynamicAdvice { get; }

        /// <summary>
        /// Registers the specified advice.
        /// </summary>
        /// <param name="advice">The advice to register.</param>
        void Register( IAdvice advice );

        /// <summary>
        /// Determines whether any static advice has been registered for the specified type.
        /// </summary>
        /// <param name="type">The type in question.</param>
        /// <returns><see langword="True"/> if advice has been registered, otherwise <see langword="false"/>.</returns>
        bool HasStaticAdvice( Type type );

        /// <summary>
        /// Gets the interceptors that should be invoked for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A collection of interceptors, ordered by the priority in which they should be invoked.</returns>
        ICollection<IInterceptor> GetInterceptors( IProxyRequest request );
    }
}