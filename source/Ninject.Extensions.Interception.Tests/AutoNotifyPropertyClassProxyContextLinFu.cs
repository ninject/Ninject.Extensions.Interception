#region Using Directives

using LinFu.DynamicProxy;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class AutoNotifyPropertyClassProxyContextLinFu :
        AutoNotifyPropertyClassProxyContext<LinFuModule>
    {
        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAClass_TheObjectIsProxied()
        {
            Assert.IsAssignableFrom<IProxy>( ViewModel );
        }
    }
}