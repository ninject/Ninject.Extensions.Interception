// -------------------------------------------------------------------------------------------------
// <copyright file="IAdviceTargetSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Advice.Syntax
{
    using System;
    using Ninject.Extensions.Interception.Request;
    using Ninject.Syntax;

    /// <summary>
    /// Describes a fluent syntax for modifying the target of an interception.
    /// </summary>
    public interface IAdviceTargetSyntax : IFluentSyntax
    {
        /// <summary>
        /// Indicates that matching requests should be intercepted via an interceptor of the
        /// specified type. The interceptor will be created via the kernel when the method is called.
        /// </summary>
        /// <typeparam name="T">The type of interceptor to call.</typeparam>
        /// <returns>The advice builder.</returns>
        IAdviceOrderSyntax With<T>()
            where T : IInterceptor;

        /// <summary>
        /// Indicates that matching requests should be intercepted via an interceptor of the
        /// specified type. The interceptor will be created via the kernel when the method is called.
        /// </summary>
        /// <param name="interceptorType">The type of interceptor to call.</param>
        /// <returns>The advice builder.</returns>
        IAdviceOrderSyntax With(Type interceptorType);

        /// <summary>
        /// Indicates that matching requests should be intercepted via the specified interceptor.
        /// </summary>
        /// <param name="interceptor">The interceptor to call.</param>
        /// <returns>The advice builder.</returns>
        IAdviceOrderSyntax With(IInterceptor interceptor);

        /// <summary>
        /// Indicates that matching requests should be intercepted via an interceptor created by
        /// calling the specified callback.
        /// </summary>
        /// <param name="factoryMethod">The factory method that will create the interceptor.</param>
        /// <returns>The advice builder.</returns>
        IAdviceOrderSyntax With(Func<IProxyRequest, IInterceptor> factoryMethod);
    }
}