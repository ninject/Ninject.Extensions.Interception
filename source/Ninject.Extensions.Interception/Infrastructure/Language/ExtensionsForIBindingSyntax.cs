#region License

// 
// Author: Ian Davis <ian@innovatian.com>
// Copyright (c) 2010, Innovatian Software, LLC
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Advice.Builders;
using Ninject.Extensions.Interception.Advice.Syntax;
using Ninject.Extensions.Interception.Registry;
using Ninject.Syntax;

#endregion

namespace Ninject.Extensions.Interception.Infrastructure.Language
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtensionsForIBindingSyntax
    {
        /// <summary>
        /// Indicates that instances associated with this binding will be proxied.
        /// </summary>
        /// <typeparam name="T">The type associated with this binding.</typeparam>
        /// <param name="bindingSyntax">The binding syntax target.</param>
        /// <returns>
        ///     An <see cref="IAdviceTargetSyntax"/> instance which allows the attachment of an <see cref="IInterceptor"/>.
        /// </returns>
        public static IAdviceTargetSyntax Intercept<T>( this IBindingWhenInNamedWithOrOnSyntax<T> bindingSyntax )
        {
            return DoIntercept( bindingSyntax );
        }

        /// <summary>
        /// Indicates that instances associated with this binding will be proxied.
        /// </summary>
        /// <typeparam name="T">The type associated with this binding.</typeparam>
        /// <param name="bindingSyntax">The binding syntax target.</param>
        /// <returns>
        ///     An <see cref="IAdviceTargetSyntax"/> instance which allows the attachment of an <see cref="IInterceptor"/>.
        /// </returns>
        public static IAdviceTargetSyntax Intercept<T>( this IBindingInNamedWithOrOnSyntax<T> bindingSyntax )
        {
            return DoIntercept( bindingSyntax );
        }

        /// <summary>
        /// Indicates that instances associated with this binding will be proxied.
        /// </summary>
        /// <typeparam name="T">The type associated with this binding.</typeparam>
        /// <param name="bindingSyntax">The binding syntax target.</param>
        /// <returns>
        ///     An <see cref="IAdviceTargetSyntax"/> instance which allows the attachment of an <see cref="IInterceptor"/>.
        /// </returns>
        public static IAdviceTargetSyntax Intercept<T>( this IBindingNamedWithOrOnSyntax<T> bindingSyntax )
        {
            return DoIntercept( bindingSyntax );
        }

        /// <summary>
        /// Indicates that instances associated with this binding will be proxied.
        /// </summary>
        /// <typeparam name="T">The type associated with this binding.</typeparam>
        /// <param name="bindingSyntax">The binding syntax target.</param>
        /// <returns>
        ///     An <see cref="IAdviceTargetSyntax"/> instance which allows the attachment of an <see cref="IInterceptor"/>.
        /// </returns>
        public static IAdviceTargetSyntax Intercept<T>( this IBindingWithOrOnSyntax<T> bindingSyntax )
        {
            return DoIntercept( bindingSyntax );
        }

        /// <summary>
        /// Constructs the interception advice to trigger according to the binding used.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>
        ///     An <see cref="IAdviceTargetSyntax"/> instance which allows the attachment of an <see cref="IInterceptor"/>.
        /// </returns>
        private static IAdviceTargetSyntax DoIntercept( IBindingSyntax binding )
        {
            IKernel kernel = binding.Kernel;
            IAdvice advice = kernel.Components.Get<IAdviceFactory>().Create(
                request => binding.Binding.Matches( request.Context.Request ) );
            kernel.Components.Get<IAdviceRegistry>().Register( advice );

            return new AdviceBuilder( advice );
        }
    }
}