#region Using Directives

using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class PropertyInterceptionContextLinfFu : PropertyInterceptionContext<LinFuModule>
    {
        [Fact]
        public void PropertySetInterceptedBefore()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptBeforeSet<Mock>( o => o.MyProperty,
                                                 i => i.Request.Arguments[0] = "intercepted" );
                var obj = kernel.Get<Mock>();

                Assert.NotNull( obj );
                Assert.Equal( "start", obj.MyProperty );

                obj.MyProperty = "end";

                Assert.Equal( "intercepted", obj.MyProperty );
            }
        }

        [Fact]
        public void PropertySetInterceptedAfter()
        {
            string testString = "empty";

            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptAfterSet<Mock>( o => o.MyProperty,
                                                i => testString = ( (Mock) i.Request.Target ).MyProperty );
                var obj = kernel.Get<Mock>();

                Assert.NotNull( obj );

                Assert.Equal( "empty", testString );
                Assert.Equal( "start", obj.MyProperty );

                obj.MyProperty = "end";

                Assert.Equal( "end", obj.MyProperty );
                Assert.Equal( "end", testString );
            }
        }
    }
}