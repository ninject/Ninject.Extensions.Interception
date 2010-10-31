#region License

// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;

#endregion

namespace Ninject.Extensions.Interception
{
    /// <summary>
    /// Provides the ability to supply an action which will be invoked during method inteterception.
    /// </summary>
    public class ActionInterceptor : IInterceptor
    {
        private readonly Action<IInvocation> _interceptAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInterceptor"/> class.
        /// </summary>
        /// <param name="interceptAction">The intercept action to take.</param>
        public ActionInterceptor( Action<IInvocation> interceptAction )
        {
            _interceptAction = interceptAction;
        }

        #region IInterceptor Members

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept( IInvocation invocation )
        {
            _interceptAction( invocation );
        }

        #endregion
    }
}