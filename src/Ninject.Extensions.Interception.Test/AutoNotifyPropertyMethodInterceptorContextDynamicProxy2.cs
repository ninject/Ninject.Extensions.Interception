namespace Ninject.Extensions.Interception
{
    using Castle.DynamicProxy;

    using FluentAssertions;

    using Xunit;
    
    public class AutoNotifyPropertyMethodInterceptorContextDynamicProxy2
        : AutoNotifyPropertyMethodInterceptorContext<DynamicProxy2Module>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAProperty_TheObjectIsProxied()
        {
            typeof(IProxyTargetAccessor).IsAssignableFrom(ViewModel.GetType()).Should().BeTrue();
        }
    }
} 