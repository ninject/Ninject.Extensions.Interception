// -------------------------------------------------------------------------------------------------
// <copyright file="InternalInterceptAttribute.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Attributes
{
    using System;

    using Ninject.Extensions.Interception.Infrastructure;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Internal attribute used to mark methods for interception.
    /// </summary>
    internal sealed class InternalInterceptAttribute : InterceptAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalInterceptAttribute"/> class.
        /// </summary>
        /// <param name="createCallback">The callback to create the <see cref="IInterceptor"/>.</param>
        public InternalInterceptAttribute(Func<IProxyRequest, IInterceptor> createCallback)
        {
            Ensure.ArgumentNotNull(createCallback, "createCallback");
            this.CreateCallback = createCallback;
        }

        /// <summary>
        /// Gets or sets the delegate to create the <see cref="IInterceptor"/>.
        /// </summary>
        public Func<IProxyRequest, IInterceptor> CreateCallback { get; set; }

        /// <summary>
        /// Creates the interceptor associated with the attribute.
        /// </summary>
        /// <param name="request">The request that is being intercepted.</param>
        /// <returns>The interceptor.</returns>
        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return this.CreateCallback(request);
        }
    }
}