namespace Ninject.Extensions.Interception
{
    using FluentAssertions;
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Xunit;

    public abstract class ImplicitInterfaceContext : InterceptionTestContext
    {
        [Fact]
        public void InterceptedInterfaceObjectShouldKeepImplementingImplicitInterfaces()
        {
            using (StandardKernel kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<IBase>().To<ImplicitDerived>().Intercept(typeof(IDerived));

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
                kernel.Bind<Base>().To<ImplicitDerived>().Intercept(typeof(IDerived));

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
                kernel.Bind<VirtualBase>().To<ImplicitDerivedFromVirtualBase>().Intercept(typeof(IDerived));

                var obj = kernel.Get<VirtualBase>();

                obj.Should().NotBeNull();
                obj.Should().BeAssignableTo<IDerived>();
            }
        }
    }
}
