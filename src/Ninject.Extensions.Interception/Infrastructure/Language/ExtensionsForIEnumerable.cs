// -------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsForIEnumerable.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010 Enkari, Ltd. All rights reserved.
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Infrastructure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
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
        public static IEnumerable<TOutput> Convert<TInput, TOutput>(
            this IEnumerable<TInput> items,
            Func<TInput, TOutput> converter)
        {
            return items.Select(converter);
        }

        /// <summary>
        /// Skips the last items where the count of skipped items is given by count.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="count">The count of skipped items.</param>
        /// <returns>An enumerable that skippes the last items from the source enumerable.</returns>
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
        {
            var enumerator = source.GetEnumerator();
            Queue<T> items = new Queue<T>();

            while (enumerator.MoveNext())
            {
                if (count-- <= 0)
                {
                    yield return items.Dequeue();
                }

                items.Enqueue(enumerator.Current);
            }
        }
    }
}