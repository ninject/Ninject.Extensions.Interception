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

using System;
using System.Reflection;

#endregion

namespace Ninject.Extensions.Interception.Infrastructure.Language
{
    /// <summary>
    /// Extension methods that enhance <see cref="ICustomAttributeProvider"/>.
    /// </summary>
    internal static class ExtensionsForICustomAttributeProvider
    {
        /// <summary>
        /// Gets the first attribute of a specified type that decorates the member.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <param name="member">The member to examine.</param>
        /// <returns>The first attribute matching the specified type.</returns>
        public static T GetOneAttribute<T>( this ICustomAttributeProvider member )
            where T : Attribute
        {
            var attributes = member.GetCustomAttributes( typeof (T), true ) as T[];

            if ( ( attributes == null ) ||
                 ( attributes.Length == 0 ) )
            {
                return null;
            }
            else
            {
                return attributes[0];
            }
        }

        /// <summary>
        /// Gets the first attribute of a specified type that decorates the member.
        /// </summary>
        /// <param name="member">The member to examine.</param>
        /// <param name="type">The type of attribute to search for.</param>
        /// <returns>The first attribute matching the specified type.</returns>
        public static object GetOneAttribute( this ICustomAttributeProvider member, Type type )
        {
            object[] attributes = member.GetCustomAttributes( type, true );

            if ( ( attributes == null ) ||
                 ( attributes.Length == 0 ) )
            {
                return null;
            }
            else
            {
                return attributes[0];
            }
        }

        /// <summary>
        /// Gets an array of attributes matching the specified type that decorate the member.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <param name="member">The member to examine.</param>
        /// <returns>An array of attributes matching the specified type.</returns>
        public static T[] GetAllAttributes<T>( this ICustomAttributeProvider member )
            where T : Attribute
        {
            return member.GetCustomAttributes( typeof (T), true ) as T[];
        }

        /// <summary>
        /// Gets an array of attributes matching the specified type that decorate the member.
        /// </summary>
        /// <param name="member">The member to examine.</param>
        /// <param name="type">The type of attribute to search for.</param>
        /// <returns>An array of attributes matching the specified type.</returns>
        public static object[] GetAllAttributes( this ICustomAttributeProvider member, Type type )
        {
            return member.GetCustomAttributes( type, true );
        }

        /// <summary>
        /// Determines whether the member is decorated with one or more attributes of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <param name="member">The member to examine.</param>
        /// <returns><see langword="True"/> if the member is decorated with one or more attributes of the type, otherwise <see langword="false"/>.</returns>
        public static bool HasAttribute<T>( this ICustomAttributeProvider member )
            where T : Attribute
        {
            return member.IsDefined( typeof (T), true );
        }

        /// <summary>
        /// Determines whether the member is decorated with one or more attributes of the specified type.
        /// </summary>
        /// <param name="member">The member to examine.</param>
        /// <param name="type">The type of attribute to search for.</param>
        /// <returns><see langword="True"/> if the member is decorated with one or more attributes of the type, otherwise <see langword="false"/>.</returns>
        public static bool HasAttribute( this ICustomAttributeProvider member, Type type )
        {
            return member.IsDefined( type, true );
        }

        /// <summary>
        /// Determines whether the member is decorated with an attribute that matches the one provided.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <param name="member">The member to examine.</param>
        /// <param name="attributeToMatch">The attribute to match against.</param>
        /// <returns><see langword="True"/> if the member is decorated with a matching attribute, otherwise <see langword="false"/>.</returns>
        public static bool HasMatchingAttribute<T>( this ICustomAttributeProvider member, T attributeToMatch )
            where T : Attribute
        {
            T[] attributes = member.GetAllAttributes<T>();

            if ( ( attributes == null ) ||
                 ( attributes.Length == 0 ) )
            {
                return false;
            }

            foreach ( T attribute in attributes )
            {
                if ( attribute.Match( attributeToMatch ) )
                {
                    return true;
                }
            }

            return false;
        }
    }
}