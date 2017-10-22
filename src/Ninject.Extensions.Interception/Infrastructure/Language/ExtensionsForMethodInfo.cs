// -------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsForMethodInfo.cs" company="Ninject Project Contributors">
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
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides extension methods for <see cref="MethodInfo"/>.
    /// </summary>
    internal static class ExtensionsForMethodInfo
    {
        private const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// Gets the property from the special method using the given type.
        /// </summary>
        /// <param name="method">The method info.</param>
        /// <param name="implementingType">The given type.</param>
        /// <returns>The property info.</returns>
        public static PropertyInfo GetPropertyFromMethod(this MethodInfo method, Type implementingType)
        {
            if (!method.IsSpecialName || method.Name.Length < 4)
            {
                return null;
            }

            var isGetMethod = method.Name.Substring(0, 3) == "get";
            var returnType = isGetMethod ? method.ReturnType : method.GetParameterTypes().Last();
            var indexerTypes = isGetMethod ? method.GetParameterTypes() : method.GetParameterTypes().SkipLast(1);

            return implementingType.GetProperty(method.Name.Substring(4), DefaultBindingFlags, null, returnType, indexerTypes.ToArray(), null);
        }

        /// <summary>
        /// Gets the property from the special method.
        /// </summary>
        /// <param name="method">The method info.</param>
        /// <returns>The property info.</returns>
        public static PropertyInfo GetPropertyFromMethod(this MethodInfo method)
        {
            if (!method.IsSpecialName)
            {
                return null;
            }

            return method.DeclaringType.GetProperty(method.Name.Substring(4), DefaultBindingFlags);
        }
    }
}