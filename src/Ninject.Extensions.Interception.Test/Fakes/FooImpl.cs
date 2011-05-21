namespace Ninject.Extensions.Interception.Fakes
{
    using System;

    public class FooImpl : IFoo
    {
        public bool TestProperty
        {
            get; set;
        }

        public virtual void Foo()
        {
            
        }

        public void Bar()        {
            throw new NotImplementedException();
        }
    }
}