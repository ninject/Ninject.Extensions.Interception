namespace Ninject.Extensions.Interception.Tests
{
    using System.ServiceModel;
    using Castle.Core.Interceptor;
    using Fakes;
    using Infrastructure.Language;
    using Interceptors;
    using Xunit;

    public class DynamicProxy2ChannelFactoryProxyTest : DynamicProxy2BaseTests
    {
        [Fact]
        public void ProxiesCreatedWithChannelFactoryAreIntercepted()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<IFooService>().ToMethod(
                    context =>
                    ChannelFactory<IFooService>.CreateChannel(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost/FooService")));

                kernel.Intercept((request) => false).With<FlagInterceptor>();

                var obj = kernel.Get<IFooService>();

                Assert.NotNull(obj);
                Assert.IsAssignableFrom<IProxyTargetAccessor>(obj);
            }
        }
    }
}