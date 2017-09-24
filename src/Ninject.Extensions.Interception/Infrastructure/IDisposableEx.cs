// -------------------------------------------------------------------------------------------------
// <copyright file="IDisposableEx.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Infrastructure
{
    using System;

    /// <summary>
    /// A disposable object.
    /// </summary>
    public interface IDisposableEx : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        bool IsDisposed { get; }
    }
}