// -------------------------------------------------------------------------------------------------
// <copyright file="LinFuProxyFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !SILVERLIGHT && !NO_LINFU

namespace Ninject.Extensions.Interception.ProxyFactory
{
    using System;
    using System.Linq;
    using System.Reflection;
    using LinFu.DynamicProxy;
    using Ninject.Activation;
    using Ninject.Extensions.Interception.Parameters;
    using Ninject.Extensions.Interception.Wrapper;
    using Ninject.Infrastructure;

    /// <summary>
    /// An implementation of a proxy factory that uses a LinFu <see cref="LinFu.DynamicProxy.ProxyFactory"/>
    /// to create wrapped instances.
    /// </summary>
    public class LinFuProxyFactory : ProxyFactoryBase, IHaveKernel
    {
        private LinFu.DynamicProxy.ProxyFactory factory = new LinFu.DynamicProxy.ProxyFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="LinFuProxyFactory"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public LinFuProxyFactory(IKernel kernel)
        {
            this.Kernel = kernel;
        }

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        public IKernel Kernel { get; private set; }

        /// <summary>
        /// Releases all resources held by the object.
        /// </summary>
        /// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
        public override void Dispose(bool disposing)
        {
            if (disposing && !this.IsDisposed)
            {
                this.factory = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Wraps the instance in the specified context in a proxy.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="reference">The <see cref="InstanceReference"/> to wrap.</param>
        public override void Wrap(IContext context, InstanceReference reference)
        {
            var wrapper = new LinFuWrapper(this.Kernel, context, reference.Instance);

            Type targetType = context.Request.Service;

            Type[] additionalInterfaces = context.Parameters.OfType<AdditionalInterfaceParameter>().Select(ai => (Type)ai.GetValue(context, null)).ToArray();

            reference.Instance = targetType.IsInterface
                ? this.factory.CreateProxy(typeof(object), wrapper, new[] { targetType }.Concat(additionalInterfaces).ToArray())
                : this.factory.CreateProxy(targetType, wrapper, additionalInterfaces);
        }

        /// <summary>
        /// Unwraps the instance in the specified context.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="reference">The <see cref="InstanceReference"/> to unwrap.</param>
        public override void Unwrap(IContext context, InstanceReference reference)
        {
            var proxy = reference.Instance as IProxy;

            if (proxy == null)
            {
                return;
            }

            var wrapper = proxy.Interceptor as LinFuWrapper;
            reference.Instance = (wrapper == null) ? proxy : wrapper.Instance;
        }
    }
}

#endif //!SILVERLIGHT && !NO_LINFU