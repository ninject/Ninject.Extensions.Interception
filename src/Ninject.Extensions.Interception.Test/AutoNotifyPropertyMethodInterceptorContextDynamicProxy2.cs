namespace Ninject.Extensions.Interception
{
    using Castle.DynamicProxy;

    using FluentAssertions;

    using Xunit;
    
    public class AutoNotifyPropertyMethodInterceptorContextDynamicProxy2 : AutoNotifyPropertyMethodInterceptorContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new DynamicProxyModule();
            }
        }


        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAProperty_TheObjectIsProxied()
        {
            typeof(IProxyTargetAccessor).IsAssignableFrom(ViewModel.GetType()).Should().BeTrue();
        }
    }
} 