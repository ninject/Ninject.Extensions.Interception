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
                                               LastPropertyToChange = e.PropertyName;
                                               PropertyChanges.Add(LastPropertyToChange);
                                           };
        }

        public ViewModelWithClassNotify ViewModel { get; set; }

        [Fact]
        public void WhenValueChangesOnPropertyWithoutNotifyAttribute_ItShouldNotifyChanges()
        {
            this.ViewModel.Address = "123 Main Street";
            LastPropertyToChange.Should().Be("Address");
        }

        [Fact]
        public void WhenValueChangesOnPropertyWithDependentProperties_ItShouldNotifyAllChanges()
        {
            this.ViewModel.ZipCode = 9700;
            PropertyChanges[0].Should().Be("ZipCode");
            PropertyChanges[1].Should().Be("City");
            PropertyChanges[2].Should().Be("State");
        }

        [Fact]
        public void WhenPropertyHasDoNotNotifyAttribute_ChangNotificationIsSuppressed()
        {
            this.ViewModel.DoNotNotifyChanges = "...";
            LastPropertyToChange.Should().BeNull();
        }
    }
}