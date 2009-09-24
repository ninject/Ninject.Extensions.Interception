#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Collections.Generic;

#endregion

namespace Ninject.Extensions.Interception.Infrastructure.Language
{
    /// <summary>
    /// Extension methods that enhance <see cref="IEnumerable{T}"/>.
    /// </summary>
    internal static class ExtensionsForIEnumerable
    {
        /// <summary>
        /// Converts all of the items in the specified series using the specified converter.
        /// </summary>
        /// <typeparam name="TInput">The type of items contained in the input list.</typeparam>
        /// <typeparam name="TOutput">The type of items to return.</typeparam>
        /// <param name="items">The series of items to convert.</param>
        /// <param name="converter">The converter to use to convert the items.</param>
        /// <returns>A list of the converted items.</returns>
        public static IEnumerable<TOutput> Convert<TInput, TOutput>( this IEnumerable<TInput> items,
                                                                     Func<TInput, TOutput> converter )
        {
            foreach ( TInput item in items )
            {
                yield return converter( item );
            }
        }
    }
}