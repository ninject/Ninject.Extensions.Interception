namespace Ninject.Extensions.Interception
{
    using System;

    using Castle.DynamicProxy;

    public class InterceptionSyntaxContextDynamicProxy2 : InterceptionSyntaxContext
    {
        protected override InterceptionModule InterceptionModule
        {
            get
            {
                return new DynamicProxyModule();
            }
        }

        protected override Type ProxyType
        {
            get
            {
                return typeof(IProxyTargetAccessor);
            }
        }
    }
}