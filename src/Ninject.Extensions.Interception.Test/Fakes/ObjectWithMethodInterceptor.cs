namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Attributes;

    public class ObjectWithMethodInterceptor : IFoo
    {
        #region IFoo Members

        public virtual bool TestProperty { get; set; }

        [Count]
        public virtual bool TestProperty2 { get; set; }

        [Count]
        public virtual void Foo()
        {
        }

        #endregion

        public virtual void Bar()
        {
        }

        public virtual void Baz()
        {
        }
    }
}