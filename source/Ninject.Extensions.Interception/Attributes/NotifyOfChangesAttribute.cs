#region License

// 
// Author: Ian Davis <ian@innovatian.com>
// Copyright (c) 2010, Innovatian Software, LLC
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Attributes
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
    public class NotifyOfChangesAttribute : InterceptAttribute
    {
        private static readonly Type InterceptorType = typeof (AutoNotifyPropertyChangedInterceptor<>);

        public NotifyOfChangesAttribute():this(new string[]{})
        {
        }

        public NotifyOfChangesAttribute( params string[] notifyChangeFor )
        {
            NotifyChangeFor = notifyChangeFor;
        }

        public string[] NotifyChangeFor { get; private set; }

        /// <summary>
        /// Creates the interceptor associated with the attribute.
        /// </summary>
        /// <param name="request">The request that is being intercepted.</param>
        /// <returns>The interceptor.</returns>
        public override IInterceptor CreateInterceptor( IProxyRequest request )
        {
            Type targetType = request.Target.GetType();
            Type closedInterceptorType = InterceptorType.MakeGenericType( targetType );
            var interceptor = (IInterceptor) request.Context.Kernel.Get( closedInterceptorType );
            return interceptor;
        }
    }
}