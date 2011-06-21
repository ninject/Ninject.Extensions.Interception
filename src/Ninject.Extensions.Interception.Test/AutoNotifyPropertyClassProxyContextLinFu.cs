#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using LinFu.DynamicProxy; 
    using Xunit;
    
    public class AutoNotifyPropertyClassProxyContextLinFu :
        AutoNotifyPropertyClassProxyContext<LinFuModule>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAClass_TheObjectIsProxied()
        {
            typeof(IProxy).IsAssignableFrom(ViewModel.GetType()).Should().BeTrue();
        }
    }
}
#endif
