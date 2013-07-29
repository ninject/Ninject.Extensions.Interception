namespace Ninject.Extensions.Interception
{
    using System;
    using System.Collections.Generic;
    using Castle.DynamicProxy;

    using FluentAssertions;

    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    
    public class DynamicProxy2BaseTests : DynamicProxy2InterceptionContext
    {
        [Fact]
        public void SelfBoundTypesThatAreProxiedReceiveConstructorInjections()
        {
            using ( StandardKernel kernel = CreateDefaultInterceptionKernel() )
            {
                kernel.Bind<RequestsConstructorInjection>().ToSelf();
                // This is just here to trigger proxying, but we won't intercept any calls
                kernel.Intercept(request => true).With<FlagInterceptor>();

                var obj = kernel.Get<RequestsConstructorInjection>();

                obj.Should().NotBeNull();
                typeof(IProxyTargetAccessor).IsAssignableFrom(obj.GetType()).Should().BeTrue();
                obj.Child.Should().NotBeNull();
            }
        }
    }
}