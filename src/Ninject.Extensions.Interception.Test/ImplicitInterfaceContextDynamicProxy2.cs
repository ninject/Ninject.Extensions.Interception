namespace Ninject.Extensions.Interception
{
    public class ImplicitInterfaceContextDynamicProxy2 : ImplicitInterfaceContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new DynamicProxyModule();
            }
        }
    }
}
