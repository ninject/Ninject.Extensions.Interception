// -------------------------------------------------------------------------------------------------
// <copyright file="ProxyBase.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.ProxyFactory
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Castle.DynamicProxy;

    using Ninject.Extensions.Interception.Wrapper;

    /// <summary>
    /// The proxy base.
    /// </summary>
    public class ProxyBase
    {
        private static readonly MethodInfo ToStringMethodInfo = typeof(object).GetMethod("ToString");
        private static readonly MethodInfo GetHashCodeMethodInfo = typeof(object).GetMethod("GetHashCode");
        private static readonly MethodInfo EqualsMethodInfo = typeof(object).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// Intercepts the <c>GetHashCode</c> method using object's <c>GetHashCode</c> method.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.InterceptMethod(() => base.GetHashCode(), GetHashCodeMethodInfo, new object[0]);
        }

        /// <summary>
        /// Intercepts the <c>ToString</c> method using object's <c>ToString</c> method.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return this.InterceptMethod(() => base.ToString(), ToStringMethodInfo, new object[0]);
        }

        /// <summary>
        /// Intercepts the <c>Equals</c> method using object's <c>Equals</c> method.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><c>True</c> if the two equals, otherwise <c>False</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.InterceptMethod(() => base.Equals(obj), EqualsMethodInfo, new[] { obj });
        }

        private TResult InterceptMethod<TResult>(Func<TResult> invokeBase, MethodInfo method, object[] arguments)
        {
            var proxy = this as IProxyTargetAccessor;
            if (proxy == null)
            {
                return invokeBase();
            }

            var interceptor = proxy.GetInterceptors().FirstOrDefault() as DynamicProxyWrapper;
            if (interceptor == null)
            {
                return invokeBase();
            }

            var invocation = new ObjectMethodsInvocation(this, interceptor.Instance, method, arguments);
            interceptor.Intercept(invocation);
            return (TResult)invocation.ReturnValue;
        }
    }
}