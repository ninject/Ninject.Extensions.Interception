#region License

// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Components;

#endregion

namespace Ninject.Extensions.Interception.Registry
{
    public class MethodInterceptorRegistry : NinjectComponent, IMethodInterceptorRegistry
    {
        private readonly Dictionary<Type, MethodInterceptorCollection> _typeMethods =
            new Dictionary<Type, MethodInterceptorCollection>();

        protected Dictionary<Type, MethodInterceptorCollection> TypeMethods { get { return _typeMethods; } }

        #region IMethodInterceptorRegistry Members

        public void Add( MethodInfo method, IInterceptor interceptor )
        {
            Type type = method.DeclaringType;
            if ( !TypeMethods.ContainsKey( type ) )
            {
                TypeMethods.Add( type, new MethodInterceptorCollection() );
            }
            TypeMethods[type].Add( method, interceptor );
        }

        public bool Contains( Type type )
        {
            return TypeMethods.ContainsKey( type );
        }

        public MethodInterceptorCollection GetMethodInterceptors( Type type )
        {
            return TypeMethods[type];
        }

        #endregion
    }
}