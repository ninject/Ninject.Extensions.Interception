namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
    using MsTest.Should;
    using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven;
    using UnitDriven.Should;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Xunit;
    using Xunit.Should;
#endif

    public abstract class MethodInterceptionContext<TInterceptionModule> : InterceptionTestContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
        protected override StandardKernel CreateDefaultInterceptionKernel()
        {
            StandardKernel kernel = base.CreateDefaultInterceptionKernel();
            kernel.Bind<Mock>().ToSelf().WithConstructorArgument("myProperty", "start");
            return kernel;
        }

        [Fact]
        public void MethodInterceptedWithReplace()
        {
            
            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                var mock = kernel.Get<Mock>();

                mock.MyProperty.ShouldBe("start");
                mock.GetMyProperty().ShouldBe("start");
            }

            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                kernel.InterceptReplace<Mock>(o => o.GetMyProperty(), i => i.ReturnValue = "intercepted");

                var mock = kernel.Get<Mock>();
                mock.MyProperty.ShouldBe("start");
                mock.GetMyProperty().ShouldBe("intercepted");
            }
        }

        [Fact]
        public void MethodInterceptedBefore()
        {
            string testString = "empty";

            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                kernel.InterceptBefore<Mock>(
                    o => o.SetMyProperty(string.Empty), 
                    i => testString = ((Mock)i.Request.Target).MyProperty);
                var mock = kernel.Get<Mock>();

                mock.MyProperty.ShouldBe("start");
                testString.ShouldBe("empty");

                mock.SetMyProperty("end");

                mock.MyProperty.ShouldBe("end");
                testString.ShouldBe("start");
            }
        }

        [Fact]
        public void MethodInterceptedAfter()
        {
            string testString = "empty";

            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                kernel.InterceptAfter<Mock>(
                    o => o.SetMyProperty(string.Empty), 
                    i => testString = ((Mock)i.Request.Target).MyProperty);

                var mock = kernel.Get<Mock>();

                mock.MyProperty.ShouldBe("start");
                testString.ShouldBe("empty");

                mock.SetMyProperty("end");

                mock.MyProperty.ShouldBe("end");
                testString.ShouldBe("end");
            }
        }
    }
}