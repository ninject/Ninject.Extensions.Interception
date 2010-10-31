#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using LinFu.DynamicProxy; 
    using Xunit;
    using Xunit.Should;

    public class AutoNotifyPropertyClassProxyContextLinFu :
        AutoNotifyPropertyClassProxyContext<LinFuModule>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAClass_TheObjectIsProxied()
        {
            typeof(IProxy).IsAssignableFrom(ViewModel.GetType()).ShouldBeTrue();
        }
    }
}
#endif
