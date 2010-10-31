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

#endregion

namespace Ninject.Extensions.Interception.Infrastructure
{
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