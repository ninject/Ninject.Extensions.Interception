#if !NET_35 && !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    public class AsyncInterceptionContextDynamicProxy2 : AsyncInterceptionContext
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
#endif