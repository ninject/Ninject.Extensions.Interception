
namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Xunit;
    using Xunit.Should;

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