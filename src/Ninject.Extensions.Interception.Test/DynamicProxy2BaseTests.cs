namespace Ninject.Extensions.Interception
{
    using System;
    using System.Collections.Generic;
    using Castle.DynamicProxy;

    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    
    public class DynamicProxy2BaseTests : DynamicProxy2InterceptionContext
    {
        [Fact]
        public void ServiceBoundTypesDeclaringMethodInterceptorsAreIntercepted()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<IFoo>().To<ObjectWithMethodInterceptor>();
                var obj = kernel.Get<IFoo>();

                obj.Should().NotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();

                CountInterceptor.Reset();

                obj.Foo();
                obj.Bar();

                CountInterceptor.Count.Should().Be(1);
            }
        }

        [Fact]
        public void ServiceBoundTypesDeclaringInterceptorsOnGenericMethodsAreIntercepted()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<IGenericMethod>().To<ObjectWithGenericMethod>();
                var obj = kernel.Get<IGenericMethod>();

                obj.Should().NotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();

                FlagInterceptor.Reset();

                string result = obj.ConvertGeneric("", 42);

                result.Should().Be("42");
                FlagInterceptor.WasCalled.Should().BeTrue();
            }
        }

        [Fact]
        public void SelfBoundTypesThatAreProxiedReceiveConstructorInjections()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<RequestsConstructorInjection>().ToSelf();
                // This is just here to trigger proxying, but we won't intercept any calls
                kernel.Intercept(request => true).With<FlagInterceptor>();

                var obj = kernel.Get<RequestsConstructorInjection>();

                obj.Should().NotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();
                obj.Child.Should().NotBeNull();
            }
        }

        [Fact]
        public void SingletonTests()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<RequestsConstructorInjection>().ToSelf().InSingletonScope();
                // This is just here to trigger proxying, but we won't intercept any calls
                kernel.Intercept((request) => true).With<FlagInterceptor>();

                var obj = kernel.Get<RequestsConstructorInjection>();

                obj.Should().NotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();
                obj.Child.Should().NotBeNull();
            }
        }
    }
}