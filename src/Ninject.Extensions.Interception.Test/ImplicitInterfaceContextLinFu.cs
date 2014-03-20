#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using System;
    using Xunit;

    public class ImplicitInterfaceContextLinFu : ImplicitInterfaceContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new LinFuModule();
            }
        }

        public override void InterceptedClassObjectCanImplementImplicitInterfaces()
        {
            Assert.Throws<TypeLoadException>(() => this.BaseInterceptedClassObjectCanImplementImplicitInterfaces());
        }

        private void BaseInterceptedClassObjectCanImplementImplicitInterfaces()
        {
            base.InterceptedClassObjectCanImplementImplicitInterfaces();
        }
    }
}
#endif