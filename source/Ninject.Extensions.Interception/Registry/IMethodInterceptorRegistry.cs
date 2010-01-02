#region License

// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Reflection;
using Ninject.Components;

#endregion

namespace Ninject.Extensions.Interception.Registry
{
    public interface IMethodInterceptorRegistry : INinjectComponent
    {
        void Add( MethodInfo method, IInterceptor interceptor );
        bool Contains( Type type );
        MethodInterceptorCollection GetMethodInterceptors( Type type );
    }
}