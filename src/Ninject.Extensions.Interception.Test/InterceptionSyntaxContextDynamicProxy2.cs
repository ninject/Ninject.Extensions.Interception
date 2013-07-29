namespace Ninject.Extensions.Interception
{
    using System;

    using Castle.DynamicProxy;

    public class InterceptionSyntaxContextDynamicProxy2 : InterceptionSyntaxContext<DynamicProxyModule>
    {
        protected override Type ProxyType
        {
            get
            {
                return typeof(IProxyTargetAccessor);
            }
        }
    }
}