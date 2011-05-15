namespace Ninject.Extensions.Interception
{
    using Castle.DynamicProxy;
    using Xunit;
    using Xunit.Should;

    public class AutoNotifyPropertyClassProxyContextDynamicProxy2 :
        AutoNotifyPropertyClassProxyContext<DynamicProxy2Module>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAClass_TheObjectIsProxied()
        {
            typeof(IProxyTargetAccessor).IsAssignableFrom(ViewModel.GetType()).ShouldBeTrue();
        }
    }
}