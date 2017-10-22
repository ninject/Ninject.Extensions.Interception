// -------------------------------------------------------------------------------------------------
// <copyright file="IAdviceRegistry.cs" company="Ninject Project Contributors">
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
    using System.Collections.Generic;

    using Ninject.Activation;
    using Ninject.Components;
    using Ninject.Extensions.Interception.Advice;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Collects advice defined for methods.
    /// </summary>
    public interface IAdviceRegistry : INinjectComponent
    {
        /// <summary>
        /// Gets a value indicating whether dynamic advice has been registered.
        /// </summary>
        bool HasDynamicAdvice { get; }

        /// <summary>
        /// Determines whether an advice for the specified context exists.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// <c>true</c> if an advice for the specified context exists.; otherwise, <c>false</c>.
        /// </returns>
        bool HasAdvice(IContext context);

        /// <summary>
        /// Registers the specified advice.
        /// </summary>
        /// <param name="advice">The advice to register.</param>
        void Register(IAdvice advice);

        /// <summary>
        /// Determines whether any static advice has been registered for the specified type.
        /// </summary>
        /// <param name="type">The type in question.</param>
        /// <returns><see langword="True"/> if advice has been registered, otherwise <see langword="false"/>.</returns>
        bool HasStaticAdvice(Type type);

        /// <summary>
        /// Gets the interceptors that should be invoked for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A collection of interceptors, ordered by the priority in which they should be invoked.</returns>
        ICollection<IInterceptor> GetInterceptors(IProxyRequest request);
    }
}