namespace Ninject.Extensions.Interception
{
    public class MethodInterceptionContextDynamicProxy2 : MethodInterceptionContext
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