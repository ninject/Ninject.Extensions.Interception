#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

namespace Ninject.Extensions.Interception
{
    /// <summary>
    /// A simple definition of an interceptor, which can take action both before and after
    /// the invocation proceeds.
    /// </summary>
    public abstract class SimpleInterceptor : IInterceptor
    {
        #region IInterceptor Members

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept( IInvocation invocation )
        {
            BeforeInvoke( invocation );
            invocation.Proceed();
            AfterInvoke( invocation );
        }

        #endregion

        /// <summary>
        /// Takes some action before the invocation proceeds.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void BeforeInvoke( IInvocation invocation )
        {
        }

        /// <summary>
        /// Takes some action after the invocation proceeds.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void AfterInvoke( IInvocation invocation )
        {
        }
    }
}