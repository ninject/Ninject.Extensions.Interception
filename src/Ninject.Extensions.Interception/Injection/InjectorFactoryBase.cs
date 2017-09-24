// -------------------------------------------------------------------------------------------------
// <copyright file="InjectorFactoryBase.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Ninject.Components;

    /// <summary>
    /// Creates instances of injectors.
    /// </summary>
    public abstract class InjectorFactoryBase : NinjectComponent, IInjectorFactory
    {
        private readonly Dictionary<MethodInfo, IMethodInjector> methodInjectors =
            new Dictionary<MethodInfo, IMethodInjector>();

        /// <summary>
        /// Gets an injector for the specified method.
        /// </summary>
        /// <param name="method">The method that the injector will invoke.</param>
        /// <returns>A new injector for the method.</returns>
        public IMethodInjector GetInjector(MethodInfo method)
        {
            lock (this.methodInjectors)
            {
                if (method.IsGenericMethodDefinition)
                {
                    throw new InvalidOperationException()
                        /*ExceptionFormatter.CannotCreateInjectorFromGenericTypeDefinition(method)*/ ;
                }

                if (this.methodInjectors.ContainsKey(method))
                {
                    return this.methodInjectors[method];
                }

                IMethodInjector injector = this.CreateInjector(method);
                this.methodInjectors.Add(method, injector);

                return injector;
            }
        }

        /// <summary>
        /// Creates a new method injector.
        /// </summary>
        /// <param name="method">The method that the injector will invoke.</param>
        /// <returns>A new injector for the method.</returns>
        protected abstract IMethodInjector CreateInjector(MethodInfo method);
    }
}