// -------------------------------------------------------------------------------------------------
// <copyright file="ActionInterceptor.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception
{
    using System;

    /// <summary>
    /// Provides the ability to supply an action which will be invoked during method inteterception.
    /// </summary>
    public class ActionInterceptor : IInterceptor
    {
        private readonly Action<IInvocation> interceptAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInterceptor"/> class.
        /// </summary>
        /// <param name="interceptAction">The intercept action to take.</param>
        public ActionInterceptor(Action<IInvocation> interceptAction)
        {
            this.interceptAction = interceptAction;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            this.interceptAction(invocation);
        }
    }
}