#region Using Directives

using Ninject.Activation;
using Ninject.Extensions.Interception.ProxyFactory;

#endregion

namespace Ninject.Extensions.Interception.Tests.Fakes
{
    public class DummyProxyFactory : ProxyFactoryBase
    {
        #region Overrides of ProxyFactoryBase

        /// <summary>
        /// Wraps the instance in the specified context in a proxy.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <returns>A proxy that wraps the instance.</returns>
        public override void Wrap( IContext context, InstanceReference reference )
        {
        }

        /// <summary>
        /// Unwraps the instance in the specified context.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <returns>The unwrapped instance.</returns>
        public override void Unwrap( IContext context, InstanceReference reference )
        {
        }

        #endregion
    }
}