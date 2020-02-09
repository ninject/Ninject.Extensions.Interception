namespace Ninject.Extensions.Interception.Fakes
{
    public interface ISameNameMethod
    {
        void Foo();

        void Foo<T>();

        void Foo<T1, T2>();
    }
}