#region License

// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System.Collections.Generic;
using System.Reflection;
using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Planning.Directives;
using Ninject.Extensions.Interception.Registry;
using Ninject.Planning;

#endregion

namespace Ninject.Extensions.Interception.Planning.Strategies
{
    /// <summary>
    /// Provides interceptor attachment for methods configured by the kernel for method level interception.
    /// </summary>
    public class MethodInterceptorRegistrationStrategy : InterceptorRegistrationStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInterceptorRegistrationStrategy"/> class.
        /// </summary>
        /// <param name="adviceFactory">The advice factory.</param>
        /// <param name="adviceRegistry">The advice registry.</param>
        /// <param name="methodInterceptorRegistry">The method interceptor registry.</param>
        public MethodInterceptorRegistrationStrategy( IAdviceFactory adviceFactory,
                                                      IAdviceRegistry adviceRegistry,
                                                      IMethodInterceptorRegistry methodInterceptorRegistry )
            : base( adviceFactory, adviceRegistry )
        {
            MethodInterceptorRegistry = methodInterceptorRegistry;
        }

        /// <summary>
        /// Gets or sets the method interceptor registry.
        /// </summary>
        /// <value>The method interceptor registry.</value>
        public IMethodInterceptorRegistry MethodInterceptorRegistry { get; set; }

        #region Implementation of INinjectComponent

        /// <summary>
        /// Contributes to the specified plan.
        /// </summary>
        /// <param name="plan">The plan that is being generated.</param>
        public override void Execute( IPlan plan )
        {
            if ( !MethodInterceptorRegistry.Contains( plan.Type ) )
            {
                return;
            }

            MethodInterceptorCollection methodInterceptors = MethodInterceptorRegistry.GetMethodInterceptors( plan.Type );

            Dictionary<MethodInfo, List<IInterceptor>>.KeyCollection methods = methodInterceptors.Keys;
            if ( methods.Count == 0 )
            {
                return;
            }

            foreach ( MethodInfo method in methods )
            {
                for ( int order = 0; order < methodInterceptors[method].Count; order++ )
                {
                    IInterceptor interceptor = methodInterceptors[method][order];
                    RegisterMethodInterceptors( plan.Type,
                                                method,
                                                new[]
                                                {
                                                    new InternalInterceptAttribute( request => interceptor )
                                                    { Order = order }
                                                } );
                }
            }

            if ( !plan.Has<ProxyDirective>() )
            {
                plan.Add( new ProxyDirective() );
            }
        }

        #endregion
    }
}