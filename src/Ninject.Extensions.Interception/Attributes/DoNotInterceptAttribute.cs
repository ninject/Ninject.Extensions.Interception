// -------------------------------------------------------------------------------------------------
// <copyright file="DoNotInterceptAttribute.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Attributes
{
    using System;

    /// <summary>
    /// Indicates that the decorated method should not be intercepted by interceptors defined
    /// at the class level.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DoNotInterceptAttribute : Attribute
    {
    }
}