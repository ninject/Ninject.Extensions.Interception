namespace Ninject.Extensions.Interception
{
    using Castle.DynamicProxy;

    using FluentAssertions;

    using Xunit;
    
    public class AutoNotifyPropertyClassProxyContextDynamicProxy2 : AutoNotifyPropertyClassProxyContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new DynamicProxyModule();
            }
        }

        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAClass_TheObjectIsProxied()
        {
            typeof(IProxyTargetAccessor).IsAssignableFrom(this.ViewModel.GetType()).Should().BeTrue();
        }
    }
}