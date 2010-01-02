#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

namespace Ninject.Extensions.Interception.Injection
{
    /// <summary>
    /// Represents a method that calls another method.
    /// </summary>
    /// <param name="target">The object on which to call the associated method.</param>
    /// <param name="arguments">A collection of arguments to pass to the associated method.</param>
    /// <returns>The return value of the method.</returns>
    public delegate object Invoker( object target, params object[] arguments );
}