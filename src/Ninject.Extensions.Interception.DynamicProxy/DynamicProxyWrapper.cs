// -------------------------------------------------------------------------------------------------
// <copyright file="DynamicProxyWrapper.cs" company="Ninject Project Contributors">
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

#if !NO_CDP2

namespace Ninject.Extensions.Interception.Wrapper
{
    using Ninject.Activation;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Defines an interception wrapper that can convert a Castle DynamicProxy2 <see cref="Castle.DynamicProxy.IInvocation"/>
    /// into a Ninject <see cref="IRequest"/> for interception.
    /// </summary>
    public class DynamicProxyWrapper : StandardWrapper, Castle.DynamicProxy.IInterceptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProxyWrapper"/> class.
        /// </summary>
        /// <param name="kernel">The kernel associated with the wrapper.</param>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="instance">The wrapped instance.</param>
        public DynamicProxyWrapper(IKernel kernel, IContext context, object instance)
            : base(kernel, context, instance)
        {
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="castleInvocation">The invocation.</param>
        public void Intercept(Castle.DynamicProxy.IInvocation castleInvocation)
        {
            IProxyRequest request = this.CreateRequest(castleInvocation);
            IInvocation invocation = this.CreateInvocation(request);

            invocation.Proceed();

            castleInvocation.ReturnValue = invocation.ReturnValue;
        }

        private IProxyRequest CreateRequest(Castle.DynamicProxy.IInvocation castleInvocation)
        {
            var requestFactory = this.Context.Kernel.Components.Get<IProxyRequestFactory>();

            return requestFactory.Create(
                this.Context,
                castleInvocation.Proxy,
                this.Instance,
                castleInvocation.GetConcreteMethod(),
                castleInvocation.Arguments,
                castleInvocation.GenericArguments);
        }
    }
}

#endif //!MONO && !NO_CDP2