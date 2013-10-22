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

        public override string ToString()
        {
            return "42";
        }

        public override int GetHashCode()
        {
            return 42;
        }

        public override bool Equals(object obj)
        {
            return (int)obj == 42;
        }
    }
}