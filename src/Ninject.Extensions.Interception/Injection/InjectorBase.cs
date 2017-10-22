// -------------------------------------------------------------------------------------------------
// <copyright file="InjectorBase.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Injection
{
    using System.Reflection;

    using Ninject.Extensions.Interception.Infrastructure;

    /// <summary>
    /// A baseline definition of an injector. This type can be extended to create custom injectors.
    /// </summary>
    /// <typeparam name="TMember">The type of the member.</typeparam>
    public abstract class InjectorBase<TMember> : DisposableObject, IInjector<TMember>
        where TMember : MemberInfo
    {
        private TMember member;

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectorBase{TMember}"/> class.
        /// Creates a new InjectorBase.
        /// </summary>
        /// <param name="member">The member associated with the injector.</param>
        protected InjectorBase(TMember member)
        {
            Ensure.ArgumentNotNull(member, "member");
            this.member = member;
        }

        /// <summary>
        /// Gets or sets the member associated with the injector.
        /// </summary>
        public TMember Member
        {
            get { return this.member; }
            set { this.member = value; }
        }

        /// <summary>
        /// Releases all resources currently held by the object.
        /// </summary>
        /// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !this.IsDisposed)
            {
                this.member = null;
            }

            base.Dispose(disposing);
        }
    }
}