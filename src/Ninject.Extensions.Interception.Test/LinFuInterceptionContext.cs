#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    public class LinFuInterceptionContext : InterceptionTestContext
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