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
    /// Represents a method that gets the value stored in a field or property on
    /// the specified object.
    /// </summary>
    /// <param name="target">The object to get the value from.</param>
    /// <returns>The value stored in the associated field or property.</returns>
    public delegate object Getter( object target );
}