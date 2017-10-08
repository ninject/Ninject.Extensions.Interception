namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Xunit;
    
    public abstract class AutoNotifyPropertyMethodInterceptorContext : AutoNotifyPropertyChangedContext
    {
        public AutoNotifyPropertyMethodInterceptorContext()
        {
            this.LastPropertyToChange = null;
            this.ViewModel = this.Kernel.Get<ViewModel>();
            this.ViewModel.PropertyChanged += ( o, e ) =>
                {
                    this.LastPropertyToChange = e.PropertyName;
                    this.PropertyChanges.Add(this.LastPropertyToChange );
                };
        }

        public ViewModel ViewModel { get; set; }

        [Fact]
        public void WhenValueChangesOnPropertyWithoutNotifyAttribute_ItShouldNotNotify()
        {
            this.ViewModel.Address = "123 Main Street";

            this.LastPropertyToChange.Should().BeNull();
        }

        [Fact]
        public void WhenValueChangesOnPropertyWithDependentProperties_ItShouldNotifyAllChanges()
        {
            this.ViewModel.ZipCode = 9700;
            this.PropertyChanges[0].Should().Be("ZipCode");
            this.PropertyChanges[1].Should().Be("City");
            this.PropertyChanges[2].Should().Be("State");
        }

        [Fact]
        public void WhenPropertyGetterIsCalled_ItShouldNotBeIntercepted()
        {
            int zip = this.ViewModel.ZipCode;

            zip.Should().Be(0);
            this.LastPropertyToChange.Should().BeNull();
        }
    }
}