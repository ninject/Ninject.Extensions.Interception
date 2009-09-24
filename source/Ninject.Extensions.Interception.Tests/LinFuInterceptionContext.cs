#region Using Directives

using LinFu.DynamicProxy;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Tests.Fakes;
using Ninject.Extensions.Interception.Tests.Interceptors;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class LinFuInterceptionContext : InterceptionTestContext
    {
        [Fact]
        public void SelfBoundTypesDeclaringMethodInterceptorsAreProxied()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();
                Assert.NotNull( obj );
                Assert.IsAssignableFrom<IProxy>( obj );
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
                Assert.IsAssignableFrom<IProxy>( obj );

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
                Assert.IsAssignableFrom<IProxy>( obj );

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
                Assert.IsAssignableFrom<IProxy>( obj );
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
                Assert.IsAssignableFrom<IProxy>( obj );

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
                Assert.IsAssignableFrom<IProxy>( obj );

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
                Assert.IsAssignableFrom<IProxy>( obj );
                Assert.NotNull( obj.Child );
            }
        }
    }
}