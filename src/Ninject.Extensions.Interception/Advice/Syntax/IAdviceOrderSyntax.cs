// -------------------------------------------------------------------------------------------------
// <copyright file="IAdviceOrderSyntax.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Advice.Syntax
{
    using Ninject.Syntax;

    /// <summary>
    /// Describes a fluent syntax for modifying the order of an interception.
    /// </summary>
    public interface IAdviceOrderSyntax : IFluentSyntax
    {
        /// <summary>
        /// Indicates that the interceptor should be called with the specified order. (Interceptors
        /// with a lower order will be called first.)
        /// </summary>
        /// <param name="order">The order.</param>
        void InOrder(int order);
    }
}