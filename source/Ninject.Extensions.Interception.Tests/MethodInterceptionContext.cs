#region Using Directives

using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class MethodInterceptionContext : InterceptionTestContext
    {
        protected override StandardKernel CreateDefaultInterceptionKernel()
        {
            var kernel = base.CreateDefaultInterceptionKernel();
            kernel.Bind<Mock>().ToSelf().WithConstructorArgument( "myProperty", "start" );
            return kernel;
        }

        [Fact]
        public void MethodInterceptedWithReplace()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                var mock = kernel.Get<Mock>();
                Assert.NotNull( mock );
                Assert.Equal( "start", mock.MyProperty );
                Assert.Equal( "start", mock.GetMyProperty() );
            }


            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptReplace<Mock>( o => o.GetMyProperty(),
                                               i => i.ReturnValue = "intercepted" );

                var mock = kernel.Get<Mock>();
                Assert.NotNull( mock );
                Assert.Equal( "start", mock.MyProperty );
                Assert.Equal( "intercepted", mock.GetMyProperty() );
            }
        }

        [Fact]
        public void MethodInterceptedBefore()
        {
            string testString = "empty";

            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptBefore<Mock>( o => o.SetMyProperty( "" ),
                                              i => testString = ( (Mock) i.Request.Target ).MyProperty );
                var mock = kernel.Get<Mock>();

                Assert.NotNull( mock );
                Assert.Equal( "start", mock.MyProperty );
                Assert.Equal( "empty", testString );

                mock.SetMyProperty( "end" );

                Assert.Equal( "end", mock.MyProperty );
                Assert.Equal( "start", testString );
            }
        }

        [Fact]
        public void MethodInterceptedAfter()
        {
            string testString = "empty";

            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptAfter<Mock>( o => o.SetMyProperty( "" ),
                                             i => testString = ( (Mock) i.Request.Target ).MyProperty );

                var mock = kernel.Get<Mock>();

                Assert.NotNull( mock );
                Assert.Equal( "start", mock.MyProperty );
                Assert.Equal( "empty", testString );

                mock.SetMyProperty( "end" );

                Assert.Equal( "end", mock.MyProperty );
                Assert.Equal( "end", testString );
            }
        }
    }
}