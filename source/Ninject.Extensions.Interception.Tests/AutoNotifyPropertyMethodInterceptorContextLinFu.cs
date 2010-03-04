#region Using Directives

using LinFu.DynamicProxy;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class AutoNotifyPropertyMethodInterceptorContextLinFu
        : AutoNotifyPropertyMethodInterceptorContext<LinFuModule>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAProperty_TheObjectIsProxied()
        {
            Assert.IsAssignableFrom<IProxy>( ViewModel );
        }
    }
}