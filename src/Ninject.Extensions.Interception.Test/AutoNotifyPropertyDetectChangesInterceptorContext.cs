namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Xunit;
    using Xunit.Should;

    public abstract class AutoNotifyPropertyDetectChangesInterceptorContext<TInterceptionModule>
        : AutoNotifyPropertyChangedContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
        public AutoNotifyPropertyDetectChangesInterceptorContext()
        {
            ViewModel = Kernel.Get<ViewModel>();
            ViewModel.PropertyChanged += (o, e) =>
                                             {
                                                 LastPropertyToChange = e.PropertyName;
                                                 PropertyChanges.Add(LastPropertyToChange);
                                             };

            ComplexViewModel = Kernel.Get<ComplexViewModel>();
            ComplexViewModel.PropertyChanged += (o, e) =>
                                                    {
                                                        LastPropertyToChange = e.PropertyName;
                                                        PropertyChanges.Add(LastPropertyToChange);
                                                    };
        }

        public ViewModel ViewModel { get; set; }

        public ComplexViewModel ComplexViewModel { get; set; }

        [Fact]
        public void WhenValueAssignedIsTheSameAsTheCurrentValue_ItShouldNotNotify()
        {
            ViewModel.ZipCode = 0;
            LastPropertyToChange.ShouldBeNull();
        }

        [Fact]
        public void WhenValueAssignedIsTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            ComplexViewModel.Complex = value; // initialize
            LastPropertyToChange.ShouldBe("Complex");
            LastPropertyToChange = null;
            var newValue = new ComplexType();
            ComplexViewModel.Complex = newValue; // test
            LastPropertyToChange.ShouldBeNull();
        }

        [Fact]
        public void WhenValueAssignedIsNotTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            ComplexViewModel.Complex = value; // initialize
            LastPropertyToChange.ShouldBe("Complex");
            LastPropertyToChange = null;
            var newValue = new ComplexType {Name = "Foo"};
            ComplexViewModel.Complex = newValue; // test
            LastPropertyToChange.ShouldBe("Complex");
        }

        [Fact]
        public void WhenPropertySubValueAssignedIsNotTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            ComplexViewModel.Complex = value; // initialize
            LastPropertyToChange.ShouldBe("Complex");
            LastPropertyToChange = null;
            var newValue = new ComplexType {Simple = new SimpleType {Id = 5}};
            ComplexViewModel.Complex = newValue; // test
            LastPropertyToChange.ShouldBe("Complex");
        }
    }
}