// -------------------------------------------------------------------------------------------------
// <copyright file="DynamicProxyProxyFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !NO_CDP2

namespace Ninject.Extensions.Interception.ProxyFactory
{
    using System;
    using System.Linq;
    using Castle.DynamicProxy;
    using Castle.DynamicProxy.Serialization;
    using Ninject.Activation;
    using Ninject.Extensions.Interception.Parameters;
    using Ninject.Extensions.Interception.Wrapper;
    using Ninject.Infrastructure;
    using Ninject.Parameters;

    /// <summary>
    /// An implementation of a proxy factory that uses a Castle DynamicProxy2 <see cref="ProxyGenerator"/>
    /// and <see cref="DynamicProxyWrapper"/>s to create wrapped instances.
    /// </summary>
    public class DynamicProxyProxyFactory : ProxyFactoryBase, IHaveKernel
    {
        private static readonly ProxyGenerationOptions ProxyOptions = ProxyGenerationOptions.Default;
        private static readonly ProxyGenerationOptions InterfaceProxyOptions = ProxyGenerationOptions.Default;
        private ProxyGenerator generator = new ProxyGenerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProxyProxyFactory"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public DynamicProxyProxyFactory(IKernel kernel)
        {
            this.Kernel = kernel;
            InterfaceProxyOptions.BaseTypeForInterfaceProxy = typeof(ProxyBase);
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
                this.generator = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Wraps the specified instance in a proxy.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="reference">The <see cref="InstanceReference"/> to wrap.</param>
        public override void Wrap(IContext context, InstanceReference reference)
        {
            if (reference.Instance is IInterceptor ||
                reference.Instance is IProxyTargetAccessor)
            {
                return;
            }

            var wrapper = new DynamicProxyWrapper(this.Kernel, context, reference.Instance);

            Type targetType = context.Request.Service;

            Type[] additionalInterfaces = context.Parameters.OfType<AdditionalInterfaceParameter>().Select(ai => (Type)ai.GetValue(context, null)).ToArray();

            if (targetType.IsInterface)
            {
                reference.Instance = this.generator.CreateInterfaceProxyWithoutTarget(targetType, additionalInterfaces, InterfaceProxyOptions, wrapper);
            }
            else
            {
                object[] parameters = context.Parameters.OfType<ConstructorArgument>()
                    .Select(parameter => parameter.GetValue(context, null))
                    .ToArray();
                reference.Instance = this.generator.CreateClassProxy(targetType, additionalInterfaces, ProxyOptions, parameters, wrapper);
            }
        }

        /// <summary>
        /// Unwraps the specified proxied instance.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="reference">The <see cref="InstanceReference"/> to unwrap.</param>
        public override void Unwrap(IContext context, InstanceReference reference)
        {
            var accessor = reference.Instance as IProxyTargetAccessor;

            if (accessor == null)
            {
                return;
            }

            Castle.DynamicProxy.IInterceptor[] interceptors = accessor.GetInterceptors();

            if ((interceptors == null) || (interceptors.Length == 0))
            {
                return;
            }

            var wrapper = interceptors[0] as IWrapper;

            if (wrapper == null)
            {
                return;
            }

            reference.Instance = wrapper.Instance;
        }
    }
}

#endif //!MONO && !NO_CDP2