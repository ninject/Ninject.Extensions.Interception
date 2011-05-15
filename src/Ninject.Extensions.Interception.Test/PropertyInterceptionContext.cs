namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Xunit;
    using Xunit.Should;

    public abstract class PropertyInterceptionContext<TInterceptionModule> :
        InterceptionTestContext<TInterceptionModule> where TInterceptionModule : InterceptionModule, new()
    {
        protected override StandardKernel CreateDefaultInterceptionKernel()
        {
            StandardKernel kernel = base.CreateDefaultInterceptionKernel();
            kernel.Bind<Mock>().ToSelf().WithConstructorArgument("myProperty", "start");
            return kernel;
        }

        [Fact]
        public void PropertyGetInterceptedWithReplace()
        {
            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                var obj = kernel.Get<Mock>();

                obj.GetMyProperty().ShouldBe("start");
                obj.MyProperty.ShouldBe("start");
            }

            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                kernel.InterceptReplaceGet<Mock>(
                    o => o.MyProperty, 
                    i => i.ReturnValue = "intercepted");
                var obj = kernel.Get<Mock>();

                obj.GetMyProperty().ShouldBe("start");
                obj.MyProperty.ShouldBe("intercepted");
            }
        }

        [Fact]
        public void PropertySetInterceptedWithReplace()
        {
            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                var obj = kernel.Get<Mock>();

                obj.MyProperty.ShouldBe("start");

                obj.MyProperty = "end";
                
                obj.MyProperty.ShouldBe("end");
            }

            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                kernel.InterceptReplaceSet<Mock>(o => o.MyProperty, i => { });

                var obj = kernel.Get<Mock>();

                obj.MyProperty.ShouldBe("start");

                obj.MyProperty = "end";

                obj.MyProperty.ShouldBe("start");
            }
        }

        [Fact]
        public void PropertyGetInterceptedBefore()
        {
            string testString = "empty";

            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.InterceptBeforeGet<Mock>(
                    o => o.MyProperty,
                    i =>
                        {
                            if (i.ReturnValue == null)
                            {
                                testString = "null";
                            }
                        });

                var obj = kernel.Get<Mock>();

                testString.ShouldBe("empty");
                obj.MyProperty.ShouldBe("start");
                testString.ShouldBe("null");
            }
        }

        [Fact]
        public void PropertyGetInterceptedAfter()
        {
            string testString = "empty";

            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                kernel.InterceptAfterGet<Mock>(
                    o => o.MyProperty, 
                    i => testString = i.ReturnValue.ToString());
                var obj = kernel.Get<Mock>();

                testString.ShouldBe("empty");
                obj.MyProperty.ShouldBe("start");
                testString.ShouldBe("start");
            }
        }
    }
}