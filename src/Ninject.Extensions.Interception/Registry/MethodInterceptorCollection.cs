// -------------------------------------------------------------------------------------------------
// <copyright file="MethodInterceptorCollection.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
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