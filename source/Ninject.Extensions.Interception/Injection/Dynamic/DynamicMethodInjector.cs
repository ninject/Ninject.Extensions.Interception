#if !NO_LCG

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

using System.Reflection;

#endregion

namespace Ninject.Extensions.Interception.Injection.Dynamic
{
    /// <summary>
    /// A method injector that uses a dynamically-generated <see cref="Invoker"/> for invocation.
    /// </summary>
    public class DynamicMethodInjector : InjectorBase<MethodInfo>, IMethodInjector
    {
        private Invoker _invoker;

        /// <summary>
        /// Creates a new DynamicMethodInjector.
        /// </summary>
        /// <param name="member">The method that will be injected.</param>
        public DynamicMethodInjector( MethodInfo member )
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
            if ( _invoker == null )
            {
                _invoker = DynamicMethodFactory.CreateInvoker( Member );
            }

            return _invoker.Invoke( target, arguments );
        }

        #endregion
    }
}

#endif //!NO_LCG