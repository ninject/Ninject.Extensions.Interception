// -------------------------------------------------------------------------------------------------
// <copyright file="ProxyRequest.cs" company="Ninject Project Contributors">
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
    using Ninject.Extensions.Interception.Infrastructure;

    /// <summary>
    /// The stock implementation of a request.
    /// </summary>
    public class ProxyRequest : IProxyRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyRequest"/> class.
        /// </summary>
        /// <param name="context">The context in which the target instance was activated.</param>
        /// <param name="proxy">The proxy instance.</param>
        /// <param name="target">The target instance.</param>
        /// <param name="method">The method that will be called on the target instance.</param>
        /// <param name="arguments">The arguments to the method.</param>
        /// <param name="genericArguments">The generic type arguments for the method.</param>
        public ProxyRequest(
            IContext context,
            object proxy,
            object target,
            MethodInfo method,
            object[] arguments,
            Type[] genericArguments)
        {
            Ensure.ArgumentNotNull(context, "context");
            Ensure.ArgumentNotNull(proxy, "proxy");
            Ensure.ArgumentNotNull(target, "target");
            Ensure.ArgumentNotNull(method, "method");
            Ensure.ArgumentNotNull(arguments, "arguments");

            this.Kernel = context.Kernel;
            this.Context = context;
            this.Proxy = proxy;
            this.Target = target;
            this.Method = method;
            this.Arguments = arguments;
            this.GenericArguments = genericArguments;
        }

        /// <summary>
        /// Gets the kernel that created the target instance.
        /// </summary>
        public IKernel Kernel { get; private set; }

        /// <summary>
        /// Gets the context in which the target instance was activated.
        /// </summary>
        public IContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the proxy instance.
        /// </summary>
        public object Proxy { get; set; }

        /// <summary>
        /// Gets the target instance.
        /// </summary>
        public object Target { get; private set; }

        /// <summary>
        /// Gets the method that will be called on the target instance.
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// Gets the arguments to the method.
        /// </summary>
        public object[] Arguments { get; private set; }

        /// <summary>
        /// Gets the generic type arguments for the method.
        /// </summary>
        public Type[] GenericArguments { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the request has generic arguments.
        /// </summary>
        public bool HasGenericArguments
        {
            get { return (this.GenericArguments != null) && (this.GenericArguments.Length > 0); }
        }
    }
}