// -------------------------------------------------------------------------------------------------
// <copyright file="DynamicProxyModule.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !NO_CDP2
namespace Ninject.Extensions.Interception
{
    using Ninject.Extensions.Interception.ProxyFactory;

    /// <summary>
    /// Extends the functionality of the kernel, providing a proxy factory that uses Castle DynamicProxy2
    /// to generate dynamic proxies.
    /// </summary>
    public class DynamicProxyModule : InterceptionModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IProxyFactory, DynamicProxyProxyFactory>();
            base.Load();
        }
    }
}

#endif //!MONO && !NO_CDP2