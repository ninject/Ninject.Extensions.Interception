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
    public class MethodInterceptorCollection : Dictionary<MethodInfo, List<IInterceptor>>
    {
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