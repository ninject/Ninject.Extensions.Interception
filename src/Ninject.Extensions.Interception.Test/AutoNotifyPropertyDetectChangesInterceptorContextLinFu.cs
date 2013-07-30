#if !SILVERLIGHT
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