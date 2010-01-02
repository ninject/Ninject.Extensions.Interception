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
using Ninject.Extensions.Interception.Wrapper;
using Ninject.Infrastructure;

#endregion

namespace Ninject.Extensions.Interception.ProxyFactory
{
    /// <summary>
    /// An implementation of a proxy factory that uses a LinFu <see cref="ProxyFactory"/> and
    /// <see cref="ProxyFactory"/>s to create wrapped instances.
    /// </summary>
    public class LinFuProxyFactory : ProxyFactoryBase, IHaveKernel
    {
        private LinFu.DynamicProxy.ProxyFactory _factory = new LinFu.DynamicProxy.ProxyFactory();

        public LinFuProxyFactory( IKernel kernel )
        {
            Kernel = kernel;
        }

        #region IHaveKernel Members

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        public IKernel Kernel { get; private set; }

        #endregion

        /// <summary>
        /// Releases all resources held by the object.
        /// </summary>
        /// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
        public override void Dispose( bool disposing )
        {
            if ( disposing && !IsDisposed )
            {
                _factory = null;
            }

            base.Dispose( disposing );
        }

        /// <summary>
        /// Wraps the instance in the specified context in a proxy.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <returns>A proxy that wraps the instance.</returns>
        public override void Wrap( IContext context, InstanceReference reference )
        {
            var wrapper = new LinFuWrapper( Kernel, context, reference.Instance );
            reference.Instance = _factory.CreateProxy( reference.Instance.GetType(), wrapper );
        }

        /// <summary>
        /// Unwraps the instance in the specified context.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <returns>The unwrapped instance.</returns>
        public override void Unwrap( IContext context, InstanceReference reference )
        {
            var proxy = reference.Instance as IProxy;

            if ( proxy == null )
            {
                return;
            }

            var wrapper = proxy.Interceptor as LinFuWrapper;
            reference.Instance = ( wrapper == null ) ? proxy : wrapper.Instance;
        }
    }
}