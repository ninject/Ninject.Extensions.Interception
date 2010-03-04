#region Using Directives

using Castle.Core.Interceptor;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class AutoNotifyPropertyClassProxyContextDynamicProxy2 :
        AutoNotifyPropertyClassProxyContext<DynamicProxy2Module>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAClass_TheObjectIsProxied()
        {
            Assert.IsAssignableFrom<IProxyTargetAccessor>( ViewModel );
        }
    }
}