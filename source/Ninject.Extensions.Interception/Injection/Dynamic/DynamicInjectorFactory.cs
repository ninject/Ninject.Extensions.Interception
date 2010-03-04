#if !NO_LCG

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

using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace Ninject.Extensions.Interception.Injection.Dynamic
{
    /// <summary>
    /// Creates instances of injectors that use generated <see cref="DynamicMethod"/> objects
    /// for invocation.
    /// </summary>
    public class DynamicInjectorFactory : InjectorFactoryBase
    {
        /// <summary>
        /// Creates a new method injector.
        /// </summary>
        /// <param name="method">The method that the injector will invoke.</param>
        /// <returns>A new injector for the method.</returns>
        protected override IMethodInjector CreateInjector( MethodInfo method )
        {
            return new DynamicMethodInjector( method );
        }
    }
}

#endif //!NO_LCG