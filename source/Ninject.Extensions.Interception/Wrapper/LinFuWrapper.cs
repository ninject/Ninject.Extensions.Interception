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

using LinFu.DynamicProxy;
using Ninject.Activation;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Wrapper
{
    /// <summary>
    /// Defines an interception wrapper that can convert a LinFu <see cref="InvocationInfo"/>
    /// into a Ninject <see cref="IProxyRequest"/> for interception.
    /// </summary>
    public class LinFuWrapper : StandardWrapper, LinFu.DynamicProxy.IInterceptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinFuWrapper"/> class.
        /// </summary>
        /// <param name="kernel">The kernel associated with the wrapper.</param>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="instance">The wrapped instance.</param>
        public LinFuWrapper( IKernel kernel, IContext context, object instance )
            : base( kernel, context, instance )
        {
        }

        #region IInterceptor Members

        object LinFu.DynamicProxy.IInterceptor.Intercept( InvocationInfo info )
        {
            IProxyRequest request = CreateRequest( info );
            IInvocation invocation = CreateInvocation( request );

            invocation.Proceed();

            return invocation.ReturnValue;
        }

        #endregion

        private IProxyRequest CreateRequest( InvocationInfo info )
        {
            var requestFactory = Context.Kernel.Components.Get<IProxyRequestFactory>();

            return requestFactory.Create(
                Context,
                Instance,
                info.TargetMethod,
                info.Arguments,
                info.TypeArguments );
        }
    }
}