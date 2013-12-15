namespace Ninject.Extensions.Interception
{
    public class ImplicitInterfaceContextLinFu : ImplicitInterfaceContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new LinFuModule();
            }
        }

        public override void InterceptedClassObjectShouldKeepImplementingImplicitInterfaces()
        {
        }
    }
}
