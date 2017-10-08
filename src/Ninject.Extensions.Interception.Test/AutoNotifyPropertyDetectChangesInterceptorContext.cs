namespace Ninject.Extensions.Interception
{
    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Xunit;
    
    public abstract class AutoNotifyPropertyDetectChangesInterceptorContext : AutoNotifyPropertyChangedContext
    {
        public AutoNotifyPropertyDetectChangesInterceptorContext()
        {
            this.ViewModel = this.Kernel.Get<ViewModel>();
            this.ViewModel.PropertyChanged += (o, e) =>
                                             {
                                                 this.LastPropertyToChange = e.PropertyName;
                                                 this.PropertyChanges.Add(this.LastPropertyToChange);
                                             };

            this.ComplexViewModel = this.Kernel.Get<ComplexViewModel>();
            this.ComplexViewModel.PropertyChanged += (o, e) =>
                                                    {
                                                        this.LastPropertyToChange = e.PropertyName;
                                                        this.PropertyChanges.Add(this.LastPropertyToChange);
                                                    };
        }

        public ViewModel ViewModel { get; set; }

        public ComplexViewModel ComplexViewModel { get; set; }

        [Fact]
        public void WhenValueAssignedIsTheSameAsTheCurrentValue_ItShouldNotNotify()
        {
            this.ViewModel.ZipCode = 0;
            this.LastPropertyToChange.Should().BeNull();
        }

        [Fact]
        public void WhenValueAssignedIsTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            this.ComplexViewModel.Complex = value; // initialize
            this.LastPropertyToChange.Should().Be("Complex");
            this.LastPropertyToChange = null;
            var newValue = new ComplexType();
            this.ComplexViewModel.Complex = newValue; // test
            this.LastPropertyToChange.Should().BeNull();
        }

        [Fact]
        public void WhenValueAssignedIsNotTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            this.ComplexViewModel.Complex = value; // initialize
            this.LastPropertyToChange.Should().Be("Complex");
            this.LastPropertyToChange = null;
            var newValue = new ComplexType {Name = "Foo"};
            this.ComplexViewModel.Complex = newValue; // test
            this.LastPropertyToChange.Should().Be("Complex");
        }

        [Fact]
        public void WhenPropertySubValueAssignedIsNotTheSameAsTheCurrentComplexValue_ItShouldNotNotify()
        {
            var value = new ComplexType();
            this.ComplexViewModel.Complex = value; // initialize
            this.LastPropertyToChange.Should().Be("Complex");
            this.LastPropertyToChange = null;
            var newValue = new ComplexType {Simple = new SimpleType {Id = 5}};
            this.ComplexViewModel.Complex = newValue; // test
            this.LastPropertyToChange.Should().Be("Complex");
        }
    }
}