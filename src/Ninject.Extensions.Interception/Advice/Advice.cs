// -------------------------------------------------------------------------------------------------
// <copyright file="Advice.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Advice
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Ninject.Activation;
    using Ninject.Extensions.Interception.Infrastructure;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// A declaration of advice, which is called for matching requests.
    /// </summary>
    public class Advice : IAdvice
    {
        private MethodInfo method;

        /// <summary>
        /// Initializes a new instance of the <see cref="Advice"/> class.
        /// </summary>
        /// <param name="method">The method that will be intercepted.</param>
        public Advice(MethodInfo method)
        {
            Ensure.ArgumentNotNull(method, "method");
            this.MethodHandle = method.GetMethodHandle();
            this.method = method;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Advice"/> class.
        /// </summary>
        /// <param name="condition">The condition that will be evaluated for a request.</param>
        public Advice(Predicate<IContext> condition)
        {
            Ensure.ArgumentNotNull(condition, "condition");
            this.Condition = condition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Advice"/> class.
        /// </summary>
        /// <param name="condition">The condition that will be evaluated for a request.</param>
        /// <param name="methodPredicate">The condition that will be evaluated to determine if the method call shall be intercepted.</param>
        public Advice(Predicate<IContext> condition, Predicate<MethodInfo> methodPredicate)
        {
            Ensure.ArgumentNotNull(condition, "condition");
            this.Condition = condition;
            this.MethodPredicate = methodPredicate;
        }

        /// <summary>
        /// Gets or sets the method handle for the advice, if it is static.
        /// </summary>
        public RuntimeMethodHandle MethodHandle { get; set; }

        /// <summary>
        /// Gets or sets the condition for the advice that
        /// will be evaluated to determine if the method call shall be intercepted.
        /// </summary>
        public Predicate<IContext> Condition { get; set; }

        /// <summary>
        /// Gets or sets the condition for the advice, if it is dynamic.
        /// </summary>
        public Predicate<MethodInfo> MethodPredicate { get; set; }

        /// <summary>
        /// Gets or sets the interceptor associated with the advice, if applicable.
        /// </summary>
        public IInterceptor Interceptor { get; set; }

        /// <summary>
        /// Gets or sets the factory method that should be called to create the interceptor, if applicable.
        /// </summary>
        public Func<IProxyRequest, IInterceptor> Callback { get; set; }

        /// <summary>
        /// Gets or sets the order of precedence that the advice should be called in.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets a value indicating whether the advice is related to a condition instead of a
        /// specific method.
        /// </summary>
        public bool IsDynamic
        {
            get { return this.Condition != null; }
        }

        /// <summary>
        /// Determines whether the advice matches the specified request.
        /// </summary>
        /// <param name="request">The request in question.</param>
        /// <returns><see langword="True"/> if the request matches, otherwise <see langword="false"/>.</returns>
        public bool Matches(IProxyRequest request)
        {
            return this.IsDynamic
                ? this.Condition(request.Context) && this.MatchesMethodPredicate(request)
                : this.MatchesMethod(request);
        }

        /// <summary>
        /// Gets the interceptor associated with the advice for the specified request.
        /// </summary>
        /// <param name="request">The request in question.</param>
        /// <returns>The interceptor.</returns>
        public IInterceptor GetInterceptor(IProxyRequest request)
        {
            return this.Interceptor ?? this.Callback(request);
        }

        /// <summary>
        /// Evaluates if the method predicate matches.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if the predicate matches, <c>false</c> otherwise.</returns>
        private bool MatchesMethodPredicate(IProxyRequest request)
        {
            if (this.MethodPredicate == null)
            {
                return true;
            }

            var requestMethod = request.Method;
            if (requestMethod.DeclaringType != request.Target.GetType())
            {
                requestMethod = request.Target.GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .SingleOrDefault(mi => mi.Name == requestMethod.Name &&
                                     mi.GetParameters().SequenceEqual(requestMethod.GetParameters()) &&
                                     mi.GetGenericArguments().SequenceEqual(requestMethod.GetGenericArguments()))
                    ?? requestMethod;
            }

            return this.MethodPredicate(requestMethod);
        }

        /// <summary>
        /// Evaluates if the method the method of this advice.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool MatchesMethod(IProxyRequest request)
        {
            if (!this.method.DeclaringType.IsAssignableFrom(request.Target.GetType()))
            {
                return false;
            }

            if (request.Method.GetMethodHandle().Equals(this.MethodHandle))
            {
                return true;
            }

            var requestType = request.Method.DeclaringType;
            if (requestType == null ||
                !requestType.IsInterface ||
                !requestType.IsAssignableFrom(this.method.DeclaringType))
            {
                return this.method.GetBaseDefinition().GetMethodHandle() == request.Method.GetMethodHandle();
            }

            var map = this.method.DeclaringType.GetInterfaceMap(request.Method.DeclaringType);
            var index = Array.IndexOf(map.InterfaceMethods, request.Method.IsGenericMethod ? request.Method.GetGenericMethodDefinition() : request.Method);

            if (index == -1)
            {
                return false;
            }

            return map.TargetMethods[index].GetMethodHandle() == this.method.GetMethodHandle();
        }
    }
}