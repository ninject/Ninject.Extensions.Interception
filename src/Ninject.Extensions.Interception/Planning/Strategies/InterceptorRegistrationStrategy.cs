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
using System.Reflection;
using Ninject.Components;
using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Planning.Directives;
using Ninject.Extensions.Interception.Registry;
using Ninject.Planning;
using Ninject.Planning.Strategies;

#endregion

namespace Ninject.Extensions.Interception.Planning.Strategies
{
    /// <summary>
    /// Examines the implementation type via reflection and registers any static interceptors
    /// that are defined via attributes.
    /// </summary>
    public class InterceptorRegistrationStrategy : NinjectComponent, IPlanningStrategy
    {
        /// <summary>
        /// The default <see cref="BindingFlags"/> to use during reflection.
        /// </summary>
        protected const BindingFlags DefaultBindingFlags =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptorRegistrationStrategy"/> class.
        /// </summary>
        /// <param name="adviceFactory">The advice factory.</param>
        /// <param name="adviceRegistry">The advice registry.</param>
        public InterceptorRegistrationStrategy( IAdviceFactory adviceFactory, IAdviceRegistry adviceRegistry )
        {
            AdviceFactory = adviceFactory;
            AdviceRegistry = adviceRegistry;
        }

        /// <summary>
        /// Gets or sets the advice factory.
        /// </summary>
        /// <value>The advice factory.</value>
        public IAdviceFactory AdviceFactory { get; set; }

        /// <summary>
        /// Gets or sets the advice registry.
        /// </summary>
        /// <value>The advice registry.</value>
        public IAdviceRegistry AdviceRegistry { get; set; }

        #region IPlanningStrategy Members

        /// <summary>
        /// Contributes to the specified plan.
        /// </summary>
        /// <param name="plan">The plan that is being generated.</param>
        public virtual void Execute( IPlan plan )
        {
            IEnumerable<MethodInfo> candidates = GetCandidateMethods( plan.Type );

            RegisterClassInterceptors( plan.Type, plan, candidates );

            foreach ( MethodInfo method in candidates )
            {
                PropertyInfo property = method.GetPropertyFromMethod( plan.Type );
                ICustomAttributeProvider provider = (ICustomAttributeProvider) property ?? method;
                InterceptAttribute[] attributes = provider.GetAllAttributes<InterceptAttribute>();

                if ( attributes.Length == 0 )
                {
                    continue;
                }

                RegisterMethodInterceptors( plan.Type, method, attributes );

                // Indicate that instances of the type should be proxied.
                if ( !plan.Has<ProxyDirective>() )
                {
                    plan.Add( new ProxyDirective() );
                }
            }
        }

        #endregion

        /// <summary>
        /// Registers static interceptors defined by attributes on the class for all candidate
        /// methods on the class, execept those decorated with a <see cref="DoNotInterceptAttribute"/>.
        /// </summary>
        /// <param name="type">The type whose activation plan is being manipulated.</param>
        /// <param name="plan">The activation plan that is being manipulated.</param>
        /// <param name="candidates">The candidate methods to intercept.</param>
        protected virtual void RegisterClassInterceptors( Type type, IPlan plan, IEnumerable<MethodInfo> candidates )
        {
            InterceptAttribute[] attributes = type.GetAllAttributes<InterceptAttribute>();

            if ( attributes.Length == 0 )
            {
                return;
            }

            foreach ( MethodInfo method in candidates )
            {
                PropertyInfo property = method.GetPropertyFromMethod( type );
                ICustomAttributeProvider provider = (ICustomAttributeProvider) property ?? method;

                if ( !provider.HasAttribute<DoNotInterceptAttribute>() )
                {
                    RegisterMethodInterceptors( type, method, attributes );
                }
            }

            // Indicate that instances of the type should be proxied.
            if ( !plan.Has<ProxyDirective>() )
            {
                plan.Add( new ProxyDirective() );
            }
        }

        /// <summary>
        /// Registers static interceptors defined by attributes on the specified method.
        /// </summary>
        /// <param name="type">The type whose activation plan is being manipulated.</param>
        /// <param name="method">The method that may be intercepted.</param>
        /// <param name="attributes">The interception attributes that apply.</param>
        protected virtual void RegisterMethodInterceptors( Type type,
                                                           MethodInfo method,
                                                           ICollection<InterceptAttributeBase> attributes )
        {
            foreach ( InterceptAttributeBase attribute in attributes )
            {
                IAdvice advice = AdviceFactory.Create( method );

                advice.Callback = attribute.CreateInterceptor;
                advice.Order = attribute.Order;

                AdviceRegistry.Register( advice );
            }
        }

        /// <summary>
        /// Gets a collection of methods that may be intercepted on the specified type.
        /// </summary>
        /// <param name="type">The type to examine.</param>
        /// <returns>The candidate methods.</returns>
        protected virtual IEnumerable<MethodInfo> GetCandidateMethods( Type type )
        {
            MethodInfo[] methods = type.GetMethods( DefaultBindingFlags );

            return methods.Where( ShouldIntercept );
        }

        /// <summary>
        /// Determines whether the specified <see cref="MethodInfo"/> should be intercepted.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns>
        ///     <c>true</c> if the method should be intercepted; <c>false</c> otherwise.
        /// </returns>
        protected virtual bool ShouldIntercept( MethodInfo methodInfo )
        {
            return methodInfo.DeclaringType != typeof (object) &&
                   !methodInfo.IsPrivate;// &&
                   //!methodInfo.IsFinal;
        }
    }
}