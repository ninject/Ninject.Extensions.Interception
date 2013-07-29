#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using System;

    using LinFu.DynamicProxy;

    public class InterceptionSyntaxContextLinfFu : InterceptionSyntaxContext<LinFuModule>
    {
        protected override Type ProxyType
        {
            get
            {
                return typeof(IProxy);
            }
        }
    }
}
#endif