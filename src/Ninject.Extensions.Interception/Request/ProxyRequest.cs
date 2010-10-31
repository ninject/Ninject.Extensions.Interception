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
using System.Reflection;
using Ninject.Activation;
using Ninject.Extensions.Interception.Infrastructure;

#endregion

namespace Ninject.Extensions.Interception.Request
{
    /// <summary>
    /// The stock implementation of a request.
    /// </summary>
    public class ProxyRequest : IProxyRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyRequest"/> class.
        /// </summary>
        /// <param name="context">The context in which the target instance was activated.</param>
        /// <param name="proxy"></param>
        /// <param name="target">The target instance.</param>
        /// <param name="method">The method that will be called on the target instance.</param>
        /// <param name="arguments">The arguments to the method.</param>
        /// <param name="genericArguments">The generic type arguments for the method.</param>
        public ProxyRequest( IContext context,
                             object proxy,
                             object target,
                             MethodInfo method,
                             object[] arguments,
                             Type[] genericArguments )
        {
            Ensure.ArgumentNotNull( context, "context" );
            Ensure.ArgumentNotNull( proxy, "proxy" );
            Ensure.ArgumentNotNull( target, "target" );
            Ensure.ArgumentNotNull( method, "method" );
            Ensure.ArgumentNotNull( arguments, "arguments" );

            Kernel = context.Kernel;
            Context = context;
            Proxy = proxy;
            Target = target;
            Method = method;
            Arguments = arguments;
            GenericArguments = genericArguments;
        }

        #region IProxyRequest Members

        /// <summary>
        /// Gets the kernel that created the target instance.
        /// </summary>
        public IKernel Kernel { get; private set; }

        /// <summary>
        /// Gets the context in which the target instance was activated.
        /// </summary>
        public IContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the proxy instance.
        /// </summary>
        public object Proxy { get; set; }

        /// <summary>
        /// Gets the target instance.
        /// </summary>
        public object Target { get; private set; }

        /// <summary>
        /// Gets the method that will be called on the target instance.
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// Gets the arguments to the method.
        /// </summary>
        public object[] Arguments { get; private set; }

        /// <summary>
        /// Gets the generic type arguments for the method.
        /// </summary>
        public Type[] GenericArguments { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the request has generic arguments.
        /// </summary>
        public bool HasGenericArguments
        {
            get { return ( GenericArguments != null ) && ( GenericArguments.Length > 0 ); }
        }

        #endregion
    }
}