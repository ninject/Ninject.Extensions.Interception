// -------------------------------------------------------------------------------------------------
// <copyright file="AsyncInterceptor.cs" company="Ninject Project Contributors">
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
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// A simple definition of an interceptor, which can take action both before and after
    /// the invocation proceeds and supports async methods.
    /// </summary>
    public abstract class AsyncInterceptor : IInterceptor
    {
        private static readonly MethodInfo StartTaskMethodInfo = typeof(AsyncInterceptor).GetMethod(nameof(InterceptTaskWithResult), BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            var returnType = invocation.Request.Method.ReturnType;
            if (returnType == typeof(Task))
            {
                this.InterceptTask(invocation);
                return;
            }

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultType = returnType.GetGenericArguments()[0];
                var mi = StartTaskMethodInfo.MakeGenericMethod(resultType);
                mi.Invoke(this, new object[] { invocation });
                return;
            }

            this.BeforeInvoke(invocation);
            invocation.Proceed();
            this.AfterInvoke(invocation);
        }

        /// <summary>
        /// Takes some action before the invocation proceeds.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void BeforeInvoke(IInvocation invocation)
        {
        }

        /// <summary>
        /// Takes some action after the invocation proceeds.
        /// </summary>
        /// <remarks>Use one AfterInvoke method overload.</remarks>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void AfterInvoke(IInvocation invocation)
        {
        }

        /// <summary>
        /// Takes some action after the invocation proceeds.
        /// </summary>
        /// <remarks>Use one AfterInvoke method overload.</remarks>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        /// <param name="task">The task that was executed.</param>
        protected virtual void AfterInvoke(IInvocation invocation, Task task)
        {
        }

        /// <summary>
        /// Handles exception for the invocation proceeding.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        /// <param name="exception">The exception when proceed the invocation.</param>
        protected virtual void HandleException(IInvocation invocation, Exception exception)
        {
        }

        private void InterceptTask(IInvocation invocation)
        {
            var invocationClone = invocation.Clone();
            invocation.ReturnValue = Task.Factory
                .StartNew(() => this.BeforeInvoke(invocation))
                .ContinueWith(t =>
                    {
                        invocationClone.Proceed();
                        return invocationClone.ReturnValue as Task;
                    }).Unwrap()
                .ContinueWith(t =>
                            {
                                if (t.IsFaulted)
                                {
                                    this.HandleException(invocationClone, t.Exception);
                                }
                                this.AfterInvoke(invocation);
                                this.AfterInvoke(invocation, t);
                            });
        }

        private void InterceptTaskWithResult<TResult>(IInvocation invocation)
        {
            var invocationClone = invocation.Clone();
            invocation.ReturnValue = Task.Factory
                .StartNew(() => this.BeforeInvoke(invocation))
                .ContinueWith(t =>
                    {
                        invocationClone.Proceed();
                        return invocationClone.ReturnValue as Task<TResult>;
                    }).Unwrap()
                .ContinueWith(t =>
                        {
                            if (t.IsFaulted)
                            {
                                this.HandleException(invocationClone, t.Exception);
                                invocationClone.ReturnValue = default(TResult);
                            }
                            else
                            {
                                invocationClone.ReturnValue = t.Result;
                            }
                            this.AfterInvoke(invocationClone);
                            this.AfterInvoke(invocationClone, t);
                            return (TResult)invocationClone.ReturnValue;
                        });
        }
    }
}