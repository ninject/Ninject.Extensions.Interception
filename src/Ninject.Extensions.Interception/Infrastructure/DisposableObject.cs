// -------------------------------------------------------------------------------------------------
// <copyright file="DisposableObject.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Infrastructure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// An abstract object that is disposable. Used for proper implementation of the Disposal pattern.
    /// </summary>
    public abstract class DisposableObject : IDisposableEx
    {
        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Releases all resources currently held by the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the specified member if it implements <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="member">The member to dispose.</param>
        protected static void DisposeMember(object member)
        {
            var disposable = member as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Disposes the collection and all of its contents, if they implement <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="collection">The collection to dispose.</param>
        protected static void DisposeCollection(IEnumerable collection)
        {
            if (collection != null)
            {
                foreach (object obj in collection)
                {
                    DisposeMember(obj);
                }

                DisposeMember(collection);
            }
        }

        /// <summary>
        /// Disposes the dictionary and all of its contents, if they implement <see cref="IDisposable"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
        /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
        /// <param name="dictionary">The dictionary to dispose.</param>
        protected static void DisposeDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary != null)
            {
                foreach (KeyValuePair<TKey, TValue> entry in dictionary)
                {
                    DisposeMember(entry.Value);
                }

                DisposeMember(dictionary);
            }
        }

        /// <summary>
        /// Releases all resources currently held by the object.
        /// </summary>
        /// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.IsDisposed = true;
        }
    }
}