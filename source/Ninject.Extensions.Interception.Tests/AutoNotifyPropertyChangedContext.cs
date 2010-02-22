#region Using Directives

using System.Collections.Generic;
using LinFu.DynamicProxy;
using Ninject.Extensions.Interception.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    /*
    public class AutoNotifyPropertyChangedContext : InterceptionTestContext
    {
        public AutoNotifyPropertyChangedContext()
        {
            Kernel = CreateDefaultInterceptionKernel();
            PropertyChanges = new List<string>();
            LastPropertyToChange = null;
        }

        public IKernel Kernel { get; set; }

        internal string LastPropertyToChange { get; set; }
        public List<string> PropertyChanges { get; set; }
    }

    public class AutoNotifyPropertyMethodInterceptorContext : AutoNotifyPropertyChangedContext
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
        public void WhenAutoNotifyAttributeIsAttachedToAProperty_TheObjectIsProxied()
        {
            Assert.IsAssignableFrom<IProxy>( ViewModel );
        }

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

    public class AutoNotifyPropertyClassProxyContext : AutoNotifyPropertyChangedContext
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
        public void WhenAutoNotifyAttributeIsAttachedToAClass_TheObjectIsProxied()
        {
            Assert.IsAssignableFrom<IProxy>( ViewModel );
        }

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
     * */
}