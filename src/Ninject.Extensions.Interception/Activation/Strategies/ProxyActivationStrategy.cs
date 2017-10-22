// -------------------------------------------------------------------------------------------------
// <copyright file="ProxyActivationStrategy.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010 Enkari, Ltd. All rights reserved.
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Activation.Strategies
{
    using Ninject.Activation;
    using Ninject.Activation.Strategies;
    using Ninject.Extensions.Interception.Planning.Directives;
    using Ninject.Extensions.Interception.ProxyFactory;
    using Ninject.Extensions.Interception.Registry;

    /// <summary>
    /// Activates and deactivates proxied instances in the activation pipeline attaching and detaching proxies.
    /// </summary>
    public class ProxyActivationStrategy : ActivationStrategy
    {
        private readonly IAdviceRegistry adviceRegistry;
        private readonly IProxyFactory proxyFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyActivationStrategy"/> class.
        /// </summary>
        /// <param name="adviceRegistry">The advice registry.</param>
        /// <param name="proxyFactory">The proxy factory.</param>
        public ProxyActivationStrategy(IAdviceRegistry adviceRegistry, IProxyFactory proxyFactory)
        {
            this.adviceRegistry = adviceRegistry;
            this.proxyFactory = proxyFactory;
        }

        /// <summary>
        /// Activates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Activate(IContext context, InstanceReference reference)
        {
            if (this.ShouldProxy(context))
            {
                this.proxyFactory.Wrap(context, reference);
            }

            base.Activate(context, reference);
        }

        /// <summary>
        /// Deactivates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Deactivate(IContext context, InstanceReference reference)
        {
            if (this.ShouldProxy(context))
            {
                context.Kernel.Components.Get<IProxyFactory>().Unwrap(context, reference);
            }

            base.Deactivate(context, reference);
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