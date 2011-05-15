namespace Ninject.Extensions.Interception
{
    using System.ServiceModel;
    using Castle.DynamicProxy;
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    using Xunit.Should;

    public class DynamicProxy2ChannelFactoryProxyTest : DynamicProxy2BaseTests
    {
#if !SILVERLIGHT
        [Fact]
        public void ProxiesCreatedWithChannelFactoryAreIntercepted()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<IFooService>().ToMethod(
                    context =>
                    ChannelFactory<IFooService>.CreateChannel(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost/FooService")));

                kernel.Intercept((request) => true).With<FlagInterceptor>();

                var obj = kernel.Get<IFooService>();

                obj.ShouldNotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).ShouldBeTrue();
            }
        }
#endif
    }
}
