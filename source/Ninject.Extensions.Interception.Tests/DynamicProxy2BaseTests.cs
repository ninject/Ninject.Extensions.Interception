#region Using Directives

using System;
using System.Collections.Generic;
using Castle.Core.Interceptor;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Tests.Fakes;
using Ninject.Extensions.Interception.Tests.Interceptors;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class DynamicProxy2BaseTests : DynamicProxy2InterceptionContext
    {
        [Fact]
        public void SelfBoundTypesDeclaringMethodInterceptorsAreProxied()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();
                Assert.NotNull( obj );
                var baseTypes = new List<Type>();
                Type type = obj.GetType();
                while ( type != typeof (object) )
                {
                    baseTypes.Add( type );
                    Type[] interfaces = type.GetInterfaces();
                    baseTypes.AddRange( interfaces );
                    type = type.BaseType;
                }
                Assert.IsAssignableFrom<IProxyTargetAccessor>( obj );
            }
        }

        [Fact]
        public void SelfBoundTypesDeclaringMethodInterceptorsCanBeReleased()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();
                Assert.NotNull( obj );
                //kernel.Release(obj);
            }
        }

        [Fact]
        public void SelfBoundTypesDeclaringMethodInterceptorsAreIntercepted()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();
                Assert.NotNull( obj );
                Assert.IsAssignableFrom<IProxyTargetAccessor>( obj );

                CountInterceptor.Reset();

                obj.Foo();
                obj.Bar();

                Assert.Equal( 1, CountInterceptor.Count );
            }
        }

        [Fact]
        public void SelfBoundTypesDeclaringInterceptorsOnGenericMethodsAreIntercepted()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<ObjectWithGenericMethod>().ToSelf();
                var obj = kernel.Get<ObjectWithGenericMethod>();
                Assert.NotNull( obj );
                Assert.IsAssignableFrom<IProxyTargetAccessor>( obj );

                FlagInterceptor.Reset();
                string result = obj.ConvertGeneric( 42 );

                Assert.Equal( "42", result );
                Assert.True( FlagInterceptor.WasCalled );
            }
        }

        [Fact]
        public void ServiceBoundTypesDeclaringMethodInterceptorsAreProxied()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<IFoo>().To<ObjectWithMethodInterceptor>();
                var obj = kernel.Get<IFoo>();
                Assert.NotNull( obj );
                Assert.IsAssignableFrom<IProxyTargetAccessor>( obj );
            }
        }

        [Fact]
        public void ServiceBoundTypesDeclaringMethodInterceptorsCanBeReleased()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<IFoo>().To<ObjectWithMethodInterceptor>();
                var obj = kernel.Get<IFoo>();
                Assert.NotNull( obj );
                //kernel.Release(obj);
            }
        }

        [Fact]
        public void ServiceBoundTypesDeclaringMethodInterceptorsAreIntercepted()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<IFoo>().To<ObjectWithMethodInterceptor>();
                var obj = kernel.Get<IFoo>();
                Assert.NotNull( obj );
                Assert.IsAssignableFrom<IProxyTargetAccessor>( obj );

                CountInterceptor.Reset();

                obj.Foo();

                Assert.Equal( 1, CountInterceptor.Count );
            }
        }

        [Fact]
        public void ServiceBoundTypesDeclaringInterceptorsOnGenericMethodsAreIntercepted()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<IGenericMethod>().To<ObjectWithGenericMethod>();
                var obj = kernel.Get<IGenericMethod>();
                Assert.NotNull( obj );
                Assert.IsAssignableFrom<IProxyTargetAccessor>( obj );

                FlagInterceptor.Reset();

                string result = obj.ConvertGeneric( 42 );

                Assert.Equal( "42", result );
                Assert.True( FlagInterceptor.WasCalled );
            }
        }

        [Fact]
        public void SelfBoundTypesThatAreProxiedReceiveConstructorInjections()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<RequestsConstructorInjection>().ToSelf();
                // This is just here to trigger proxying, but we won't intercept any calls
                kernel.Intercept( ( request ) => false ).With<FlagInterceptor>();

                var obj = kernel.Get<RequestsConstructorInjection>();

                Assert.NotNull( obj );
                Assert.IsAssignableFrom<IProxyTargetAccessor>( obj );
                Assert.NotNull( obj.Child );
            }
        }

        [Fact]
        public void SingletonTests()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<RequestsConstructorInjection>().ToSelf().InSingletonScope();
                // This is just here to trigger proxying, but we won't intercept any calls
                kernel.Intercept( ( request ) => false ).With<FlagInterceptor>();

                var obj = kernel.Get<RequestsConstructorInjection>();

                Assert.NotNull( obj );
                Assert.IsAssignableFrom<IProxyTargetAccessor>( obj );
                Assert.NotNull( obj.Child );
            }
        }
    }
}