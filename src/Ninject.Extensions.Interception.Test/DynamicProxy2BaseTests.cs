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
        public void SelfBoundTypesDeclaringMethodInterceptorsAreProxied()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();
                obj.Should().NotBeNull();
                var baseTypes = new List<Type>();
                Type type = obj.GetType();
                while (type != typeof(object))
                {
                    baseTypes.Add(type);
                    Type[] interfaces = type.GetInterfaces();
                    baseTypes.AddRange(interfaces);
                    type = type.BaseType;
                }

                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();
            }
        }

        [Fact]
        public void SelfBoundTypesDeclaringMethodInterceptorsCanBeReleased()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();
                obj.Should().NotBeNull();
            }
        }

        [Fact]
        public void ClassesWithMultiplePropertiesWithTheSameNameCanBeInjected()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<SameNameProperty>().ToSelf();
                var obj = kernel.Get<SameNameProperty>();
                obj.Should().NotBeNull();
            }
        }

        [Fact]
        public void SelfBoundTypesDeclaringMethodInterceptorsAreIntercepted()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();
                obj.Should().NotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();

                CountInterceptor.Reset();

                obj.Foo();
                obj.Bar();

                CountInterceptor.Count.Should().Be(1);
            }
        }

        [Fact]
        public void SelfBoundTypesDeclaringInterceptorsOnGenericMethodsAreIntercepted()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<ObjectWithGenericMethod>().ToSelf();
                var obj = kernel.Get<ObjectWithGenericMethod>();

                FlagInterceptor.Reset();
                string result = obj.ConvertGeneric(42);

                result.Should().Be("42");
                FlagInterceptor.WasCalled.Should().BeTrue();
            }
        }

        [Fact]
        public void ServiceBoundTypesDeclaringMethodInterceptorsAreProxied()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<IFoo>().To<ObjectWithMethodInterceptor>();
                var obj = kernel.Get<IFoo>();

                obj.Should().NotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();
            }
        }

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

                string result = obj.ConvertGeneric(42);

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

        [Fact]
        public void NoneVirtualFunctionIntercepted_WhenResolveByInterface_ThenInterceptabe()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                CountInterceptor.Reset();

                kernel.Bind<IFoo>().To<NoneVirtualFooImplementation>().Intercept().With<CountInterceptor>();
                var obj = kernel.Get<IFoo>();
                obj.Foo();

                CountInterceptor.Count.Should().Be(1);
            }
        }

        [Fact]
        public void NoneVirtualFunctionIntercepted_WhenResolveByInterfaceAndInterceptionByContext_ThenInterceptabe()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                CountInterceptor.Reset();

                kernel.Bind<IFoo>().To<NoneVirtualFooImplementation>();
                kernel.Intercept(ctx => ctx.Request.Service == typeof(IFoo)).With<CountInterceptor>();
                var obj = kernel.Get<IFoo>();
                obj.Foo();

                CountInterceptor.Count.Should().Be(1);
            }
        }
    
        [Fact]
        public void NoneVirtualPropertyIntercepted_WhenResolveByInterface_ThenInterceptabe()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                CountInterceptor.Reset();

                const bool OriginalValue = true;
                kernel.Bind<IFoo>().To<NoneVirtualFooImplementation>();
                kernel.Intercept(ctx => ctx.Request.Service == typeof(IFoo)).With<CountInterceptor>();
                var obj = kernel.Get<IFoo>();

                obj.TestProperty = OriginalValue;
                var value = obj.TestProperty;

                CountInterceptor.Count.Should().Be(1);
                value.Should().Be(OriginalValue);
            }
        }
    }
}