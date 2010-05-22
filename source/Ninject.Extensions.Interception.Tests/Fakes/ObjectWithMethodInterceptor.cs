#region Using Directives

using Ninject.Extensions.Interception.Tests.Attributes;

#endregion

namespace Ninject.Extensions.Interception.Tests.Fakes
{
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