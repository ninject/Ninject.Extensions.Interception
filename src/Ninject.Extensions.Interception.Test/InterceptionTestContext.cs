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
            return new NinjectSettings { LoadExtensions = false };
        }

        protected virtual StandardKernel CreateDefaultInterceptionKernel()
        {
            return new StandardKernel( this.GetSettings(), this.InterceptionModule );
        }

        protected abstract InterceptionModule InterceptionModule { get; }
    }
}