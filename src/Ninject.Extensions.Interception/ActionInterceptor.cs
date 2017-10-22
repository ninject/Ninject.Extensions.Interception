// -------------------------------------------------------------------------------------------------
// <copyright file="ActionInterceptor.cs" company="Ninject Project Contributors">
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
    using System;

    /// <summary>
    /// Provides the ability to supply an action which will be invoked during method inteterception.
    /// </summary>
    public class ActionInterceptor : IInterceptor
    {
        private readonly Action<IInvocation> interceptAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInterceptor"/> class.
        /// </summary>
        /// <param name="interceptAction">The intercept action to take.</param>
        public ActionInterceptor(Action<IInvocation> interceptAction)
        {
            this.interceptAction = interceptAction;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            this.interceptAction(invocation);
        }
    }
}