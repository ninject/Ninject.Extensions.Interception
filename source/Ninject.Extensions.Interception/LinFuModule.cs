#region Using Directives

using Ninject.Extensions.Interception.ProxyFactory;

#endregion

namespace Ninject.Extensions.Interception
{
    public class LinFuModule : InterceptionModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Kernel.Components.Add<IProxyFactory, LinFuProxyFactory>();
            base.Load();
        }
    }
}