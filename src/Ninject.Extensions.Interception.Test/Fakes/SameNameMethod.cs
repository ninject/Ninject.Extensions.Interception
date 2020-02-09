namespace Ninject.Extensions.Interception.Fakes
{
    public class SameNameMethod : ISameNameMethod
    {
        public void Foo()
        {
        }

        public void Foo<T>()
        {
        }

        public void Foo<T1, T2>()
        {
        }
    }
}