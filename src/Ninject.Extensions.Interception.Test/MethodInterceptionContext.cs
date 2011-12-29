namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;

    using Xunit;
    
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
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                var mock = kernel.Get<Mock>();

                mock.MyProperty.Should().Be("start");
                mock.GetMyProperty().Should().Be("start");
            }

            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.InterceptReplace<Mock>(o => o.GetMyProperty(), i => i.ReturnValue = "intercepted");

                var mock = kernel.Get<Mock>();
                mock.MyProperty.Should().Be("start");
                mock.GetMyProperty().Should().Be("intercepted");
            }
        }

        [Fact]
        public void MethodInterceptedBefore()
        {
            string testString = "empty";

            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.InterceptBefore<Mock>(
                    o => o.SetMyProperty(string.Empty), 
                    i => testString = ((Mock)i.Request.Target).MyProperty);
                var mock = kernel.Get<Mock>();

                mock.MyProperty.Should().Be("start");
                testString.Should().Be("empty");

                mock.SetMyProperty("end");

                mock.MyProperty.Should().Be("end");
                testString.Should().Be("start");
            }
        }

        [Fact]
        public void MethodInterceptedAfter()
        {
            string testString = "empty";

            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.InterceptAfter<Mock>(
                    o => o.SetMyProperty(string.Empty), 
                    i => testString = ((Mock)i.Request.Target).MyProperty);

                var mock = kernel.Get<Mock>();

                mock.MyProperty.Should().Be("start");
                testString.Should().Be("empty");

                mock.SetMyProperty("end");

                mock.MyProperty.Should().Be("end");
                testString.Should().Be("end");
            }
        }

        [Fact]
        public void MethodCanBeInterceptedWithAddMethodInterceptor()
        {
            string testString = "empty";

            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.AddMethodInterceptor(typeof(Mock).GetMethod("SetMyProperty"),
                    i => testString = "intercepted");
                var mock = kernel.Get<Mock>();

                mock.SetMyProperty( "dummy" );

                mock.MyProperty.Should().Be("start");
                testString.Should().Be("intercepted");
            }
        }

        [Fact]
        public void MethodInterceptedWithAddMethodInterceptorCanBeResumed()
        {
            string testString = "empty";

            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.AddMethodInterceptor( typeof (Mock).GetMethod( "SetMyProperty" ),
                                             i => { 
                                                 testString = "intercepted";  
                                                 i.Proceed();
                                             } );
                var mock = kernel.Get<Mock>();

                mock.SetMyProperty("dummy");

                mock.MyProperty.Should().Be("dummy");
                testString.Should().Be("intercepted");
            }
        }

        [Fact]
        public void CanInterceptMethodsWithRefValue()
        {
            CountInterceptor.Reset();
            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                var binding = kernel.Bind<RefAndOutValues>().ToSelf();
                binding.Intercept().With<CountInterceptor>();
                var foo = kernel.Get<RefAndOutValues>();
                int x = 2;

                foo.Add(1, ref x, 2);

                CountInterceptor.Count.Should().Be(1);
                x.Should().Be(3);
            }
        }

        [Fact]
        public void CanInterceptMethodsWithOutValue()
        {
            CountInterceptor.Reset();
            using (StandardKernel kernel = this.CreateDefaultInterceptionKernel())
            {
                var binding = kernel.Bind<RefAndOutValues>().ToSelf();
                binding.Intercept().With<CountInterceptor>();
                var foo = kernel.Get<RefAndOutValues>();
                int x;

                foo.Multiply(2, out x, 3);

                CountInterceptor.Count.Should().Be(1);
                x.Should().Be(6);
            }
        }
    }
}