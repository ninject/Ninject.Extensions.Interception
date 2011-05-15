namespace Ninject.Extensions.Interception
{
    using Castle.DynamicProxy;
    using Xunit;
    using Xunit.Should;

    public class AutoNotifyPropertyMethodInterceptorContextDynamicProxy2
        : AutoNotifyPropertyMethodInterceptorContext<DynamicProxy2Module>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAProperty_TheObjectIsProxied()
        {
            typeof(IProxyTargetAccessor).IsAssignableFrom(ViewModel.GetType()).ShouldBeTrue();
        }
    }
} 