
namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Xunit;
    
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

                obj.Should().NotBeNull();
                obj.MyProperty = "end";
                obj.MyProperty.Should().Be("intercepted");
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

                obj.MyProperty.Should().Be("start");

                obj.MyProperty = "end";

                obj.MyProperty.Should().Be("end");
                testString.Should().Be("end");
            }
        }
    }
}