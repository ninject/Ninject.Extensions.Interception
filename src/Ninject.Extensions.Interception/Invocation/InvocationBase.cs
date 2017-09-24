// -------------------------------------------------------------------------------------------------
// <copyright file="InvocationBase.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Invocation
{
    using System.Collections.Generic;
    using Ninject.Extensions.Interception.Infrastructure;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// A baseline definition of an invocation.
    /// </summary>
    public abstract class InvocationBase : IInvocation
    {
        private readonly IEnumerator<IInterceptor> enumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationBase"/> class.
        /// </summary>
        /// <param name="request">The request, which describes the method call.</param>
        /// <param name="interceptors">The chain of interceptors that will be executed before the target method is called.</param>
        protected InvocationBase(IProxyRequest request, IEnumerable<IInterceptor> interceptors)
        {
            Ensure.ArgumentNotNull(request, "request");

            this.Request = request;
            this.Interceptors = interceptors;

            if (interceptors != null)
            {
                this.enumerator = interceptors.GetEnumerator();
            }
        }

        /// <summary>
        /// Gets or sets the request, which describes the method call.
        /// </summary>
        public IProxyRequest Request { get; protected set; }

        /// <summary>
        /// Gets or sets the chain of interceptors that will be executed before the target method is called.
        /// </summary>
        public IEnumerable<IInterceptor> Interceptors { get; protected set; }

        /// <summary>
        /// Gets or sets the return value for the method.
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// Continues the invocation, either by invoking the next interceptor in the chain, or
        /// if there are no more interceptors, calling the target method.
        /// </summary>
        public void Proceed()
        {
            if ((this.enumerator != null) &&
                 this.enumerator.MoveNext())
            {
                this.enumerator.Current.Intercept(this);
            }
            else
            {
                this.ReturnValue = this.CallTargetMethod();
            }
        }

        /// <summary>
        /// Creates a clone of the invocation
        /// </summary>
        /// <returns>The clone</returns>
        public IInvocation Clone()
        {
            return (IInvocation)this.MemberwiseClone();
        }

        /// <summary>
        /// Calls the target method described by the request.
        /// </summary>
        /// <returns>The return value of the method.</returns>
        protected abstract object CallTargetMethod();
    }
}