// -------------------------------------------------------------------------------------------------
// <copyright file="IAdviceBuilder.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Advice.Builders
{
    using System;

    /// <summary>
    /// Builds information associated with advice.
    /// </summary>
    public interface IAdviceBuilder : IDisposable
    {
        /// <summary>
        /// Gets the advice the builder should manipulate.
        /// </summary>
        IAdvice Advice { get; }
    }
}