#region Using Directives

using Ninject.Extensions.Interception.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public abstract class AutoNotifyPropertyMethodInterceptorContext<TInterceptionModule>
        : AutoNotifyPropertyChangedContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
        public AutoNotifyPropertyMethodInterceptorContext()
        {
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
            Assert.Null( LastPropertyToChange );
        }

        [Fact]
        public void WhenValueChangesOnPropertyWithDependentProperties_ItShouldNotifyAllChanges()
        {
            ViewModel.ZipCode = 9700;
            Assert.Equal( PropertyChanges[0], "ZipCode" );
            Assert.Equal( PropertyChanges[1], "City" );
            Assert.Equal( PropertyChanges[2], "State" );
        }
    }
}