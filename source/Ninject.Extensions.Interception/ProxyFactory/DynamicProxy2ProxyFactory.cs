#if !MONO

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

using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using Ninject.Activation;
using Ninject.Extensions.Interception.Wrapper;
using Ninject.Infrastructure;

#endregion

namespace Ninject.Extensions.Interception.ProxyFactory
{
    /// <summary>
    /// An implementation of a proxy factory that uses a Castle DynamicProxy2 <see cref="ProxyGenerator"/>
    /// and <see cref="DynamicProxy2Wrapper"/>s to create wrapped instances.
    /// </summary>
    public class DynamicProxy2ProxyFactory : ProxyFactoryBase, IHaveKernel
    {
        #region Fields

        private ProxyGenerator _generator = new ProxyGenerator();

        #endregion

        public DynamicProxy2ProxyFactory( IKernel kernel )
        {
            Kernel = kernel;
        }

        #region IHaveKernel Members

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        public IKernel Kernel { get; private set; }

        #endregion

        #region Disposal

        /// <summary>
        /// Releases all resources held by the object.
        /// </summary>
        /// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
        public override void Dispose( bool disposing )
        {
            if ( disposing && !IsDisposed )
            {
                _generator = null;
            }

            base.Dispose( disposing );
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Wraps the specified instance in a proxy.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <returns>A proxy that wraps the instance.</returns>
        public override void Wrap( IContext context, InstanceReference reference )
        {
            var wrapper = new DynamicProxy2Wrapper( Kernel, context, reference.Instance );
            reference.Instance = _generator.CreateClassProxy( reference.Instance.GetType(), wrapper );
        }


        /// <summary>
        /// Unwraps the specified proxied instance.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <returns>The unwrapped instance.</returns>
        public override void Unwrap( IContext context, InstanceReference reference )
        {
            var accessor = reference.Instance as IProxyTargetAccessor;

            if ( accessor == null )
            {
                return;
            }

            Castle.Core.Interceptor.IInterceptor[] interceptors = accessor.GetInterceptors();

            if ( ( interceptors == null ) || ( interceptors.Length == 0 ) )
            {
                return;
            }

            var wrapper = interceptors[0] as IWrapper;

            if ( wrapper == null )
            {
                return;
            }

            reference.Instance = wrapper.Instance;
        }

        #endregion
    }
}

#endif //!MONO