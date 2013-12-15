namespace Ninject.Extensions.Interception
{
    using FluentAssertions;
    using Ninject.Extensions.Interception.Fakes;
    using Xunit;

    public class DynamicProxy2ImplicitInterfaceTest : DynamicProxy2BaseTests
    {
        [Fact]
        public void InterceptedObjectShouldKeepImplementingOrigialInterfaces()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<IBase>().To<ImplicitDerived>();

                var obj = kernel.Get<IBase>();

                obj.Should().NotBeNull();
                obj.Should().BeAssignableTo<IDerived>();
            }
        }
    }
}
