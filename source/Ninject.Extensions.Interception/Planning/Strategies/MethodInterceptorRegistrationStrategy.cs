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
    public class MethodInterceptorRegistrationStrategy : InterceptorRegistrationStrategy
    {
        public MethodInterceptorRegistrationStrategy( IAdviceFactory adviceFactory,
                                                      IAdviceRegistry adviceRegistry,
                                                      IMethodInterceptorRegistry methodInterceptorRegistry )
            : base( adviceFactory, adviceRegistry )
        {
            MethodInterceptorRegistry = methodInterceptorRegistry;
        }

        public IMethodInterceptorRegistry MethodInterceptorRegistry { get; set; }

        #region Implementation of INinjectComponent

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