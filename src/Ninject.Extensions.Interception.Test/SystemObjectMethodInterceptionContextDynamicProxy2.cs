namespace Ninject.Extensions.Interception
{
    public class SystemObjectMethodInterceptionContextDynamicProxy2 : SystemObjectMethodInterceptionContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get { return new DynamicProxyModule(); }
        }
    }
}