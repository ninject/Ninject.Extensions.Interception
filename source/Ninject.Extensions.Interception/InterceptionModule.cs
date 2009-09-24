#region License

// 
// Author: Ian Davis <ian.f.davis@gmail.com>
// Copyright (c) 2009, Ian Davis
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
using Ninject.Extensions.Interception.Injection.Dynamic;
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
    /// Extends the functionality of the kernel, providing a proxy factory that uses LinFu
    /// to generate dynamic proxies.
    /// </summary>
    public class InterceptionModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Kernel.Components.Add<IProxyFactory, LinFuProxyFactory>();
            Kernel.Components.Add<IActivationStrategy, ProxyActivationStrategy>();
            Kernel.Components.Add<IProxyRequestFactory, ProxyRequestFactory>();
            Kernel.Components.Add<IAdviceFactory, AdviceFactory>();
            Kernel.Components.Add<IAdviceRegistry, AdviceRegistry>();
            Kernel.Components.Add<IPlanningStrategy, InterceptorRegistrationStrategy>();

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