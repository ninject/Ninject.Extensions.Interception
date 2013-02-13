namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Attributes;

    public interface IFoo
    {
        bool TestProperty { get; set; }
        void Foo();
        void Bar();
        void Baz();
    }
}