// -------------------------------------------------------------------------------------------------
// <copyright file="IInvocation.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception
{
    using System.Collections.Generic;

    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Describes an executable invocation.
    /// </summary>
    public interface IInvocation
    {
        /// <summary>
        /// Gets the request, which describes the method call.
        /// </summary>
        IProxyRequest Request { get; }

        /// <summary>
        /// Gets the chain of interceptors that will be executed before the target method is called.
        /// </summary>
        IEnumerable<IInterceptor> Interceptors { get; }

        /// <summary>
        /// Gets or sets the return value for the method.
        /// </summary>
        object ReturnValue { get; set; }

        /// <summary>
        /// Continues the invocation, either by invoking the next interceptor in the chain, or
        /// if there are no more interceptors, calling the target method.
        /// </summary>
        void Proceed();

        /// <summary>
        /// Creates a clone of the invocation
        /// </summary>
        /// <returns>The clone</returns>
        IInvocation Clone();
    }
}