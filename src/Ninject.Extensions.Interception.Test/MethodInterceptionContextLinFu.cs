#if !NETCOREAPP2_0
namespace Ninject.Extensions.Interception
{
    public class MethodInterceptionContextLinFu : MethodInterceptionContext
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