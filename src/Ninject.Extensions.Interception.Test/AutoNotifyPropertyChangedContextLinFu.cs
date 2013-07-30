#if !SILVERLIGHT

namespace Ninject.Extensions.Interception
{
    public class AutoNotifyPropertyChangedContextLinFu : AutoNotifyPropertyChangedContext
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