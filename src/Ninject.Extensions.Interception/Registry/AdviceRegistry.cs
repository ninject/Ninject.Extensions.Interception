// -------------------------------------------------------------------------------------------------
// <copyright file="AdviceRegistry.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Registry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Ninject.Activation;
    using Ninject.Components;
    using Ninject.Extensions.Interception.Advice;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Request;
    using Ninject.Infrastructure;

    /// <summary>
    /// Collects advice defined for methods.
    /// </summary>
    public class AdviceRegistry : NinjectComponent, IAdviceRegistry
    {
        private readonly List<IAdvice> advice = new List<IAdvice>();

        private readonly Dictionary<RuntimeMethodHandle, IDictionary<RuntimeTypeHandle, List<IInterceptor>>> cache =
            new Dictionary<RuntimeMethodHandle, IDictionary<RuntimeTypeHandle, List<IInterceptor>>>();

        /// <summary>
        /// Gets a value indicating whether one or more dynamic interceptors have been registered.
        /// </summary>
        public bool HasDynamicAdvice { get; private set; }

        /// <summary>
        /// Determines whether an advice for the specified context exists.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// <c>true</c> if an advice for the specified context exists.; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAdvice(IContext context)
        {
            lock (this.advice)
            {
                return this.advice.Any(a => a.IsDynamic && a.Condition(context));
            }
        }

        /// <summary>
        /// Registers the specified advice.
        /// </summary>
        /// <param name="advice">The advice to register.</param>
        public void Register(IAdvice advice)
        {
            if (advice.IsDynamic)
            {
                this.HasDynamicAdvice = true;
                lock (this.cache)
                {
                    this.cache.Clear();
                }
            }

            lock (this.advice)
            {
                this.advice.Add(advice);
            }
        }

        /// <summary>
        /// Determines whether any static advice has been registered for the specified type.
        /// </summary>
        /// <param name="type">The type in question.</param>
        /// <returns><see langword="True"/> if advice has been registered, otherwise <see langword="false"/>.</returns>
        public bool HasStaticAdvice(Type type)
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Gets the interceptors that should be invoked for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A collection of interceptors, ordered by the priority in which they should be invoked.</returns>
        public ICollection<IInterceptor> GetInterceptors(IProxyRequest request)
        {
            RuntimeMethodHandle methodHandle = request.Method.GetMethodHandle();
            RuntimeTypeHandle typeHandle = request.Target.GetType().TypeHandle;

            ICollection<IInterceptor> interceptors = null;
            IDictionary<RuntimeTypeHandle, List<IInterceptor>> methodCache = null;

            lock (this.cache)
            {
                if (!this.cache.TryGetValue(methodHandle, out methodCache))
                {
                    methodCache = new Dictionary<RuntimeTypeHandle, List<IInterceptor>>();
                    this.cache.Add(methodHandle, methodCache);
                }
            }

            lock (methodCache)
            {
                if (methodCache.ContainsKey(typeHandle))
                {
                    return methodCache[typeHandle];
                }

                if (!this.HasDynamicAdvice)
                {
                    interceptors = this.GetInterceptorsForRequest(request);

                    // If there are no dynamic interceptors defined, we can safely cache the results.
                    // Otherwise, we have to evaluate and re-activate the interceptors each time.
                    methodCache.Add(typeHandle, interceptors.ToList());
                }

                return interceptors ?? this.GetInterceptorsForRequest(request);
            }
        }

        private ICollection<IInterceptor> GetInterceptorsForRequest(IProxyRequest request)
        {
            List<IAdvice> matches;
            lock (this.advice)
            {
                matches = this.advice.Where(advice => advice.Matches(request)).ToList();
            }

            matches.Sort((lhs, rhs) => lhs.Order - rhs.Order);

            List<IInterceptor> interceptors = matches.Convert(a => a.GetInterceptor(request)).ToList();
            return interceptors;
        }
    }
}