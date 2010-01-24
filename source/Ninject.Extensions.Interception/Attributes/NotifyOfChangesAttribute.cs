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
using System.Linq;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Attributes
{
    /// <summary>
    /// Provides interceptor's details on how this property or class should participate in <see cref="IAutoNotifyPropertyChanged"/> events.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
    public class NotifyOfChangesAttribute : InterceptAttribute
    {
        private static readonly Type DefaultInterceptorType = typeof (AutoNotifyPropertyChangedInterceptor<>);
        private static readonly Type DefaultServiceType = typeof (IAutoNotifyPropertyChangedInterceptor<>);

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyOfChangesAttribute"/> class.
        /// </summary>
        public NotifyOfChangesAttribute()
            : this( new string[] {} )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyOfChangesAttribute"/> class.
        /// </summary>
        /// <param name="notifyChangeFor">The dependent properties to also trigger events for.</param>
        public NotifyOfChangesAttribute( params string[] notifyChangeFor )
        {
            NotifyChangeFor = notifyChangeFor;
        }

        /// <summary>
        /// Gets or sets the additional properties to notify changes for.
        /// </summary>
        /// <value>The notify change for.</value>
        public string[] NotifyChangeFor { get; private set; }

        /// <summary>
        /// Creates the interceptor associated with the attribute.
        /// </summary>
        /// <param name="request">The request that is being intercepted.</param>
        /// <returns>The interceptor.</returns>
        public override IInterceptor CreateInterceptor( IProxyRequest request )
        {
            Type targetType = request.Target.GetType();
            Type serviceType = request.Kernel.GetBindings( DefaultServiceType ).Any()
                                   ? DefaultServiceType
                                   : DefaultInterceptorType;
            Type closedInterceptorType = serviceType.MakeGenericType( targetType );
            var interceptor = (IInterceptor) request.Context.Kernel.Get( closedInterceptorType );
            return interceptor;
        }
    }
}