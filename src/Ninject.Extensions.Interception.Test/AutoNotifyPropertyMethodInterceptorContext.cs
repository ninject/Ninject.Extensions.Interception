namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Xunit;
    
    public abstract class AutoNotifyPropertyMethodInterceptorContext : AutoNotifyPropertyChangedContext
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

            LastPropertyToChange.Should().BeNull();
        }

        [Fact]
        public void WhenValueChangesOnPropertyWithDependentProperties_ItShouldNotifyAllChanges()
        {
            ViewModel.ZipCode = 9700;
            PropertyChanges[0].Should().Be("ZipCode");
            PropertyChanges[1].Should().Be("City");
            PropertyChanges[2].Should().Be("State");
        }

        [Fact]
        public void WhenPropertyGetterIsCalled_ItShouldNotBeIntercepted()
        {
            int zip = ViewModel.ZipCode;

            zip.Should().Be(0);
            LastPropertyToChange.Should().BeNull();
        }
    }
}