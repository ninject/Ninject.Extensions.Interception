namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Xunit;
    using Xunit.Should;

    public abstract class AutoNotifyPropertyMethodInterceptorContext<TInterceptionModule>
        : AutoNotifyPropertyChangedContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
        public AutoNotifyPropertyMethodInterceptorContext()
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