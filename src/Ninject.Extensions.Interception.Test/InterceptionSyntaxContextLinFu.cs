#if !NETCOREAPP2_0
namespace Ninject.Extensions.Interception
{
    using System;

    using LinFu.DynamicProxy;

    public class InterceptionSyntaxContextLinfFu : InterceptionSyntaxContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new LinFuModule();
            }
        }

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