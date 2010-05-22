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
using System.Reflection;

#endregion

namespace Ninject.Extensions.Interception.Injection.Reflection
{
    /// <summary>
    /// A method injector that uses reflection for invocation.
    /// </summary>
    public class ReflectionMethodInjector : InjectorBase<MethodInfo>, IMethodInjector
    {
        /// <summary>
        /// Creates a new ReflectionMethodInjector.
        /// </summary>
        /// <param name="member">The method that will be injected.</param>
        public ReflectionMethodInjector( MethodInfo member )
            : base( member )
        {
        }

        #region IMethodInjector Members

        /// <summary>
        /// Calls the method associated with the injector.
        /// </summary>
        /// <param name="target">The instance on which to call the method.</param>
        /// <param name="arguments">The arguments to pass to the method.</param>
        /// <returns>The return value of the method.</returns>
        public object Invoke( object target, params object[] arguments )
        {
            object result = null;

            try
            {
                result = Member.Invoke( target, arguments );
            }
            catch ( TargetInvocationException ex )
            {
                // If an exception occurs inside the called member, unwrap it and re-throw.
                RethrowPreservingStackTrace( ex.InnerException ?? ex );
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Re-throws the specified exception, preserving its internal stack trace.
        /// </summary>
        /// <param name="ex">The exception to re-throw.</param>
        private static void RethrowPreservingStackTrace( Exception ex )
        {
            FieldInfo stackTraceField = typeof (Exception).GetField( "_remoteStackTraceString",
                                                                     BindingFlags.Instance | BindingFlags.NonPublic );

            stackTraceField.SetValue( ex, ex.StackTrace );

            throw ex;
        }
    }
}