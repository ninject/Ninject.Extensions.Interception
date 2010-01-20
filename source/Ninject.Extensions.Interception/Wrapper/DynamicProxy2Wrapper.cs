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

using Ninject.Activation;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Wrapper
{
    /// <summary>
    /// Defines an interception wrapper that can convert a Castle DynamicProxy2 <see cref="Castle.Core.Interceptor.IInvocation"/>
    /// into a Ninject <see cref="IRequest"/> for interception.
    /// </summary>
    public class DynamicProxy2Wrapper : StandardWrapper, Castle.Core.Interceptor.IInterceptor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProxy2Wrapper"/> class.
        /// </summary>
        /// <param name="kernel">The kernel associated with the wrapper.</param>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="instance">The wrapped instance.</param>
        public DynamicProxy2Wrapper( IKernel kernel, IContext context, object instance )
            : base( kernel, context, instance )
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="castleInvocation">The invocation.</param>
        /// <returns>The return value of the invocation, once it is completed.</returns>
        public void Intercept( Castle.Core.Interceptor.IInvocation castleInvocation )
        {
            IProxyRequest request = CreateRequest( castleInvocation );
            IInvocation invocation = CreateInvocation( request );

            invocation.Proceed();

            castleInvocation.ReturnValue = invocation.ReturnValue;
        }

        #endregion

        #region Private Methods

        private IProxyRequest CreateRequest( Castle.Core.Interceptor.IInvocation castleInvocation )
        {
            var requestFactory = Context.Kernel.Components.Get<IProxyRequestFactory>();

            return requestFactory.Create(
                Context,
                castleInvocation.Proxy,
                Instance,
                castleInvocation.GetConcreteMethod(),
                castleInvocation.Arguments,
                castleInvocation.GenericArguments );
        }

        #endregion
    }
}