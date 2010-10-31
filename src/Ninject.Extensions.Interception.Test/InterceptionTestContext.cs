namespace Ninject.Extensions.Interception
{
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