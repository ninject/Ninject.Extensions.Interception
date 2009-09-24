#region Using Directives



#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public abstract class InterceptionTestContext
    {
        protected INinjectSettings GetSettings()
        {
            var ninjectSettings = new NinjectSettings {LoadExtensions = false};
            return ninjectSettings;
        }

        protected StandardKernel CreateDefaultInterceptionKernel()
        {
            return new StandardKernel( GetSettings(), new InterceptionModule() );
        }
    }
}