// -------------------------------------------------------------------------------------------------
// <copyright file="Invocation.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Invocation
{
    using System.Collections.Generic;

    using Ninject.Extensions.Interception.Infrastructure;
    using Ninject.Extensions.Interception.Injection;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// An implementation of an invocation which uses an <see cref="IMethodInjector"/> to call
    /// the target method.
    /// </summary>
    public class Invocation : InvocationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Invocation"/> class.
        /// </summary>
        /// <param name="request">The request, which describes the method call.</param>
        /// <param name="injector">The injector that will be used to call the target method.</param>
        /// <param name="interceptors">The chain of interceptors that will be executed before the target method is called.</param>
        public Invocation(
            IProxyRequest request,
            IMethodInjector injector,
            IEnumerable<IInterceptor> interceptors)
            : base(request, interceptors)
        {
            Ensure.ArgumentNotNull(injector, "injector");
            this.Injector = injector;
        }

        /// <summary>
        /// Gets or sets the injector that will be used to call the target method.
        /// </summary>
        public IMethodInjector Injector { get; protected set; }

        /// <summary>
        /// Calls the target method described by the request.
        /// </summary>
        /// <returns>The return value of the method.</returns>
        protected override object CallTargetMethod()
        {
            return this.Injector.Invoke(this.Request.Target, this.Request.Arguments);
        }
    }
}