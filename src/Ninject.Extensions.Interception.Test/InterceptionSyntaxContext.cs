namespace Ninject.Extensions.Interception
{
    using System;

    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    
    public abstract class InterceptionSyntaxContext : InterceptionTestContext
    {
        protected abstract Type ProxyType { get; }

        protected override StandardKernel CreateDefaultInterceptionKernel()
        {
            StandardKernel kernel = base.CreateDefaultInterceptionKernel();
            kernel.Bind<ViewModel>().ToSelf().Intercept().With<FlagInterceptor>();
            return kernel;
        }

        [Fact]
        public void Doo()
        {
            FlagInterceptor.Reset();
            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                var mock = kernel.Get<ViewModel>();
                mock.Address = "|ad";
                FlagInterceptor.WasCalled.Should().BeTrue();
            }
        }

        [Fact]
        public void CanAttachMultipleInterceptors()
        {
            FlagInterceptor.Reset();
            CountInterceptor.Reset();
            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                var binding = kernel.Bind<FooImpl>().ToSelf();
                binding.Intercept().With<FlagInterceptor>();
                binding.Intercept().With<CountInterceptor>();
                var foo = kernel.Get<FooImpl>();

                foo.Foo();

                FlagInterceptor.WasCalled.Should().BeTrue();
                CountInterceptor.Count.Should().Be(1);
            }
        }

        [Fact]
        public void SelfBoundTypesDeclaringMethodInterceptorsAreProxied()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();
                obj.Should().NotBeNull();
                this.ProxyType.IsAssignableFrom(obj.GetType()).Should().BeTrue();
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
                this.ProxyType.IsAssignableFrom(obj.GetType()).Should().BeTrue();

                CountInterceptor.Reset();

                obj.Foo();
                obj.Bar();

                CountInterceptor.Count.Should().Be(1);
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
                this.ProxyType.IsAssignableFrom(obj.GetType()).Should().BeTrue();
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
                this.ProxyType.IsAssignableFrom(obj.GetType()).Should().BeTrue();

                CountInterceptor.Reset();

                obj.Foo();
                obj.Bar();

                CountInterceptor.Count.Should().Be(1);
            }
        }

        [Fact]
        public void ServiceBoundTypesDeclaringInterceptorsOnGenericMethodsAreIntercepted()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<IGenericMethod>().To<ObjectWithGenericMethod>();
                var obj = kernel.Get<IGenericMethod>();

                obj.Should().NotBeNull();
                this.ProxyType.IsAssignableFrom(obj.GetType()).Should().BeTrue();

                FlagInterceptor.Reset();

                string result = obj.ConvertGeneric("", 42);

                result.Should().Be("42");
                FlagInterceptor.WasCalled.Should().BeTrue();
            }
        }

        [Fact]
        public void SingletonTests()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<RequestsConstructorInjection>().ToSelf().InSingletonScope();
                // This is just here to trigger proxying, but we won't intercept any calls
                kernel.Intercept(request => true).With<FlagInterceptor>();

                var obj = kernel.Get<RequestsConstructorInjection>();

                obj.Should().NotBeNull();
                this.ProxyType.IsAssignableFrom(obj.GetType()).Should().BeTrue();
                obj.Child.Should().NotBeNull();
            }
        }

        [Fact]
        public void SelfBoundTypesThatAreProxiedReceiveConstructorInjections()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<RequestsConstructorInjection>().ToSelf();
                // This is just here to trigger proxying, but we won't intercept any calls
                kernel.Intercept(request => true).With<FlagInterceptor>();

                var obj = kernel.Get<RequestsConstructorInjection>();

                obj.Should().NotBeNull();
                this.ProxyType.IsAssignableFrom(obj.GetType()).Should().BeTrue();
                obj.Child.Should().NotBeNull();
            }
        }
    }
}