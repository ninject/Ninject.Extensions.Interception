namespace Ninject.Extensions.Interception
{
    using FluentAssertions;
    using Ninject.Extensions.Interception.Fakes;
    using Xunit;

    public abstract class ImplicitInterfaceContext : InterceptionTestContext
    {
        [Fact]
        public void InterceptedInterfaceObjectShouldKeepImplementingImplicitInterfaces()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<IBase>().To<ImplicitDerived>();

                var obj = kernel.Get<IBase>();

                obj.Should().NotBeNull();
                obj.Should().BeAssignableTo<IDerived>();
            }
        }

        [Fact]
        public virtual void InterceptedClassObjectShouldKeepImplementingImplicitInterfaces()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<Base>().To<ImplicitDerived>();

                var obj = kernel.Get<Base>();

                obj.Should().NotBeNull();
                obj.Should().BeAssignableTo<IDerived>();
            }
        }

        [Fact]
        public void InterceptedVirutalClassObjectShouldKeepImplementingImplicitInterfaces()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<VirtualBase>().To<ImplicitDerivedFromVirtualBase>();

                var obj = kernel.Get<VirtualBase>();

                obj.Should().NotBeNull();
                obj.Should().BeAssignableTo<IDerived>();
            }
        }
    }
}
