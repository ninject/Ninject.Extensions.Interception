#if !MONO

#region License

// 
// Author: Ian Davis <ian@innovatian.com>
// Copyright (c) 2010, Innovatian Software, LLC
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using Ninject.Extensions.Interception.ProxyFactory;

#endregion

namespace Ninject.Extensions.Interception
{
    /// <summary>
    /// Extends the functionality of the kernel, providing a proxy factory that uses Castle DynamicProxy2
    /// to generate dynamic proxies.
    /// </summary>
    public class DynamicProxy2Module : InterceptionModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            //var proxyFactory = Kernel.Components.Get<IProxyFactory>();
            //if(proxyFactory != null)
            //{
            //    string message =
            //        string.Format(
            //            "IProxyFactory already bound to kernel. Please only load a single interception module. The IProxyFactory found was of type {0}.",
            //            proxyFactory.GetType().FullName );
            //    throw new InvalidOperationException(message);
            //}
            Kernel.Components.Add<IProxyFactory, DynamicProxy2ProxyFactory>();
            base.Load();
        }
    }
}

#endif //!MONO