namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Xunit;
    using Xunit.Should;

    public abstract class AutoNotifyPropertyClassProxyContext<TInterceptionModule>
        : AutoNotifyPropertyChangedContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
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
            LastPropertyToChange.ShouldBe("Address");
        }

        [Fact]
        public void WhenValueChangesOnPropertyWithDependentProperties_ItShouldNotifyAllChanges()
        {
            this.ViewModel.ZipCode = 9700;
            PropertyChanges[0].ShouldBe("ZipCode");
            PropertyChanges[1].ShouldBe("City");
            PropertyChanges[2].ShouldBe("State");
        }

        [Fact]
        public void WhenPropertyHasDoNotNotifyAttribute_ChangNotificationIsSuppressed()
        {
            this.ViewModel.DoNotNotifyChanges = "...";
            LastPropertyToChange.ShouldBeNull();
        }
    }
}