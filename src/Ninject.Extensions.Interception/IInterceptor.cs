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
    /// Intercepts a method call on an activated instance.
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        void Intercept( IInvocation invocation );
    }
}