// -------------------------------------------------------------------------------------------------
// <copyright file="AdviceBuilder.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Advice.Builders
{
    using System;

    using Ninject.Extensions.Interception.Advice.Syntax;
    using Ninject.Extensions.Interception.Infrastructure;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// The stock definition of an advice builder.
    /// </summary>
    public class AdviceBuilder : DisposableObject, IAdviceBuilder, IAdviceTargetSyntax, IAdviceOrderSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdviceBuilder"/> class.
        /// </summary>
        /// <param name="advice">The advice that should be manipulated.</param>
        public AdviceBuilder(IAdvice advice)
        {
            Ensure.ArgumentNotNull(advice, "advice");
            this.Advice = advice;
        }

        /// <summary>
        /// Gets or sets the advice the builder should manipulate.
        /// </summary>
        public IAdvice Advice { get; protected set; }

        /// <summary>
        /// Indicates that the interceptor should be called with the specified order. (Interceptors
        /// with a lower order will be called first.)
        /// </summary>
        /// <param name="order">The order.</param>
        void IAdviceOrderSyntax.InOrder(int order)
        {
            this.Advice.Order = order;
        }

        /// <summary>
        /// Indicates that matching requests should be intercepted via an interceptor of the
        /// specified type. The interceptor will be created via the kernel when the method is called.
        /// </summary>
        /// <typeparam name="T">The type of interceptor to call.</typeparam>
        /// <returns>The advice builder.</returns>
        IAdviceOrderSyntax IAdviceTargetSyntax.With<T>()
        {
            this.Advice.Callback = r => r.Kernel.Get<T>();
            return this;
        }

        /// <summary>
        /// Indicates that matching requests should be intercepted via an interceptor of the
        /// specified type. The interceptor will be created via the kernel when the method is called.
        /// </summary>
        /// <param name="interceptorType">The type of interceptor to call.</param>
        /// <returns>The advice builder.</returns>
        IAdviceOrderSyntax IAdviceTargetSyntax.With(Type interceptorType)
        {
            this.Advice.Callback = r => r.Kernel.Get(interceptorType) as IInterceptor;
            return this;
        }

        /// <summary>
        /// Indicates that matching requests should be intercepted via the specified interceptor.
        /// </summary>
        /// <param name="interceptor">The interceptor to call.</param>
        /// <returns>The advice builder.</returns>
        IAdviceOrderSyntax IAdviceTargetSyntax.With(IInterceptor interceptor)
        {
            this.Advice.Interceptor = interceptor;
            return this;
        }

        /// <summary>
        /// Indicates that matching requests should be intercepted via an interceptor created by
        /// calling the specified callback.
        /// </summary>
        /// <param name="factoryMethod">The factory method that will create the interceptor.</param>
        /// <returns>The advice builder.</returns>
        IAdviceOrderSyntax IAdviceTargetSyntax.With(Func<IProxyRequest, IInterceptor> factoryMethod)
        {
            this.Advice.Callback = factoryMethod;
            return this;
        }
    }
}