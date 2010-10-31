namespace Ninject.Extensions.Interception
{
    using Castle.DynamicProxy;
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
    using MsTest.Should;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven;
    using UnitDriven.Should;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Ninject.Extensions.Interception.MSTestAttributes;
    using Xunit;
    using Xunit.Should;
#endif

    [TestClass]
    public class AutoNotifyPropertyMethodInterceptorContextDynamicProxy2
        : AutoNotifyPropertyMethodInterceptorContext<DynamicProxy2Module>
    {
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
        }


        [Fact]
        public void WhenAutoNotifyAttributeIsAttachedToAProperty_TheObjectIsProxied()
        {
            typeof(IProxyTargetAccessor).IsAssignableFrom(ViewModel.GetType()).ShouldBeTrue();
        }
    }
} 