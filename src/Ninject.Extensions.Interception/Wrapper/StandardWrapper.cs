// -------------------------------------------------------------------------------------------------
// <copyright file="StandardWrapper.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Wrapper
{
    using System.Collections.Generic;
    using Ninject.Activation;
    using Ninject.Components;
    using Ninject.Extensions.Interception.Infrastructure;
    using Ninject.Extensions.Interception.Injection;
    using Ninject.Extensions.Interception.Registry;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Defines an interception wrapper, which contains a contextualized instance and can be
    /// used to create executable invocations.
    /// </summary>
    public class StandardWrapper : IWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardWrapper"/> class.
        /// </summary>
        /// <param name="kernel">The kernel associated with the wrapper.</param>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="instance">The wrapped instance.</param>
        protected StandardWrapper(IKernel kernel, IContext context, object instance)
        {
            Ensure.ArgumentNotNull(kernel, "kernel");
            Ensure.ArgumentNotNull(context, "context");
            Ensure.ArgumentNotNull(instance, "instance");

            this.Kernel = kernel;
            this.Context = context;
            this.Instance = instance;
        }

        /// <summary>
        /// Gets or sets the kernel associated with the wrapper.
        /// </summary>
        public IKernel Kernel { get; set; }

        /// <summary>
        /// Gets or sets the context in which the wrapper was created.
        /// </summary>
        public IContext Context { get; set; }

        /// <summary>
        /// Gets or sets the wrapped instance.
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Creates an executable invocation for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An executable invocation representing the specified request.</returns>
        public virtual IInvocation CreateInvocation(IProxyRequest request)
        {
            IComponentContainer components = request.Context.Kernel.Components;

            IEnumerable<IInterceptor> interceptors =
                components.Get<IAdviceRegistry>().GetInterceptors(request);
            IMethodInjector injector =
                components.Get<IInjectorFactory>().GetInjector(request.Method);

            return new Invocation.Invocation(request, injector, interceptors);
        }
    }
}