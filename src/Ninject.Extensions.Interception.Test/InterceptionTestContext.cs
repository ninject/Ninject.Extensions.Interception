namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.Fakes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Interceptors;
    using Xunit;
    
    public abstract class InterceptionTestContext
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
            return new StandardKernel( this.GetSettings(), InterceptionModule );
        }

        protected abstract InterceptionModule InterceptionModule { get; }
    }
}