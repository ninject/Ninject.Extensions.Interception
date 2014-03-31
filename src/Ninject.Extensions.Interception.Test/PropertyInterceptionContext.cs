namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;

    using Xunit;
    
    public abstract class PropertyInterceptionContext : InterceptionTestContext
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

                obj.GetMyProperty().Should().Be("start");
                obj.MyProperty.Should().Be("start");
            }

            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                kernel.InterceptReplaceGet<Mock>(
                    o => o.MyProperty, 
                    i => i.ReturnValue = "intercepted");
                var obj = kernel.Get<Mock>();

                obj.GetMyProperty().Should().Be("start");
                obj.MyProperty.Should().Be("intercepted");
            }
        }

        [Fact]
        public void PropertySetInterceptedWithReplace()
        {
            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                var obj = kernel.Get<Mock>();

                obj.MyProperty.Should().Be("start");

                obj.MyProperty = "end";
                
                obj.MyProperty.Should().Be("end");
            }

            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                kernel.InterceptReplaceSet<Mock>(o => o.MyProperty, i => { });

                var obj = kernel.Get<Mock>();

                obj.MyProperty.Should().Be("start");

                obj.MyProperty = "end";

                obj.MyProperty.Should().Be("start");
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

                testString.Should().Be("empty");
                obj.MyProperty.Should().Be("start");
                testString.Should().Be("null");
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

                testString.Should().Be("empty");
                obj.MyProperty.Should().Be("start");
                testString.Should().Be("start");
            }
        }


        [Fact]
        public void NoneVirtualPropertyIntercepted_WhenResolveByInterface_ThenInterceptabe()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                CountInterceptor.Reset();

                const bool OriginalValue = true;
                kernel.Bind<IFoo>().To<NoneVirtualFooImplementation>();
                kernel.Intercept(ctx => ctx.Request.Service == typeof(IFoo)).With<CountInterceptor>();
                var obj = kernel.Get<IFoo>();

                obj.TestProperty = OriginalValue;
                var value = obj.TestProperty;

                CountInterceptor.Count.Should().Be(2);
                value.Should().Be(OriginalValue);
            }
        }

        [Fact]
        public void SelfBoundTypesDeclaringPropertyInterceptorsAreIntercepted()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<ObjectWithMethodInterceptor>().ToSelf();
                var obj = kernel.Get<ObjectWithMethodInterceptor>();

                CountInterceptor.Reset();

                var value = obj.TestProperty;
                obj.TestProperty = value;
                CountInterceptor.Count.Should().Be(0);

                var value2 = obj.TestProperty2;
                obj.TestProperty2 = value2;
                CountInterceptor.Count.Should().Be(2);
            }
        }

        [Fact]
        public void ClassesWithMultiplePropertiesWithTheSameNameCanBeInjected()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<SameNameProperty>().ToSelf();
                var obj = kernel.Get<SameNameProperty>();
                obj.Should().NotBeNull();
            }
        }
    }
}