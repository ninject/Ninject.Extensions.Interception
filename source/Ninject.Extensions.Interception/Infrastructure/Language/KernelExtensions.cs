#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Advice.Builders;
using Ninject.Extensions.Interception.Advice.Syntax;
using Ninject.Extensions.Interception.Registry;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Infrastructure.Language
{
    public static class KernelExtensions
    {
        public static IAdviceTargetSyntax Intercept( this IKernel kernel, Predicate<IProxyRequest> predicate )
        {
            return DoIntercept( kernel, predicate );
        }

        /// <summary>
        /// Begins a dynamic interception definition.
        /// </summary>
        /// <param name="condition">The condition to evaluate to determine if a request should be intercepted.</param>
        /// <returns>An advice builder.</returns>
        private static IAdviceTargetSyntax DoIntercept( IKernel kernel, Predicate<IProxyRequest> condition )
        {
            IAdvice advice = kernel.Components.Get<IAdviceFactory>().Create( condition );
            kernel.Components.Get<IAdviceRegistry>().Register( advice );

            return CreateAdviceBuilder( advice );
        }

        /// <summary>
        /// Creates an advice builder.
        /// </summary>
        /// <param name="advice">The advice that will be built.</param>
        /// <returns>The created builder.</returns>
        private static IAdviceTargetSyntax CreateAdviceBuilder( IAdvice advice )
        {
            return new AdviceBuilder( advice );
        }
    }
}