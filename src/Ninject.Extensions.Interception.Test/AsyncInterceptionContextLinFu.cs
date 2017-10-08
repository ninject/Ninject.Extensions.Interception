#if !NETCOREAPP2_0
namespace Ninject.Extensions.Interception
{
    public class AsyncInterceptionContextLinFu : AsyncInterceptionContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new LinFuModule();
            }
        }
    }
}
#endif