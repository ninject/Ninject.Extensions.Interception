// -------------------------------------------------------------------------------------------------
// <copyright file="DoNotNotifyOfChangesAttribute.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Attributes
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Indicated that the <see cref="INotifyPropertyChanged"/> events should not be raised for the given property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DoNotNotifyOfChangesAttribute : DoNotInterceptAttribute
    {
    }
}