#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using Ninject.Extensions.Interception.Advice.Syntax;
using Ninject.Extensions.Interception.Infrastructure;
using Ninject.Extensions.Interception.Request;
using Ninject.Interception;

#endregion

namespace Ninject.Extensions.Interception.Advice.Builders
{
    /// <summary>
    /// The stock definition of an advice builder.
    /// </summary>
    public class AdviceBuilder : DisposableObject, IAdviceBuilder, IAdviceTargetSyntax, IAdviceOrderSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdviceBuilder"/> class.
        /// </summary>
        /// <param name="advice">The advice that should be manipulated.</param>
        public AdviceBuilder( IAdvice advice )
        {
            Ensure.ArgumentNotNull( advice, "advice" );
            Advice = advice;
        }

        #region IAdviceBuilder Members

        /// <summary>
        /// Gets or sets the advice the builder should manipulate.
        /// </summary>
        public IAdvice Advice { get; protected set; }

        #endregion

        #region IAdviceOrderSyntax Members

        void IAdviceOrderSyntax.InOrder( int order )
        {
            Advice.Order = order;
        }

        #endregion

        #region IAdviceTargetSyntax Members

        IAdviceOrderSyntax IAdviceTargetSyntax.With<T>()
        {
            Advice.Callback = r => r.Kernel.Get<T>();
            return this;
        }

        IAdviceOrderSyntax IAdviceTargetSyntax.With( Type interceptorType )
        {
            Advice.Callback = r => r.Kernel.Get( interceptorType ) as IInterceptor;
            return this;
        }

        IAdviceOrderSyntax IAdviceTargetSyntax.With( IInterceptor interceptor )
        {
            Advice.Interceptor = interceptor;
            return this;
        }

        IAdviceOrderSyntax IAdviceTargetSyntax.With( Func<IProxyRequest, IInterceptor> factoryMethod )
        {
            Advice.Callback = factoryMethod;
            return this;
        }

        #endregion
    }
}