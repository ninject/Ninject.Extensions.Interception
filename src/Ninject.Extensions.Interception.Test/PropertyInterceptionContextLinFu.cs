#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Xunit;
    
    public class PropertyInterceptionContextLinfFu : PropertyInterceptionContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new LinFuModule();
            }
        }

        [Fact]
        public void PropertySetInterceptedBefore()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.InterceptBeforeSet<Mock>(
                    o => o.MyProperty, 
                    i => i.Request.Arguments[0] = "intercepted");
                var obj = kernel.Get<Mock>();

                obj.MyProperty.Should().Be("start");
 
                obj.MyProperty = "end";

                obj.MyProperty.Should().Be("intercepted");
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

                obj.MyProperty.Should().Be("start");

                obj.MyProperty = "end";

                obj.MyProperty.Should().Be("end");
                testString.Should().Be("end");
            }
        }
    }
}
#endif