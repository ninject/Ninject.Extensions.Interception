namespace Ninject.Extensions.Interception
{
    public class AutoNotifyPropertyDetectChangesInterceptorContextDynamicProxy2 
        : AutoNotifyPropertyDetectChangesInterceptorContext
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