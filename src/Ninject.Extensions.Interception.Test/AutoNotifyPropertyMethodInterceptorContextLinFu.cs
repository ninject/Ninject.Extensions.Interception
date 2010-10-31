#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using LinFu.DynamicProxy;
    using Xunit;
    using Xunit.Should;

    public class AutoNotifyPropertyMethodInterceptorContextLinFu
        : AutoNotifyPropertyMethodInterceptorContext<LinFuModule>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAProperty_TheObjectIsProxied()
        {
            typeof(IProxy).IsAssignableFrom(ViewModel.GetType()).ShouldBeTrue();
        }
    }
} 
#endif
