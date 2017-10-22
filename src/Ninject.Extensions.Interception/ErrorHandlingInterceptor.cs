// -------------------------------------------------------------------------------------------------
// <copyright file="ErrorHandlingInterceptor.cs" company="Ninject Project Contributors">
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
    /// A simple definition of an interceptor, which can take action both before and after
    /// the invocation proceeds.
    /// </summary>
    public abstract class ErrorHandlingInterceptor : IInterceptor
    {
        private readonly IInterceptor interceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingInterceptor"/> class.
        /// </summary>
        protected ErrorHandlingInterceptor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingInterceptor"/> class.
        /// </summary>
        /// <param name="interceptor">The interceptor to decorate.</param>
        protected ErrorHandlingInterceptor(IInterceptor interceptor)
        {
            this.interceptor = interceptor;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                if (this.interceptor == null)
                {
                    invocation.Proceed();
                }
                else
                {
                    this.interceptor.Intercept(invocation);
                }
            }
            catch (Exception exception)
            {
                if (!this.HandleException(invocation, exception))
                {
                    throw;
                }
            }
            finally
            {
                this.CompleteInvoke(invocation);
            }
        }

        /// <summary>
        /// Handles exception for the invocation proceeding.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        /// <param name="exception">The exception when proceed the invocation.</param>
        /// <returns>A boolean value indicating whether the exception is being handled or not.</returns>
        protected virtual bool HandleException(IInvocation invocation, Exception exception)
        {
            return false;
        }

        /// <summary>
        /// Takes some action after the invocation proceeds, no matter the proceeding is success or failed.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void CompleteInvoke(IInvocation invocation)
        {
        }
    }
}