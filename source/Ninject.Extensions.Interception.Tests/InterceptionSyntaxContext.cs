#region Using Directives

using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Tests.Fakes;
using Ninject.Extensions.Interception.Tests.Interceptors;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
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
            Assert.False( FlagInterceptor.WasCalled );
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                var mock = kernel.Get<ViewModel>();
                mock.Address = "|ad";
                Assert.True( FlagInterceptor.WasCalled );
            }
        }
    }
}