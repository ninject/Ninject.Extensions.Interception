#region Using Directives

using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;
using Ninject.Extensions.Interception.Tests.Interceptors;

#endregion

namespace Ninject.Extensions.Interception.Tests.Attributes
{
    public class FlagAttribute : InterceptAttribute
    {
        public override IInterceptor CreateInterceptor( IProxyRequest request )
        {
            return request.Context.Kernel.Get<FlagInterceptor>();
        }
    }
}