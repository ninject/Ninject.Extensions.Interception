// -------------------------------------------------------------------------------------------------
// <copyright file="IWrapper.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Wrapper
{
    using Ninject.Activation;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Contains a contextualized instance and can be used to create executable invocations.
    /// </summary>
    public interface IWrapper
    {
        /// <summary>
        /// Gets or sets the kernel associated with the wrapper.
        /// </summary>
        IKernel Kernel { get; set; }

        /// <summary>
        /// Gets or sets the context in which the wrapper was created.
        /// </summary>
        IContext Context { get; set; }

        /// <summary>
        /// Gets or sets the wrapped instance.
        /// </summary>
        object Instance { get; set; }

        /// <summary>
        /// Creates an executable invocation for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An executable invocation representing the specified request.</returns>
        IInvocation CreateInvocation(IProxyRequest request);
    }
}