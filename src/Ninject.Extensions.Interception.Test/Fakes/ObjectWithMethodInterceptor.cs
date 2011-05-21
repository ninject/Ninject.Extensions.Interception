namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Attributes;

    public class ObjectWithMethodInterceptor : IFoo
    {
        #region IFoo Members

        public bool TestProperty { get; set; }

        [Count]
        public virtual void Foo()
        {
        }

        #endregion

        public void Bar()
        {
        }

        public void Baz()
        {
        }
    }
}