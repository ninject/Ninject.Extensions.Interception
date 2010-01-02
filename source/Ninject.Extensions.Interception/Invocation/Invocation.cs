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

using System.Collections.Generic;
using Ninject.Extensions.Interception.Infrastructure;
using Ninject.Extensions.Interception.Injection;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Invocation
{
    /// <summary>
    /// An implementation of an invocation which uses an <see cref="IMethodInjector"/> to call
    /// the target method.
    /// </summary>
    public class Invocation : InvocationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Invocation"/> class.
        /// </summary>
        /// <param name="request">The request, which describes the method call.</param>
        /// <param name="injector">The injector that will be used to call the target method.</param>
        /// <param name="interceptors">The chain of interceptors that will be executed before the target method is called.</param>
        public Invocation( IProxyRequest request,
                           IMethodInjector injector,
                           IEnumerable<IInterceptor> interceptors )
            : base( request, interceptors )
        {
            Ensure.ArgumentNotNull( injector, "injector" );
            Injector = injector;
        }

        /// <summary>
        /// Gets the injector that will be used to call the target method.
        /// </summary>
        public IMethodInjector Injector { get; protected set; }

        /// <summary>
        /// Calls the target method described by the request.
        /// </summary>
        protected override object CallTargetMethod()
        {
            return Injector.Invoke( Request.Target, Request.Arguments );
        }
    }
}