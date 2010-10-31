namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes; 
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
    using MsTest.Should;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven;
    using UnitDriven.Should;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Ninject.Extensions.Interception.MSTestAttributes;
    using Xunit;
    using Xunit.Should;
#endif

    public abstract class AutoNotifyPropertyMethodInterceptorContext<TInterceptionModule>
        : AutoNotifyPropertyChangedContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
        public AutoNotifyPropertyMethodInterceptorContext()
        {
            this.SetUp();
        }

        [TestInitialize]
        public virtual void SetUp()
        {
            LastPropertyToChange = null;
            ViewModel = Kernel.Get<ViewModel>();
            ViewModel.PropertyChanged += ( o, e ) =>
                {
                    LastPropertyToChange = e.PropertyName;
                    PropertyChanges.Add( LastPropertyToChange );
                };
        }

        public ViewModel ViewModel { get; set; }

        [Fact]
        public void WhenValueChangesOnPropertyWithoutNotifyAttribute_ItShouldNotNotify()
        {
            ViewModel.Address = "123 Main Street";

            LastPropertyToChange.ShouldBeNull();
        }

        [Fact]
        public void WhenValueChangesOnPropertyWithDependentProperties_ItShouldNotifyAllChanges()
        {
            ViewModel.ZipCode = 9700;
            PropertyChanges[0].ShouldBe("ZipCode");
            PropertyChanges[1].ShouldBe("City");
            PropertyChanges[2].ShouldBe("State");
        }

        [Fact]
        public void WhenPropertyGetterIsCalled_ItShouldNotBeIntercepted()
        {
            int zip = ViewModel.ZipCode;

            zip.ShouldBe(0);
            LastPropertyToChange.ShouldBeNull();
        }
    }
}