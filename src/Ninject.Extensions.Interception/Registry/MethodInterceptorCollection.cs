// -------------------------------------------------------------------------------------------------
// <copyright file="MethodInterceptorCollection.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Registry
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Provides lookup table functionality to trace <see cref="MethodInfo"/> to a collection of <see cref="IInterceptor"/>.
    /// </summary>
    public class MethodInterceptorCollection : Dictionary<MethodInfo, List<IInterceptor>>
    {
        /// <summary>
        /// Adds the specified interceptor for the given method.
        /// </summary>
        /// <param name="method">The method to bind the interceptor to.</param>
        /// <param name="interceptor">The interceptor to add.</param>
        public void Add(MethodInfo method, IInterceptor interceptor)
        {
            if (!this.ContainsKey(method))
            {
                this.Add(method, new List<IInterceptor>());
            }

            this[method].Add(interceptor);
        }
    }
}