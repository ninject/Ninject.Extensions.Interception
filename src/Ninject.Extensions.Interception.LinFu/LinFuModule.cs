// -------------------------------------------------------------------------------------------------
// <copyright file="LinFuModule.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

#if !SILVERLIGHT && !NO_LINFU

namespace Ninject.Extensions.Interception
{
    using System;
    using Ninject.Extensions.Interception.ProxyFactory;

    /// <summary>
    /// Extends the functionality of the kernel, providing a proxy factory that uses LinFu
    /// to generate dynamic proxies.
    /// </summary>
    public class LinFuModule : InterceptionModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IProxyFactory, LinFuProxyFactory>();
            base.Load();
        }
    }
}

#endif //!SILVERLIGHT && !NO_LINFU