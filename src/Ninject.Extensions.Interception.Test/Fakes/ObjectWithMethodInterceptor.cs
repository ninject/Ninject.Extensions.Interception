namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Attributes;

    public class ObjectWithMethodInterceptor : IFoo
    {
        #region IFoo Members

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