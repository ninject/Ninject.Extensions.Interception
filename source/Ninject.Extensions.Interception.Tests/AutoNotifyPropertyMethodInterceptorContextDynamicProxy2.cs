#region Using Directives

using Castle.Core.Interceptor;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class AutoNotifyPropertyMethodInterceptorContextDynamicProxy2
        : AutoNotifyPropertyMethodInterceptorContext<DynamicProxy2Module>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAProperty_TheObjectIsProxied()
        {
            Assert.IsAssignableFrom<IProxyTargetAccessor>( ViewModel );
        }
    }
}