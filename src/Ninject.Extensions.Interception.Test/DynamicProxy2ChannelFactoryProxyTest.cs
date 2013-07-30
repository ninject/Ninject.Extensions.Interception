namespace Ninject.Extensions.Interception
{
#if !SILVERLIGHT
    using System.ServiceModel;
#endif
    using Castle.DynamicProxy;

    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    
    public class DynamicProxy2ChannelFactoryProxyTest : DynamicProxy2BaseTests
    {
#if !SILVERLIGHT && !MONO
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

                obj.Should().NotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();
            }
        }
#endif
    }
}
