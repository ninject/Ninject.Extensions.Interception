// -------------------------------------------------------------------------------------------------
// <copyright file="InterceptionModule.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception
{
    using System;
    using System.Text;
    using Ninject.Activation.Strategies;
    using Ninject.Extensions.Interception.Activation.Strategies;
    using Ninject.Extensions.Interception.Advice;
    using Ninject.Extensions.Interception.Injection;
#if !NO_LCG
    using Ninject.Extensions.Interception.Injection.Dynamic;
#endif
    using Ninject.Extensions.Interception.Injection.Reflection;
    using Ninject.Extensions.Interception.Planning.Strategies;
    using Ninject.Extensions.Interception.ProxyFactory;
    using Ninject.Extensions.Interception.Registry;
    using Ninject.Extensions.Interception.Request;
    using Ninject.Modules;
    using Ninject.Planning.Strategies;

    /// <summary>
    /// Extends the functionality of the kernel, providing the base functionality needed for interception.
    /// </summary>
    /// <remarks>
    /// Note: Inheritors must provide a component binding for <see cref="IProxyFactory"/>.
    /// </remarks>
    public abstract class InterceptionModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IActivationStrategy, ProxyActivationStrategy>();
            this.Kernel.Components.Add<IProxyRequestFactory, ProxyRequestFactory>();
            this.Kernel.Components.Add<IAdviceFactory, AdviceFactory>();
            this.Kernel.Components.Add<IAdviceRegistry, AdviceRegistry>();
            this.Kernel.Components.Add<IPlanningStrategy, InterceptorRegistrationStrategy>();
            this.Kernel.Components.Add<IPlanningStrategy, AutoNotifyInterceptorRegistrationStrategy>();
            this.Kernel.Components.Add<IPlanningStrategy, MethodInterceptorRegistrationStrategy>();
            this.Kernel.Components.Add<IMethodInterceptorRegistry, MethodInterceptorRegistry>();

#if NO_LCG
            // If the target platform doesn't have DynamicMethod support, we can't use DynamicInjectorFactory.
            Kernel.Components.Add<IInjectorFactory, ReflectionInjectorFactory>();
#else
            if (this.Kernel.Settings.UseReflectionBasedInjection)
            {
                this.Kernel.Components.Add<IInjectorFactory, ReflectionInjectorFactory>();
            }
            else
            {
                this.Kernel.Components.Add<IInjectorFactory, DynamicInjectorFactory>();
            }
#endif
        }

        /// <summary>
        /// Verifies that there are no proxy factories attached as kernel components.
        /// </summary>
        protected virtual void VerifyNoBoundProxyFactoriesExist()
        {
            IProxyFactory existingProxyFactory = this.Kernel.Components.Get<IProxyFactory>();

            if (existingProxyFactory != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(
                    "The Ninject Kernel already has an implementation of IProxyFactory bound to {0}. ",
                    existingProxyFactory.GetType().Name);
                builder.AppendLine();
                builder.AppendLine(
                    " Please verify that if you are using automatic extension loading that you only have one interception module.");
                builder.AppendLine(
                    " If you have more than one interception module, please disable automatic extension loading by passing an INinjectSettings object into your Kernel's .ctor with");
                throw new InvalidOperationException();
            }
        }
    }
}