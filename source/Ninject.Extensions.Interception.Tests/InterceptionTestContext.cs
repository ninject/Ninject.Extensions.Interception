#region Using Directives

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public abstract class InterceptionTestContext
    {
        protected virtual INinjectSettings GetSettings()
        {
            var ninjectSettings = new NinjectSettings { LoadExtensions = false };
            return ninjectSettings;
        }

        protected virtual StandardKernel CreateDefaultInterceptionKernel()
        {
            return new StandardKernel( GetSettings(), new LinFuModule() );
        }
    }
}