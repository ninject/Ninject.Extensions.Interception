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

#endregion

namespace Ninject.Extensions.Interception.Attributes
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
    public class DoNotNotifyOfChangesAttribute : DoNotInterceptAttribute
    {
    }
}