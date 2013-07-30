namespace Ninject.Extensions.Interception
{
    public class AutoNotifyPropertyChangedContextDynamicProxy2
        : AutoNotifyPropertyChangedContext
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