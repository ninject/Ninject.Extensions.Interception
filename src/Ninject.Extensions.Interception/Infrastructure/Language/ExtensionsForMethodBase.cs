// -------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsForMethodBase.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Infrastructure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Provides extension methods for <see cref="MethodBase"/>.
    /// </summary>
    internal static class ExtensionsForMethodBase
    {
        /// <summary>
        /// Gets the types of the parameters of the method.
        /// </summary>
        /// <param name="method">The method in question.</param>
        /// <returns>An array containing the types of the method's parameters.</returns>
        public static IEnumerable<Type> GetParameterTypes(this MethodBase method)
        {
            return method.GetParameters().Convert(p => p.ParameterType);
        }

        /// <summary>
        /// Gets the method handle of either the method or its generic type definition, if it is
        /// a generic method.
        /// </summary>
        /// <param name="method">The method in question.</param>
        /// <returns>The runtime method handle for the method or its generic type definition.</returns>
        public static RuntimeMethodHandle GetMethodHandle(this MethodBase method)
        {
            var mi = method as MethodInfo;

            if (mi != null &&
                 mi.IsGenericMethod)
            {
                return mi.GetGenericMethodDefinition().MethodHandle;
            }

            return method.MethodHandle;
        }
    }
}