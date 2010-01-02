#region License

// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using Ninject.Extensions.Interception.Infrastructure;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Attributes
{
    internal sealed class InternalInterceptAttribute : InterceptAttribute
    {
        public InternalInterceptAttribute( Func<IProxyRequest, IInterceptor> createCallback )
        {
            Ensure.ArgumentNotNull( createCallback, "createCallback" );
            CreateCallback = createCallback;
        }

        #region Overrides of InterceptAttribute

        /// <summary>
        /// Creates the interceptor associated with the attribute.
        /// </summary>
        /// <param name="request">The request that is being intercepted.</param>
        /// <returns>The interceptor.</returns>
        public override IInterceptor CreateInterceptor( IProxyRequest request )
        {
            return CreateCallback( request );
        }

        #endregion

        public Func<IProxyRequest, IInterceptor> CreateCallback { get; set; }
    }
}