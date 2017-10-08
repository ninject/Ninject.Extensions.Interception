#if !NETCOREAPP2_0
namespace Ninject.Extensions.Interception
{
    public class SystemObjectMethodInterceptionContextLinFu : SystemObjectMethodInterceptionContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get { return new LinFuModule(); }
        }
    }
}
#endif