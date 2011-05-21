namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    using Xunit.Should;

    public abstract class InterceptionTestContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
        protected virtual INinjectSettings GetSettings()
        {
#if !SILVERLIGHT
            return new NinjectSettings { LoadExtensions = false };
#else
            return new NinjectSettings();
#endif
        }

        protected virtual StandardKernel CreateDefaultInterceptionKernel()
        {
            return new StandardKernel( this.GetSettings(), new TInterceptionModule() );
        }
    }
}