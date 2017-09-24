// -------------------------------------------------------------------------------------------------
// <copyright file="AdviceFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Advice
{
    using System;
    using System.Reflection;
    using Ninject.Activation;
    using Ninject.Components;
    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// The stock definition of an advice factory.
    /// </summary>
    public class AdviceFactory : NinjectComponent, IAdviceFactory
    {
        /// <summary>
        /// Creates static advice for the specified method.
        /// </summary>
        /// <param name="method">The method that will be intercepted.</param>
        /// <returns>The created advice.</returns>
        public IAdvice Create(MethodInfo method)
        {
            return new Advice(method);
        }

        /// <summary>
        /// Creates dynamic advice for the specified condition.
        /// </summary>
        /// <param name="condition">The condition that will be evaluated to determine whether a request should be intercepted.</param>
        /// <returns>The created advice.</returns>
        public IAdvice Create(Predicate<IContext> condition)
        {
            return new Advice(condition);
        }

        /// <summary>
        /// Creates a dynamic advice for the specified condition. That will intercept calls to the
        /// methods matching the method predicate
        /// </summary>
        /// <param name="condition">The condition that will be evaluated to determine whether a request should be intercepted.</param>
        /// <param name="methodPredicate">The condition that will be evaluated to determine whether a call to a method should be intercepted.</param>
        /// <returns>The created advice.</returns>
        public IAdvice Create(Predicate<IContext> condition, Predicate<MethodInfo> methodPredicate)
        {
            return new Advice(condition, methodPredicate);
        }
    }
}