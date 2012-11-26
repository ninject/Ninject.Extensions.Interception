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
using System.Linq;

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
            if ( !method.IsSpecialName||method.Name.Length<4)
            {
                return null;
            }

            var isGetMethod = method.Name.Substring(0, 3) == "get";
            var returnType = isGetMethod ? method.ReturnType : method.GetParameterTypes().Last();
            var indexerTypes = isGetMethod ? method.GetParameterTypes() : method.GetParameterTypes().SkipLast(1);

            return implementingType.GetProperty( method.Name.Substring( 4 ), DefaultBindingFlags, null, returnType, indexerTypes.ToArray(), null);
        }

        public static PropertyInfo GetPropertyFromMethod( this MethodInfo method )
        {
            if ( !method.IsSpecialName )
            {
                return null;
            }
            return method.DeclaringType.GetProperty( method.Name.Substring(4), DefaultBindingFlags );
        }
    }
}