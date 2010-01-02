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
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Invocation
{
    /// <summary>
    /// A baseline definition of an invocation.
    /// </summary>
    public abstract class InvocationBase : IInvocation
    {
        private readonly IEnumerator<IInterceptor> _enumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationBase"/> class.
        /// </summary>
        /// <param name="request">The request, which describes the method call.</param>
        /// <param name="interceptors">The chain of interceptors that will be executed before the target method is called.</param>
        protected InvocationBase( IProxyRequest request, IEnumerable<IInterceptor> interceptors )
        {
            Ensure.ArgumentNotNull( request, "request" );

            Request = request;
            Interceptors = interceptors;

            if ( interceptors != null )
            {
                _enumerator = interceptors.GetEnumerator();
            }
        }

        #region IInvocation Members

        /// <summary>
        /// Gets the request, which describes the method call.
        /// </summary>
        public IProxyRequest Request { get; protected set; }

        /// <summary>
        /// Gets the chain of interceptors that will be executed before the target method is called.
        /// </summary>
        /// <value></value>
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
            if ( ( _enumerator != null ) &&
                 _enumerator.MoveNext() )
            {
                _enumerator.Current.Intercept( this );
            }
            else
            {
                ReturnValue = CallTargetMethod();
            }
        }

        #endregion

        /// <summary>
        /// Calls the target method described by the request.
        /// </summary>
        protected abstract object CallTargetMethod();
    }
}