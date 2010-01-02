#region Using Directives

using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public class PropertyInterceptionContext : InterceptionTestContext
    {
        protected override StandardKernel CreateDefaultInterceptionKernel()
        {
            var kernel = new StandardKernel( GetSettings(), new InterceptionModule() );
            kernel.Bind<Mock>().ToSelf().WithConstructorArgument( "myProperty", "start" );
            return kernel;
        }

        [Fact]
        public void PropertyGetInterceptedWithReplace()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                var obj = kernel.Get<Mock>();

                Assert.NotNull( obj );
                Assert.Equal( "start", obj.GetMyProperty() );
                Assert.Equal( "start", obj.MyProperty );
            }

            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptReplaceGet<Mock>( o => o.MyProperty,
                                                  i => i.ReturnValue = "intercepted" );
                var obj = kernel.Get<Mock>();

                Assert.NotNull( obj );
                Assert.Equal( "start", obj.GetMyProperty() );
                Assert.Equal( "intercepted", obj.MyProperty );
            }
        }

        [Fact]
        public void PropertySetInterceptedWithReplace()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                var obj = kernel.Get<Mock>();

                Assert.NotNull( obj );
                Assert.Equal( "start", obj.MyProperty );

                obj.MyProperty = "end";

                Assert.Equal( "end", obj.MyProperty );
            }

            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptReplaceSet<Mock>( o => o.MyProperty, i => { } );

                var obj = kernel.Get<Mock>();

                Assert.NotNull( obj );
                Assert.Equal( "start", obj.MyProperty );

                obj.MyProperty = "end";

                Assert.Equal( "start", obj.MyProperty );
            }
        }

        [Fact]
        public void PropertyGetInterceptedBefore()
        {
            string testString = "empty";

            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptBeforeGet<Mock>( o => o.MyProperty,
                                                 i =>
                                                 {
                                                     if ( i.ReturnValue == null )
                                                     {
                                                         testString = "null";
                                                     }
                                                 } );
                var obj = kernel.Get<Mock>();

                Assert.NotNull( obj );
                Assert.Equal( "empty", testString );
                Assert.Equal( "start", obj.MyProperty );
                Assert.Equal( "null", testString );
            }
        }

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
        public void PropertyGetInterceptedAfter()
        {
            string testString = "empty";


            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptAfterGet<Mock>( o => o.MyProperty,
                                                i => testString = i.ReturnValue.ToString() );
                var obj = kernel.Get<Mock>();

                Assert.NotNull( obj );
                Assert.Equal( "empty", testString );
                Assert.Equal( "start", obj.MyProperty );
                Assert.Equal( "start", testString );
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