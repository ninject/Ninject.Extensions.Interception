#if !NETCOREAPP2_0
namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using LinFu.DynamicProxy; 
    using Xunit;
    
    public class AutoNotifyPropertyClassProxyContextLinFu : AutoNotifyPropertyClassProxyContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new LinFuModule();
            }
        }

        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAClass_TheObjectIsProxied()
        {
            typeof(IProxy).IsAssignableFrom(this.ViewModel.GetType()).Should().BeTrue();
        }
    }
}
#endif
