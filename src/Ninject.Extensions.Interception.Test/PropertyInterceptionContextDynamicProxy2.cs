
namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;

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
    public class PropertyInterceptionContextDynamicProxy2 : PropertyInterceptionContext<DynamicProxy2Module>
    {
        [Fact]
        public void PropertySetInterceptedBefore()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.InterceptBeforeSet<Mock>(
                    o => o.MyProperty, 
                    i => i.Request.Arguments[0] = "intercepted");
                var obj = kernel.Get<Mock>();

                obj.ShouldNotBeNull();
                obj.MyProperty = "end";
                obj.MyProperty.ShouldBe("intercepted");
            }
        }

        [Fact]
        public void PropertySetInterceptedAfter()
        {
            string testString = "empty";

            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.InterceptAfterSet<Mock>(
                    o => o.MyProperty, 
                    i => testString = ((Mock)i.Request.Target).MyProperty);
                var obj = kernel.Get<Mock>();

                obj.MyProperty.ShouldBe("start");

                obj.MyProperty = "end";

                obj.MyProperty.ShouldBe("end");
                testString.ShouldBe("end");
            }
        }
    }
}