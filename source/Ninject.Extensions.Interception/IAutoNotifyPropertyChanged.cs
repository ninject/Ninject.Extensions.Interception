#region License

// 
// Author: Ian Davis <ian@innovatian.com>
// Copyright (c) 2010, Innovatian Software, LLC
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System.ComponentModel;

#endregion

namespace Ninject.Extensions.Interception
{
    /// <summary>
    /// Provides a common interface for classes that wish to provide automatic <see cref="INotifyPropertyChanged"/> functionality.
    /// </summary>
    public interface IAutoNotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the given property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void OnPropertyChanged( string propertyName );
    }
}