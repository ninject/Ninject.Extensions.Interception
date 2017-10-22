// -------------------------------------------------------------------------------------------------
// <copyright file="LinFuModule.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010 Enkari, Ltd. All rights reserved.
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
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