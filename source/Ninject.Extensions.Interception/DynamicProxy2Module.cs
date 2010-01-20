using Ninject.Extensions.Interception.ProxyFactory;

namespace Ninject.Extensions.Interception
{
    public class DynamicProxy2Module : InterceptionModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Kernel.Components.Add<IProxyFactory, DynamicProxy2ProxyFactory>();
            base.Load();
        }
    }
}