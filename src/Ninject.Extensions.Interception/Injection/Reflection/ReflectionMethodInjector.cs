// -------------------------------------------------------------------------------------------------
// <copyright file="ReflectionMethodInjector.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Injection.Reflection
{
    using System;
    using System.Reflection;

    /// <summary>
    /// A method injector that uses reflection for invocation.
    /// </summary>
    public class ReflectionMethodInjector : InjectorBase<MethodInfo>, IMethodInjector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionMethodInjector"/> class.
        /// </summary>
        /// <param name="member">The method that will be injected.</param>
        public ReflectionMethodInjector(MethodInfo member)
            : base(member)
        {
        }

        /// <summary>
        /// Calls the method associated with the injector.
        /// </summary>
        /// <param name="target">The instance on which to call the method.</param>
        /// <param name="arguments">The arguments to pass to the method.</param>
        /// <returns>The return value of the method.</returns>
        public object Invoke(object target, params object[] arguments)
        {
            object result = null;

            try
            {
                result = this.Member.Invoke(target, arguments);
            }
            catch (TargetInvocationException ex)
            {
                // If an exception occurs inside the called member, unwrap it and re-throw.
                RethrowPreservingStackTrace(ex.InnerException ?? ex);
            }

            return result;
        }

        /// <summary>
        /// Re-throws the specified exception, preserving its internal stack trace.
        /// </summary>
        /// <param name="ex">The exception to re-throw.</param>
        private static void RethrowPreservingStackTrace(Exception ex)
        {
            FieldInfo stackTraceField = typeof(Exception).GetField(
                "_remoteStackTraceString",
                BindingFlags.Instance | BindingFlags.NonPublic);

            stackTraceField.SetValue(ex, ex.StackTrace);

            throw ex;
        }
    }
}