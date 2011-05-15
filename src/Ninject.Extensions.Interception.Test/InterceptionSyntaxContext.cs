namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    using Xunit.Should;

    public abstract class InterceptionSyntaxContext<TInterceptionModule> : InterceptionTestContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
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
                FlagInterceptor.WasCalled.ShouldBeTrue();
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

                FlagInterceptor.WasCalled.ShouldBeTrue();
                CountInterceptor.Count.ShouldBe(1);
            }
        }
    }
}