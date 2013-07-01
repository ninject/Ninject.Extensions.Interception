namespace Ninject.Extensions.Interception.Fakes
{
    public class FooWithNoDefaultConstructor : IFoo
    {
        public FooWithNoDefaultConstructor(IMock mock)
        {
        }

        public bool TestProperty
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        public void Foo()
        {
        }

        public void Bar()
        {
        }

        public void Baz()
        {
        }
    }
}