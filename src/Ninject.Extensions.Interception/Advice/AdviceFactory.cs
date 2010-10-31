#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Reflection;
using Ninject.Components;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Advice
{
    /// <summary>
    /// The stock definition of an advice factory.
    /// </summary>
    public class AdviceFactory : NinjectComponent, IAdviceFactory
    {
        #region IAdviceFactory Members

        /// <summary>
        /// Creates static advice for the specified method.
        /// </summary>
        /// <param name="method">The method that will be intercepted.</param>
        /// <returns>The created advice.</returns>
        public IAdvice Create( MethodInfo method )
        {
            return new Advice( method );
        }

        /// <summary>
        /// Creates dynamic advice for the specified condition.
        /// </summary>
        /// <param name="condition">The condition that will be evaluated to determine whether a request should be intercepted.</param>
        /// <returns>The created advice.</returns>
        public IAdvice Create( Predicate<IProxyRequest> condition )
        {
            return new Advice( condition );
        }

        #endregion
    }
}