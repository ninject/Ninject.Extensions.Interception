#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using Ninject.Activation;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Wrapper
{
    /// <summary>
    /// Contains a contextualized instance and can be used to create executable invocations.
    /// </summary>
    public interface IWrapper
    {
        /// <summary>
        /// Gets the kernel associated with the wrapper.
        /// </summary>
        IKernel Kernel { get; set; }

        /// <summary>
        /// Gets the context in which the wrapper was created.
        /// </summary>
        IContext Context { get; set; }

        /// <summary>
        /// Gets or sets the wrapped instance.
        /// </summary>
        object Instance { get; set; }

        /// <summary>
        /// Creates an executable invocation for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An executable invocation representing the specified request.</returns>
        IInvocation CreateInvocation( IProxyRequest request );
    }
}