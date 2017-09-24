// -------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsForMethodBase.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
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