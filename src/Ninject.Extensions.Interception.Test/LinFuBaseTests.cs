#if !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using FluentAssertions;
    using LinFu.DynamicProxy;
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    
    public class LinFuBaseTests : LinFuInterceptionContext
    {
        [Fact]
        public void SelfBoundTypesDeclaringInterceptorsOnGenericMethodsAreIntercepted()
        {
            using (var kernel = CreateDefaultInterceptionKernel())
            {
                kernel.Bind<ObjectWithGenericMethod>().ToSelf();
                var obj = kernel.Get<ObjectWithGenericMethod>();
                obj.Should().NotBeNull();
                typeof(IProxy).IsAssignableFrom(obj.GetType()).Should().BeTrue();

                FlagInterceptor.Reset();
                string result = obj.ConvertGeneric("", 42);

                result.Should().Be("42");
                FlagInterceptor.WasCalled.Should().BeTrue();
            }
        }
    }
}
#endif