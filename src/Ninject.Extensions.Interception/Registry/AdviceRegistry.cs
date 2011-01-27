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
using System.Linq;
using Ninject.Components;
using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Registry
{
    using Ninject.Activation;

    /// <summary>
    /// Collects advice defined for methods.
    /// </summary>
    public class AdviceRegistry : NinjectComponent, IAdviceRegistry
    {
        private readonly List<IAdvice> _advice = new List<IAdvice>();

        private readonly Dictionary<RuntimeMethodHandle, List<IInterceptor>> _cache =
            new Dictionary<RuntimeMethodHandle, List<IInterceptor>>();

        #region IAdviceRegistry Members

        /// <summary>
        /// Gets a value indicating whether one or more dynamic interceptors have been registered.
        /// </summary>
        public bool HasDynamicAdvice { get; private set; }

        /// <summary>
        /// Determines whether an advice for the specified context exists.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// 	<c>true</c> if an advice for the specified context exists.; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAdvice(IContext context)
        {
            return this._advice.Any(a => a.IsDynamic && (a.Condition == null || a.Condition(context)));
        }

        /// <summary>
        /// Registers the specified advice.
        /// </summary>
        /// <param name="advice">The advice to register.</param>
        public void Register( IAdvice advice )
        {
            if ( advice.IsDynamic )
            {
                HasDynamicAdvice = true;
                _cache.Clear();
            }

            _advice.Add( advice );
        }

        /// <summary>
        /// Determines whether any static advice has been registered for the specified type.
        /// </summary>
        /// <param name="type">The type in question.</param>
        /// <returns><see langword="True"/> if advice has been registered, otherwise <see langword="false"/>.</returns>
        public bool HasStaticAdvice( Type type )
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Gets the interceptors that should be invoked for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A collection of interceptors, ordered by the priority in which they should be invoked.</returns>
        public ICollection<IInterceptor> GetInterceptors( IProxyRequest request )
        {
            RuntimeMethodHandle handle = request.Method.GetMethodHandle();
            ICollection<IInterceptor> interceptors = null;

            lock ( _cache )
            {
                if ( _cache.ContainsKey( handle ) )
                {
                    return _cache[handle];
                }

                if ( HasDynamicAdvice && !_cache.ContainsKey( handle ) )
                {
                    interceptors = GetInterceptorsForRequest( request );
                    // If there are no dynamic interceptors defined, we can safely cache the results.
                    // Otherwise, we have to evaluate and re-activate the interceptors each time.
                    _cache.Add( handle, interceptors.ToList() );
                }
            }

            if ( interceptors == null )
            {
                interceptors = GetInterceptorsForRequest( request );
            }

            return interceptors;
        }

        #endregion

        private ICollection<IInterceptor> GetInterceptorsForRequest( IProxyRequest request )
        {
            List<IAdvice> matches = _advice.Where( advice => advice.Matches( request ) ).ToList();
            matches.Sort( ( lhs, rhs ) => lhs.Order - rhs.Order );

            List<IInterceptor> interceptors = matches.Convert( a => a.GetInterceptor( request ) ).ToList();
            return interceptors;
        }
    }
}