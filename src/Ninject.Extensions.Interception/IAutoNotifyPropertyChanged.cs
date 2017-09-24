// -------------------------------------------------------------------------------------------------
// <copyright file="IAutoNotifyPropertyChanged.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception
{
    using System.ComponentModel;

    /// <summary>
    /// Provides a common interface for classes that wish to provide automatic <see cref="INotifyPropertyChanged"/> functionality.
    /// </summary>
    public interface IAutoNotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the given property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void OnPropertyChanged(string propertyName);
    }
}