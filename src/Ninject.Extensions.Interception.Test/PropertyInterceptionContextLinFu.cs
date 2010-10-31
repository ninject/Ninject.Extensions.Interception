#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Xunit;
    using Xunit.Should;

    public class PropertyInterceptionContextLinfFu : PropertyInterceptionContext<LinFuModule>
    {
        [Fact]
        public void PropertySetInterceptedBefore()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.InterceptBeforeSet<Mock>(
                    o => o.MyProperty, 
                    i => i.Request.Arguments[0] = "intercepted");
                var obj = kernel.Get<Mock>();

                obj.MyProperty.ShouldBe("start");
 
                obj.MyProperty = "end";

                obj.MyProperty.ShouldBe("intercepted");
            }
        }

        [Fact]
        public void PropertySetInterceptedAfter()
        {
            string testString = "empty";

            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
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
#endif