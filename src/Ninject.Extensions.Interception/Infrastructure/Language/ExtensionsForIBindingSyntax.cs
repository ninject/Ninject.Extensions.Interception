// -------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsForIBindingSyntax.cs" company="Ninject Project Contributors">
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
    using System.Reflection;

    using Ninject.Extensions.Interception.Advice;
    using Ninject.Extensions.Interception.Advice.Builders;
    using Ninject.Extensions.Interception.Advice.Syntax;
    using Ninject.Extensions.Interception.Parameters;
    using Ninject.Extensions.Interception.Registry;
    using Ninject.Syntax;

    /// <summary>
    /// Provides extension methods for <see cref="IBindingOnSyntax{T}"/>.
    /// </summary>
    public static class ExtensionsForIBindingSyntax
    {
        /// <summary>
        /// Indicates that instances associated with this binding will be proxied.
        /// </summary>
        /// <typeparam name="T">The type associated with this binding.</typeparam>
        /// <param name="bindingSyntax">The binding syntax target.</param>
        /// <param name="additionalInterfaces">The additional interfaces for the proxy.</param>
        /// <returns>
        ///     An <see cref="IAdviceTargetSyntax"/> instance which allows the attachment of an <see cref="IInterceptor"/>.
        /// </returns>
        public static IAdviceTargetSyntax Intercept<T>(this IBindingOnSyntax<T> bindingSyntax, params Type[] additionalInterfaces)
        {
            return Intercept(
                bindingSyntax,
                mi => mi.DeclaringType != typeof(object),
                additionalInterfaces);
        }

        /// <summary>
        /// Indicates that instances associated with this binding will be proxied.
        /// Only methods that match the specified predicate will be intercepted.
        /// </summary>
        /// <typeparam name="T">The type associated with this binding.</typeparam>
        /// <param name="bindingSyntax">The binding syntax target.</param>
        /// <param name="methodPredicate">The method predicate that defines if a method shall be intercepted.</param>
        /// <param name="additionalInterfaces">The additional interfaces for the proxy.</param>
        /// <returns>An <see cref="IAdviceTargetSyntax" /> instance which allows the attachment of an <see cref="IInterceptor" />.</returns>
        public static IAdviceTargetSyntax Intercept<T>(this IBindingOnSyntax<T> bindingSyntax, Predicate<MethodInfo> methodPredicate, params Type[] additionalInterfaces)
        {
            IKernel kernel = bindingSyntax.Kernel;

            foreach (var additionalInterface in additionalInterfaces)
            {
                bindingSyntax.BindingConfiguration.Parameters.Add(new AdditionalInterfaceParameter(additionalInterface));
            }

            IAdvice advice = kernel.Components.Get<IAdviceFactory>()
                .Create(context => ReferenceEquals(bindingSyntax.BindingConfiguration, context.Binding.BindingConfiguration), methodPredicate);
            kernel.Components.Get<IAdviceRegistry>().Register(advice);

            return new AdviceBuilder(advice);
        }
    }
}