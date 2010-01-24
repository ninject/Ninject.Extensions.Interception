#region License

// 
// Author: Ian Davis <ian@innovatian.com>
// Copyright (c) 2010, Innovatian Software, LLC
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

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

#endregion

namespace Ninject.Extensions.Interception
{
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
            Kernel.Components.Add<IActivationStrategy, ProxyActivationStrategy>();
            Kernel.Components.Add<IProxyRequestFactory, ProxyRequestFactory>();
            Kernel.Components.Add<IAdviceFactory, AdviceFactory>();
            Kernel.Components.Add<IAdviceRegistry, AdviceRegistry>();
            Kernel.Components.Add<IPlanningStrategy, InterceptorRegistrationStrategy>();
            Kernel.Components.Add<IPlanningStrategy, AutoNotifyInterceptorRegistrationStrategy>();
            Kernel.Components.Add<IPlanningStrategy, MethodInterceptorRegistrationStrategy>();
            Kernel.Components.Add<IMethodInterceptorRegistry, MethodInterceptorRegistry>();

#if NO_LCG
            // If the target platform doesn't have DynamicMethod support, we can't use DynamicInjectorFactory.
            Kernel.Components.Add<IInjectorFactory, ReflectionInjectorFactory>();
#else
            if ( Kernel.Settings.UseReflectionBasedInjection )
            {
                Kernel.Components.Add<IInjectorFactory, ReflectionInjectorFactory>();
            }
            else
            {
                Kernel.Components.Add<IInjectorFactory, DynamicInjectorFactory>();
            }
#endif
        }
    }
}