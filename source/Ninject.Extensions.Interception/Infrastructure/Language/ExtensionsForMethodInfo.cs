#region License

// 
// Author: Ian Davis <ian@innovatian.com>
// Copyright (c) 2010, Innovatian Software, LLC
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Reflection;

#endregion

namespace Ninject.Extensions.Interception.Infrastructure.Language
{
    internal static class ExtensionsForMethodInfo
    {
        private const BindingFlags DefaultBindingFlags =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance;

        public static PropertyInfo GetPropertyFromMethod( this MethodInfo method, Type implementingType )
        {
            if ( !method.IsSpecialName )
            {
                return null;
            }
            return implementingType.GetProperty( method.Name.Substring( 4 ), DefaultBindingFlags );
        }
    }
}