// -------------------------------------------------------------------------------------------------
// <copyright file="ObjectMethodsInvocation.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.ProxyFactory
{
    using System;
    using System.Reflection;

    using Castle.DynamicProxy;

    /// <summary>
    /// Represents the invocation on object methods.
    /// </summary>
    public class ObjectMethodsInvocation : IInvocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMethodsInvocation"/> class.
        /// </summary>
        /// <param name="proxy">The proxy instance.</param>
        /// <param name="target">The target instance.</param>
        /// <param name="method">The method info.</param>
        /// <param name="arguments">The arguments.</param>
        public ObjectMethodsInvocation(object proxy, object target, MethodInfo method, object[] arguments)
        {
            this.Proxy = proxy;
            this.InvocationTarget = target;
            this.Method = method;
            this.MethodInvocationTarget = method;
            this.Arguments = arguments;
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public object[] Arguments { get; private set; }

        /// <summary>
        /// Gets the generic argument types.
        /// </summary>
        public Type[] GenericArguments
        {
            get { return new Type[0]; }
        }

        /// <summary>
        /// Gets the invocation target.
        /// </summary>
        public object InvocationTarget { get; private set; }

        /// <summary>
        /// Gets the method info.
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// Gets the method invocation target.
        /// </summary>
        public MethodInfo MethodInvocationTarget { get; private set; }

        /// <summary>
        /// Gets the proxy instance.
        /// </summary>
        public object Proxy { get; private set; }

        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public Type TargetType
        {
            get { return this.InvocationTarget.GetType(); }
        }

        /// <summary>
        /// Gets the argument value.
        /// </summary>
        /// <param name="index">The argument index.</param>
        /// <returns>The argument value.</returns>
        public object GetArgumentValue(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the concrete method info.
        /// </summary>
        /// <returns>The concrete method info.</returns>
        public MethodInfo GetConcreteMethod()
        {
            return this.Method;
        }

        /// <summary>
        /// Gets the concrete method invocation target.
        /// </summary>
        /// <returns>The concrete method invocation target.</returns>
        public MethodInfo GetConcreteMethodInvocationTarget()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Proceeds the invocation.
        /// </summary>
        public void Proceed()
        {
            this.Method.Invoke(this.InvocationTarget, this.Arguments);
        }

        /// <inheritdoc />
        public IInvocationProceedInfo CaptureProceedInfo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the argument value.
        /// </summary>
        /// <param name="index">The argument index.</param>
        /// <param name="value">The argument value.</param>
        public void SetArgumentValue(int index, object value)
        {
            throw new NotImplementedException();
        }
    }
}