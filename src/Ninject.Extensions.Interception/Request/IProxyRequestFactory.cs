// -------------------------------------------------------------------------------------------------
// <copyright file="IProxyRequestFactory.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Request
{
    using System;
    using System.Reflection;

    using Ninject.Activation;
    using Ninject.Components;

    /// <summary>
    /// Creates <see cref="IProxyRequest"/>s, which describe method calls.
    /// </summary>
    public interface IProxyRequestFactory : INinjectComponent
    {
        /// <summary>
        /// Creates a request representing the specified method call.
        /// </summary>
        /// <param name="context">The context in which the target instance was activated.</param>
        /// <param name="proxy">The proxy instance.</param>
        /// <param name="target">The target instance.</param>
        /// <param name="method">The method that will be called on the target instance.</param>
        /// <param name="arguments">The arguments to the method.</param>
        /// <param name="genericArguments">The generic type arguments for the method.</param>
        /// <returns>The newly-created request.</returns>
        IProxyRequest Create(
            IContext context,
            object proxy,
            object target,
            MethodInfo method,
            object[] arguments,
            Type[] genericArguments);
    }
}