// -------------------------------------------------------------------------------------------------
// <copyright file="InterceptAttribute.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

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