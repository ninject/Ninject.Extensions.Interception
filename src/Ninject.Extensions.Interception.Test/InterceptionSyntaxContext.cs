namespace Ninject.Extensions.Interception
{
    using System;

    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    
    public abstract class InterceptionSyntaxContext<TInterceptionModule> : InterceptionTestContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
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

    }
}