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

using Ninject.Activation;
using Ninject.Activation.Strategies;
using Ninject.Extensions.Interception.Planning.Directives;
using Ninject.Extensions.Interception.ProxyFactory;
using Ninject.Extensions.Interception.Registry;

#endregion

namespace Ninject.Extensions.Interception.Activation.Strategies
{
    /// <summary>
    /// Activates and deactivates proxied instances in the activation pipeline attaching and detaching proxies.
    /// </summary>
    public class ProxyActivationStrategy : ActivationStrategy
    {
        private readonly IAdviceRegistry adviceRegistry;

        public ProxyActivationStrategy(IAdviceRegistry adviceRegistry)
        {
            this.adviceRegistry = adviceRegistry;
        }

        /// <summary>
        /// Activates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Activate( IContext context, InstanceReference reference )
        {
            if ( ShouldProxy( context ) )
            {
                context.Kernel.Components.Get<IProxyFactory>().Wrap( context, reference );
            }
            base.Activate( context, reference );
        }

        /// <summary>
        /// Deactivates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Deactivate( IContext context, InstanceReference reference )
        {
            if ( ShouldProxy( context ) )
            {
                context.Kernel.Components.Get<IProxyFactory>().Unwrap( context, reference );
            }

            base.Deactivate( context, reference );
        }

        /// <summary>
        /// Returns a value indicating whether the instance in the specified context should be proxied.
        /// </summary>
        /// <param name="context">The activation context.</param>
        /// <returns><see langword="True"/> if the instance should be proxied, otherwise <see langword="false"/>.</returns>
        protected virtual bool ShouldProxy(IContext context)
        {
            if (this.adviceRegistry.HasAdvice(context))
            {
                return true;
            }

            // Otherwise, check the type's activation plan.
            return context.Plan.Has<ProxyDirective>();
        }
    }
}