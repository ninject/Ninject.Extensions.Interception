namespace Ninject.Extensions.Interception.Fakes
{
    using System;

    using Ninject.Extensions.Interception.Attributes;

    public class NoneVirtualFooImplementation : IFoo
    {
        public bool TestProperty { get; set; }

        public void Foo()
        {
        }

        public void Bar()
        {
        }

        [Count]
        public void Baz()
        {
        }
    }
}