// -------------------------------------------------------------------------------------------------
// <copyright file="IWrapper.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Wrapper
{
    using Ninject.Activation;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Contains a contextualized instance and can be used to create executable invocations.
    /// </summary>
    public interface IWrapper
    {
        /// <summary>
        /// Gets or sets the kernel associated with the wrapper.
        /// </summary>
        IKernel Kernel { get; set; }

        /// <summary>
        /// Gets or sets the context in which the wrapper was created.
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
        IInvocation CreateInvocation(IProxyRequest request);
    }
}