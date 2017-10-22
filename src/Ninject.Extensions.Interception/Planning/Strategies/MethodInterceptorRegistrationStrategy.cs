// -------------------------------------------------------------------------------------------------
// <copyright file="MethodInterceptorRegistrationStrategy.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Planning.Strategies
{
    using System.Collections.Generic;
    using System.Reflection;

    using Ninject.Extensions.Interception.Advice;
    using Ninject.Extensions.Interception.Attributes;
    using Ninject.Extensions.Interception.Planning.Directives;
    using Ninject.Extensions.Interception.Registry;
    using Ninject.Planning;

    /// <summary>
    /// Provides interceptor attachment for methods configured by the kernel for method level interception.
    /// </summary>
    public class MethodInterceptorRegistrationStrategy : InterceptorRegistrationStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInterceptorRegistrationStrategy"/> class.
        /// </summary>
        /// <param name="adviceFactory">The advice factory.</param>
        /// <param name="adviceRegistry">The advice registry.</param>
        /// <param name="methodInterceptorRegistry">The method interceptor registry.</param>
        public MethodInterceptorRegistrationStrategy(
            IAdviceFactory adviceFactory,
            IAdviceRegistry adviceRegistry,
            IMethodInterceptorRegistry methodInterceptorRegistry)
            : base(adviceFactory, adviceRegistry)
        {
            this.MethodInterceptorRegistry = methodInterceptorRegistry;
        }

        /// <summary>
        /// Gets or sets the method interceptor registry.
        /// </summary>
        /// <value>The method interceptor registry.</value>
        public IMethodInterceptorRegistry MethodInterceptorRegistry { get; set; }

        /// <summary>
        /// Contributes to the specified plan.
        /// </summary>
        /// <param name="plan">The plan that is being generated.</param>
        public override void Execute(IPlan plan)
        {
            if (!this.MethodInterceptorRegistry.Contains(plan.Type))
            {
                return;
            }

            MethodInterceptorCollection methodInterceptors = this.MethodInterceptorRegistry.GetMethodInterceptors(plan.Type);

            Dictionary<MethodInfo, List<IInterceptor>>.KeyCollection methods = methodInterceptors.Keys;
            if (methods.Count == 0)
            {
                return;
            }

            foreach (MethodInfo method in methods)
            {
                for (int order = 0; order < methodInterceptors[method].Count; order++)
                {
                    IInterceptor interceptor = methodInterceptors[method][order];
                    this.RegisterMethodInterceptors(
                        plan.Type,
                        method,
                        new[] { new InternalInterceptAttribute(request => interceptor) { Order = order }, });
                }
            }

            if (!plan.Has<ProxyDirective>())
            {
                plan.Add(new ProxyDirective());
            }
        }
    }
}