namespace Ninject.Extensions.Interception
{
    public class DynamicProxy2InterceptionContext : InterceptionTestContext
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