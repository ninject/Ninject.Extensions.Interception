namespace Ninject.Extensions.Interception.ProxyFactory
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Castle.DynamicProxy;

    using Ninject.Extensions.Interception.Wrapper;

    public class ProxyBase
    {
        private static readonly MethodInfo ToStringMethodInfo = typeof(object).GetMethod("ToString");
        private static readonly MethodInfo GetHashCodeMethodInfo = typeof(object).GetMethod("GetHashCode");
        private static readonly MethodInfo EqualsMethodInfo = typeof(object).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public);

        public override int GetHashCode()
        {
            return this.InterceptMethod(() => base.GetHashCode(), GetHashCodeMethodInfo, new object[0]);
        }

        public override string ToString()
        {
            return this.InterceptMethod(() => base.ToString(), ToStringMethodInfo, new object[0]);
        }

        public override bool Equals(object obj)
        {
            return this.InterceptMethod(() => base.Equals(obj), EqualsMethodInfo, new[] { obj });
        }

        private TResult InterceptMethod<TResult>(Func<TResult> invokeBase, MethodInfo method, object[] arguments)
        {
            var proxy = this as IProxyTargetAccessor;
            if (proxy == null)
            {
                return invokeBase();
            }

            var interceptor = proxy.GetInterceptors().FirstOrDefault() as DynamicProxyWrapper;
            if (interceptor == null)
            {
                return invokeBase();
            }

            var invocation = new ObjectMethodsInvocation(this, interceptor.Instance, method, arguments);
            interceptor.Intercept(invocation);
            return (TResult)invocation.ReturnValue;
        }
    }
}