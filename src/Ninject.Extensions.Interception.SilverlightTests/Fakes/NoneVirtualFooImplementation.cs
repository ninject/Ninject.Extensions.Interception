namespace Ninject.Extensions.Interception.Fakes
{
    using System;

    public class NoneVirtualFooImplementation : IFoo
    {
        public bool TestProperty { get; set; }

        public void Foo()
        {
        }

        public void Bar()
        {
        }
    }
}