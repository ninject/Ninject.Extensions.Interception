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

using System.Reflection;
using Ninject.Extensions.Interception.Infrastructure;

#endregion

namespace Ninject.Extensions.Interception.Injection
{
    /// <summary>
    /// A baseline definition of an injector. This type can be extended to create custom injectors.
    /// </summary>
    /// <typeparam name="TMember"></typeparam>
    public abstract class InjectorBase<TMember> : DisposableObject, IInjector<TMember>
        where TMember : MemberInfo
    {
        #region Fields

        private TMember _member;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the member associated with the injector.
        /// </summary>
        public TMember Member
        {
            get { return _member; }
            set { _member = value; }
        }

        #endregion

        #region Disposal

        /// <summary>
        /// Releases all resources currently held by the object.
        /// </summary>
        /// <param name="disposing"><see langword="True"/> if managed objects should be disposed, otherwise <see langword="false"/>.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && !IsDisposed )
            {
                _member = null;
            }

            base.Dispose( disposing );
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new InjectorBase.
        /// </summary>
        /// <param name="member">The member associated with the injector.</param>
        protected InjectorBase( TMember member )
        {
            Ensure.ArgumentNotNull( member, "member" );
            _member = member;
        }

        #endregion
    }
}