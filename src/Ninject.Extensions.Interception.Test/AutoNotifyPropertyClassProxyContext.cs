namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Xunit;
    
    public abstract class AutoNotifyPropertyClassProxyContext : AutoNotifyPropertyChangedContext
    {
        public AutoNotifyPropertyClassProxyContext()
        {
            this.ViewModel = this.Kernel.Get<ViewModelWithClassNotify>();
            this.ViewModel.PropertyChanged += (o, e) =>
                                           {
                                               this.LastPropertyToChange = e.PropertyName;
                                               this.PropertyChanges.Add(this.LastPropertyToChange);
                                           };
        }

        public ViewModelWithClassNotify ViewModel { get; set; }

        [Fact]
        public void WhenValueChangesOnPropertyWithoutNotifyAttribute_ItShouldNotifyChanges()
        {
            this.ViewModel.Address = "123 Main Street";
            this.LastPropertyToChange.Should().Be("Address");
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
        public void WhenPropertyHasDoNotNotifyAttribute_ChangNotificationIsSuppressed()
        {
            this.ViewModel.DoNotNotifyChanges = "...";
            this.LastPropertyToChange.Should().BeNull();
        }
    }
}