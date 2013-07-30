namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Xunit;
    
    public abstract class AutoNotifyPropertyDetectChangesInterceptorContext : AutoNotifyPropertyChangedContext
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
            LastPropertyToChange.Should().BeNull();
        }

        [Fact]
        public void WhenValueAssignedIsTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            ComplexViewModel.Complex = value; // initialize
            LastPropertyToChange.Should().Be("Complex");
            LastPropertyToChange = null;
            var newValue = new ComplexType();
            ComplexViewModel.Complex = newValue; // test
            LastPropertyToChange.Should().BeNull();
        }

        [Fact]
        public void WhenValueAssignedIsNotTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            ComplexViewModel.Complex = value; // initialize
            LastPropertyToChange.Should().Be("Complex");
            LastPropertyToChange = null;
            var newValue = new ComplexType {Name = "Foo"};
            ComplexViewModel.Complex = newValue; // test
            LastPropertyToChange.Should().Be("Complex");
        }

        [Fact]
        public void WhenPropertySubValueAssignedIsNotTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            ComplexViewModel.Complex = value; // initialize
            LastPropertyToChange.Should().Be("Complex");
            LastPropertyToChange = null;
            var newValue = new ComplexType {Simple = new SimpleType {Id = 5}};
            ComplexViewModel.Complex = newValue; // test
            LastPropertyToChange.Should().Be("Complex");
        }
    }
}