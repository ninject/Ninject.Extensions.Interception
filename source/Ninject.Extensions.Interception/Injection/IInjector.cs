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

#endregion

namespace Ninject.Extensions.Interception.Injection
{
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