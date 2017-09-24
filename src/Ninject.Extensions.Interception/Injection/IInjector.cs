// -------------------------------------------------------------------------------------------------
// <copyright file="IInjector.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Injection
{
    using System.Reflection;

    /// <summary>
    /// An object that can inject one or more values into the specified member.
    /// </summary>
    /// <typeparam name="TMember">The type of member that the injector can inject.</typeparam>
    public interface IInjector<TMember>
        where TMember : MemberInfo
    {
        /// <summary>
        /// Gets the member associated with the injector.
        /// </summary>
        TMember Member { get; }
    }
}