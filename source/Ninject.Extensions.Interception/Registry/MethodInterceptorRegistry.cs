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
    /// <summary>
    /// Provides a registry of <see cref="IInterceptor"/> and <see cref="MethodInfo"/> bindings for interception.
    /// </summary>
    public class MethodInterceptorRegistry : NinjectComponent, IMethodInterceptorRegistry
    {
        private readonly Dictionary<Type, MethodInterceptorCollection> _typeMethods =
            new Dictionary<Type, MethodInterceptorCollection>();

        /// <summary>
        /// Gets the type method lookup table.
        /// </summary>
        /// <value>The type methods.</value>
        protected Dictionary<Type, MethodInterceptorCollection> TypeMethods
        {
            get { return _typeMethods; }
        }

        #region IMethodInterceptorRegistry Members

        /// <summary>
        /// Adds the specified interceptor for the given method.
        /// </summary>
        /// <param name="method">The method to bind the interceptor to.</param>
        /// <param name="interceptor">The interceptor to add.</param>
        public void Add( MethodInfo method, IInterceptor interceptor )
        {
            Type type = method.DeclaringType;
            if ( !TypeMethods.ContainsKey( type ) )
            {
                TypeMethods.Add( type, new MethodInterceptorCollection() );
            }
            TypeMethods[type].Add( method, interceptor );
        }

        /// <summary>
        /// Determines whether the registry contains interceptors for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the registry contains interceptors for the specified type; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains( Type type )
        {
            return TypeMethods.ContainsKey( type );
        }

        /// <summary>
        /// Gets the method interceptors bindings associated with the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<see cref="MethodInfo"/> and <see cref="IInterceptor"/> bindings for the given type.
        /// </returns>
        public MethodInterceptorCollection GetMethodInterceptors( Type type )
        {
            return TypeMethods[type];
        }

        #endregion
    }
}