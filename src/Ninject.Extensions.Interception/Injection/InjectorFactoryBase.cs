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
using System.Collections.Generic;
using System.Reflection;
using Ninject.Components;

#endregion

namespace Ninject.Extensions.Interception.Injection
{
    /// <summary>
    /// Creates instances of injectors.
    /// </summary>
    public abstract class InjectorFactoryBase : NinjectComponent, IInjectorFactory
    {
        private readonly Dictionary<MethodInfo, IMethodInjector> _methodInjectors =
            new Dictionary<MethodInfo, IMethodInjector>();

        #region IInjectorFactory Members

        /// <summary>
        /// Gets an injector for the specified method.
        /// </summary>
        /// <param name="method">The method that the injector will invoke.</param>
        /// <returns>A new injector for the method.</returns>
        public IMethodInjector GetInjector( MethodInfo method )
        {
            lock ( _methodInjectors )
            {
                if ( method.IsGenericMethodDefinition )
                {
                    throw new InvalidOperationException(
                        /*ExceptionFormatter.CannotCreateInjectorFromGenericTypeDefinition(method)*/ );
                }

                if ( _methodInjectors.ContainsKey( method ) )
                {
                    return _methodInjectors[method];
                }

                IMethodInjector injector = CreateInjector( method );
                _methodInjectors.Add( method, injector );

                return injector;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new method injector.
        /// </summary>
        /// <param name="method">The method that the injector will invoke.</param>
        /// <returns>A new injector for the method.</returns>
        protected abstract IMethodInjector CreateInjector( MethodInfo method );
    }
}