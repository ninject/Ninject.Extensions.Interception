// -------------------------------------------------------------------------------------------------
// <copyright file="FactoryMethod.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Injection
{
    /// <summary>
    /// Represents a method that creates instances of a type.
    /// </summary>
    /// <param name="arguments">A collection of arguments to pass to the constructor.</param>
    /// <returns>An instance of the associated type.</returns>
    public delegate object FactoryMethod(params object[] arguments);
}