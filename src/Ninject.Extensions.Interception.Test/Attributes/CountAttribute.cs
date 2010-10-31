namespace Ninject.Extensions.Interception.Attributes
{
    using Ninject.Extensions.Interception.Interceptors;
    using Ninject.Extensions.Interception.Request;

    public class CountAttribute : InterceptAttribute
    {
        public override IInterceptor CreateInterceptor( IProxyRequest request )
        {
            return request.Context.Kernel.Get<CountInterceptor>();
        }
    }
}