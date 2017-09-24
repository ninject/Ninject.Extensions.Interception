// -------------------------------------------------------------------------------------------------
// <copyright file="DynamicProxyWrapper.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !NO_CDP2

namespace Ninject.Extensions.Interception.Wrapper
{
    using Ninject.Activation;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Defines an interception wrapper that can convert a Castle DynamicProxy2 <see cref="Castle.DynamicProxy.IInvocation"/>
    /// into a Ninject <see cref="IRequest"/> for interception.
    /// </summary>
    public class DynamicProxyWrapper : StandardWrapper, Castle.DynamicProxy.IInterceptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProxyWrapper"/> class.
        /// </summary>
        /// <param name="kernel">The kernel associated with the wrapper.</param>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="instance">The wrapped instance.</param>
        public DynamicProxyWrapper(IKernel kernel, IContext context, object instance)
            : base(kernel, context, instance)
        {
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="castleInvocation">The invocation.</param>
        public void Intercept(Castle.DynamicProxy.IInvocation castleInvocation)
        {
            IProxyRequest request = this.CreateRequest(castleInvocation);
            IInvocation invocation = this.CreateInvocation(request);

            invocation.Proceed();

            castleInvocation.ReturnValue = invocation.ReturnValue;
        }

        private IProxyRequest CreateRequest(Castle.DynamicProxy.IInvocation castleInvocation)
        {
            var requestFactory = this.Context.Kernel.Components.Get<IProxyRequestFactory>();

            return requestFactory.Create(
                this.Context,
                castleInvocation.Proxy,
                this.Instance,
                castleInvocation.GetConcreteMethod(),
                castleInvocation.Arguments,
                castleInvocation.GenericArguments);
        }
    }
}

#endif //!MONO && !NO_CDP2