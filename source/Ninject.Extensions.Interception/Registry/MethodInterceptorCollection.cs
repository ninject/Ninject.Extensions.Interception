#region License

// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System.Collections.Generic;
using System.Reflection;

#endregion

namespace Ninject.Extensions.Interception.Registry
{
    /// <summary>
    /// Provides lookup table functionality to trace <see cref="MethodInfo"/> to a collection of <see cref="IInterceptor"/>.
    /// </summary>
    public class MethodInterceptorCollection : Dictionary<MethodInfo, List<IInterceptor>>
    {
        /// <summary>
        /// Adds the specified interceptor for the given method.
        /// </summary>
        /// <param name="method">The method to bind the interceptor to.</param>
        /// <param name="interceptor">The interceptor to add.</param>
        public void Add( MethodInfo method, IInterceptor interceptor )
        {
            if ( !ContainsKey( method ) )
            {
                Add( method, new List<IInterceptor>() );
            }
            this[method].Add( interceptor );
        }
    }
}