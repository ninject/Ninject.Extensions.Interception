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



#endregion

namespace Ninject.Extensions.Interception.Attributes
{
    /// <summary>
    /// Base attribute used to mark methods for interception.
    /// </summary>
    /// <remarks>
    /// This class should be extended in order to provide an <see cref="IInterceptor"/> reference.
    /// If you are trying to create a cusom interception strategy, you should inherit from <see cref="InterceptAttributeBase"/>
    /// instead.
    /// </remarks>
    public abstract class InterceptAttribute : InterceptAttributeBase
    {
    }
}