// -------------------------------------------------------------------------------------------------
// <copyright file="IAdvice.cs" company="Ninject Project Contributors">
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

    using Ninject.Activation;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// A declaration of advice, which relates a method or condition with an interceptor.
    /// </summary>
    public interface IAdvice
    {
        /// <summary>
        /// Gets or sets the method handle for the advice, if it is static.
        /// </summary>
        RuntimeMethodHandle MethodHandle { get; set; }

        /// <summary>
        /// Gets or sets the condition for the advice, if it is dynamic.
        /// </summary>
        Predicate<IContext> Condition { get; set; }

        /// <summary>
        /// Gets or sets the interceptor associated with the advice, if one was defined during
        /// registration.
        /// </summary>
        IInterceptor Interceptor { get; set; }

        /// <summary>
        /// Gets or sets the callback that will be triggered to create the interceptor, if applicable.
        /// </summary>
        Func<IProxyRequest, IInterceptor> Callback { get; set; }

        /// <summary>
        /// Gets or sets the order of precedence in which the advice should be called.
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// Gets a value indicating whether the advice is related to a condition instead of a
        /// specific method.
        /// </summary>
        bool IsDynamic { get; }

        /// <summary>
        /// Determines whether the advice matches the specified request.
        /// </summary>
        /// <param name="request">The request in question.</param>
        /// <returns><see langword="True"/> if the request matches, otherwise <see langword="false"/>.</returns>
        bool Matches(IProxyRequest request);

        /// <summary>
        /// Gets the interceptor associated with the advice for the specified request.
        /// </summary>
        /// <param name="request">The request in question.</param>
        /// <returns>The interceptor.</returns>
        IInterceptor GetInterceptor(IProxyRequest request);
    }
}