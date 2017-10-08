#if !NETCOREAPP2_0
namespace Ninject.Extensions.Interception
{
    public class AutoNotifyPropertyDetectChangesInterceptorContextLinFu : AutoNotifyPropertyDetectChangesInterceptorContext
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