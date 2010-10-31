#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using Ninject.Activation.Strategies;
using Ninject.Planning.Directives;

#endregion

namespace Ninject.Extensions.Interception.Planning.Directives
{
    /// <summary>
    /// Provides hints to the <see cref="IActivationStrategy"/> pipeline that the type should be proxied.
    /// </summary>
    public class ProxyDirective : IDirective
    {
    }
}