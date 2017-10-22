// -------------------------------------------------------------------------------------------------
// <copyright file="KernelExtensions.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Infrastructure.Language
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Ninject.Activation;
    using Ninject.Extensions.Interception.Advice;
    using Ninject.Extensions.Interception.Advice.Builders;
    using Ninject.Extensions.Interception.Advice.Syntax;
    using Ninject.Extensions.Interception.Registry;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Provides extensions methods on the <see cref="IKernel"/> interface.
    /// </summary>
    public static class KernelExtensions
    {
        /// <summary>
        /// Begins a dynamic interception definition.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="predicate">The predicate to run when determining whether to intercept the method call.</param>
        /// <returns>An advice builder.</returns>
        public static IAdviceTargetSyntax Intercept(this IKernel kernel, Predicate<IContext> predicate)
        {
            return Intercept(kernel, predicate, mi => mi.DeclaringType != typeof(object));
        }

        /// <summary>
        /// Begins a dynamic interception definition.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="condition">The condition to evaluate to determine if a request should be intercepted.</param>
        /// <param name="methodPredicate">The condition to evaluate if a method call should be intercepted.</param>
        /// <returns>An advice builder.</returns>
        public static IAdviceTargetSyntax Intercept(this IKernel kernel, Predicate<IContext> condition, Predicate<MethodInfo> methodPredicate)
        {
            IAdvice advice = kernel.Components.Get<IAdviceFactory>().Create(condition, methodPredicate);
            kernel.Components.Get<IAdviceRegistry>().Register(advice);

            return CreateAdviceBuilder(advice);
        }

        /// <summary>
        /// Adds the method interceptor.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="method">The method to intercept.</param>
        /// <param name="action">The action to take in its place.</param>
        public static void AddMethodInterceptor(
            this IKernel kernel,
            MethodInfo method,
            Action<IInvocation> action)
        {
            var interceptor = new ActionInterceptor(action);
            kernel.Components.Get<IMethodInterceptorRegistry>().Add(method, interceptor);
        }

        /// <summary>
        /// Intercepts the given method call and replaces it with the proxy action.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="methodExpr">The method to intercept.</param>
        /// <param name="action">The action to take in its place.</param>
        public static void InterceptReplace<T>(
            this IKernel kernel,
            Expression<Action<T>> methodExpr,
            Action<IInvocation> action)
        {
            kernel.AddMethodInterceptor(GetMethodFromExpression(methodExpr), action);
        }

        /// <summary>
        /// Intercepts the action surrounding the invocation with the two interceptor calls.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="methodExpr">The method to wrap.</param>
        /// <param name="beforeAction">The before action.</param>
        /// <param name="afterAction">The after action.</param>
        public static void InterceptAround<T>(
            this IKernel kernel,
            Expression<Action<T>> methodExpr,
            Action<IInvocation> beforeAction,
            Action<IInvocation> afterAction)
        {
            kernel.InterceptReplace(
                methodExpr,
                i =>
                {
                    beforeAction(i);
                    i.Proceed();
                    afterAction(i);
                });
        }

        /// <summary>
        /// Intercepts the given action and executes the interception action prior to continuing execution.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="methodExpr">The method to intercept.</param>
        /// <param name="action">The action to execute before.</param>
        public static void InterceptBefore<T>(
            this IKernel kernel,
            Expression<Action<T>> methodExpr,
            Action<IInvocation> action)
        {
            kernel.InterceptAround(methodExpr, action, i => { });
        }

        /// <summary>
        /// Intercepts the given action and executes the interception action after the method is executed.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="methodExpr">The method to intercept.</param>
        /// <param name="action">The action to execute before.</param>
        public static void InterceptAfter<T>(
            this IKernel kernel,
            Expression<Action<T>> methodExpr,
            Action<IInvocation> action)
        {
            kernel.InterceptAround(methodExpr, i => { }, action);
        }

        /// <summary>
        /// Intercepts and the replaces get call of a property.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="propertyExpr">The get_ property expression to replace.</param>
        /// <param name="action">The action to take in its place.</param>
        public static void InterceptReplaceGet<T>(
            this IKernel kernel,
            Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> action)
        {
            kernel.AddMethodInterceptor(GetGetterFromExpression(propertyExpr), action);
        }

        /// <summary>
        /// Intercepts the action surrounding the invocation with the two interceptor calls.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="propertyExpr">The property expression.</param>
        /// <param name="beforeAction">The before action.</param>
        /// <param name="afterAction">The after action.</param>
        public static void InterceptAroundGet<T>(
            this IKernel kernel,
            Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> beforeAction,
            Action<IInvocation> afterAction)
        {
            kernel.InterceptReplaceGet(
                propertyExpr,
                i =>
                {
                    beforeAction(i);
                    i.Proceed();
                    afterAction(i);
                });
        }

        /// <summary>
        /// Intercepts the get call and runs the action prior to executing the original expression.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="propertyExpr">The property expr.</param>
        /// <param name="action">The action.</param>
        public static void InterceptBeforeGet<T>(
            this IKernel kernel,
            Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> action)
        {
            kernel.InterceptAroundGet(propertyExpr, action, i => { });
        }

        /// <summary>
        /// Intercepts the get call and runs the specified interception action after the get call is made.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="propertyExpr">The property expr.</param>
        /// <param name="action">The action.</param>
        public static void InterceptAfterGet<T>(
            this IKernel kernel,
            Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> action)
        {
            kernel.InterceptAroundGet(propertyExpr, i => { }, action);
        }

        /// <summary>
        /// Intercepts and the replaces set call of a property.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="propertyExpr">The set_ property expression to replace.</param>
        /// <param name="action">The action to take in its place.</param>
        public static void InterceptReplaceSet<T>(
            this IKernel kernel,
            Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> action)
        {
            kernel.AddMethodInterceptor(GetSetterFromExpression(propertyExpr), action);
        }

        /// <summary>
        /// Intercepts the action surrounding the invocation with the two interceptor calls.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="propertyExpr">The property expression.</param>
        /// <param name="beforeAction">The before action.</param>
        /// <param name="afterAction">The after action.</param>
        public static void InterceptAroundSet<T>(
            this IKernel kernel,
            Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> beforeAction,
            Action<IInvocation> afterAction)
        {
            kernel.InterceptReplaceSet(
                propertyExpr,
                i =>
                {
                    beforeAction(i);
                    i.Proceed();
                    afterAction(i);
                });
        }

        /// <summary>
        /// Intercepts the set call and runs the action prior to executing the original expression.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="propertyExpr">The property expr.</param>
        /// <param name="action">The action.</param>
        public static void InterceptBeforeSet<T>(
            this IKernel kernel,
            Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> action)
        {
            kernel.InterceptAroundSet(propertyExpr, action, i => { });
        }

        /// <summary>
        /// Intercepts the set call and runs the specified interception action after the set call is made.
        /// </summary>
        /// <typeparam name="T">The type of the target.</typeparam>
        /// <param name="kernel">The kernel.</param>
        /// <param name="propertyExpr">The property expr.</param>
        /// <param name="action">The action.</param>
        public static void InterceptAfterSet<T>(
            this IKernel kernel,
            Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> action)
        {
            kernel.InterceptAroundSet(propertyExpr, i => { }, action);
        }

        /// <summary>
        /// Creates an advice builder.
        /// </summary>
        /// <param name="advice">The advice that will be built.</param>
        /// <returns>The created builder.</returns>
        internal static IAdviceTargetSyntax CreateAdviceBuilder(IAdvice advice)
        {
            return new AdviceBuilder(advice);
        }

        private static MethodInfo GetMethodFromExpression<T>(Expression<Action<T>> methodExpr)
        {
            var call = methodExpr.Body as MethodCallExpression;
            if (call == null)
            {
                throw new InvalidOperationException("Expression must be a method call");
            }

            if (call.Object !=
                 methodExpr.Parameters[0])
            {
                throw new InvalidOperationException("Method call must target lambda argument");
            }

            return call.Method;
        }

        private static MethodInfo GetGetterFromExpression<T>(Expression<Func<T, object>> propertyExpr)
        {
            PropertyInfo propertyInfo = GetPropertyFromExpression(propertyExpr);
            if (!propertyInfo.CanRead)
            {
                throw new InvalidOperationException("Property must be readable");
            }

            return propertyInfo.GetGetMethod();
        }

        private static MethodInfo GetSetterFromExpression<T>(Expression<Func<T, object>> propertyExpr)
        {
            PropertyInfo propertyInfo = GetPropertyFromExpression(propertyExpr);
            if (!propertyInfo.CanWrite)
            {
                throw new InvalidOperationException("Property must be writable");
            }

            return propertyInfo.GetSetMethod();
        }

        private static PropertyInfo GetPropertyFromExpression<T>(Expression<Func<T, object>> propertyExpr)
        {
            Expression body = propertyExpr.Body;
            if (body is UnaryExpression &&
                 body.NodeType == ExpressionType.Convert)
            {
                body = ((UnaryExpression)body).Operand;
            }

            var memberExpr = body as MemberExpression;
            if (memberExpr == null ||
                 !(memberExpr.Member is PropertyInfo))
            {
                throw new InvalidOperationException("Expression must be a property access");
            }

            if (memberExpr.Expression !=
                 propertyExpr.Parameters[0])
            {
                throw new InvalidOperationException("Method call must target lambda argument");
            }

            return (PropertyInfo)memberExpr.Member;
        }
    }
}