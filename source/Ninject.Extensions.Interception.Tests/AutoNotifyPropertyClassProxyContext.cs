#region Using Directives

using Ninject.Extensions.Interception.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public abstract class AutoNotifyPropertyClassProxyContext<TInterceptionModule>
        : AutoNotifyPropertyChangedContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
        public AutoNotifyPropertyClassProxyContext()
        {
            ViewModel = Kernel.Get<ViewModelWithClassNotify>();
            ViewModel.PropertyChanged += ( o, e ) =>
                                         {
                                             LastPropertyToChange = e.PropertyName;
                                             PropertyChanges.Add( LastPropertyToChange );
                                         };
        }

        public ViewModelWithClassNotify ViewModel { get; set; }

        [Fact]
        public void WhenValueChangesOnPropertyWithoutNotifyAttribute_ItShouldNotifyChanges()
        {
            ViewModel.Address = "123 Main Street";
            Assert.Equal( LastPropertyToChange, "Address" );
        }

        [Fact]
        public void WhenValueChangesOnPropertyWithDependentProperties_ItShouldNotifyAllChanges()
        {
            ViewModel.ZipCode = 9700;
            Assert.Equal( PropertyChanges[0], "ZipCode" );
            Assert.Equal( PropertyChanges[1], "City" );
            Assert.Equal( PropertyChanges[2], "State" );
        }

        [Fact]
        public void WhenPropertyHasDoNotNotifyAttribute_ChangNotificationIsSuppressed()
        {
            ViewModel.DoNotNotifyChanges = "...";
            Assert.Null( LastPropertyToChange );
        }
    }
}