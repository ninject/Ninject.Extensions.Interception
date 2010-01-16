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

using System.Reflection;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Infrastructure.Language;

#endregion

namespace Ninject.Extensions.Interception
{
    public class AutoNotifyPropertyChangedInterceptor<TViewModel> : IInterceptor
        where TViewModel : IAutoNotifyPropertyChanged
    {
        #region IInterceptor Members

        public void Intercept( IInvocation invocation )
        {
            invocation.Proceed();

            MethodInfo methodInfo = invocation.Request.Method;
            var model = (TViewModel) invocation.Request.Proxy;
            model.OnPropertyChanged( methodInfo.GetPropertyFromMethod( methodInfo.DeclaringType ).Name );

            ChangeNotificationForDependentProperties( methodInfo, model );
        }

        #endregion

        private static void ChangeNotificationForDependentProperties( MethodInfo methodInfo,
                                                                      IAutoNotifyPropertyChanged model )
        {
            if ( NoAdditionalProperties( methodInfo ) )
            {
                return;
            }

            string[] properties = GetAdditionalPropertiesToNotifyOfChanges( methodInfo );

            foreach ( string propertyName in properties )
            {
                model.OnPropertyChanged( propertyName );
            }
        }

        private static bool NoAdditionalProperties( MethodInfo methodInfo )
        {
            PropertyInfo propertyInfo = methodInfo.GetPropertyFromMethod( methodInfo.DeclaringType );
            return ( propertyInfo == null || propertyInfo.GetOneAttribute<NotifyOfChangesAttribute>() == null );
        }

        private static string[] GetAdditionalPropertiesToNotifyOfChanges( MethodInfo methodInfo )
        {
            PropertyInfo propertyInfo = methodInfo.GetPropertyFromMethod( methodInfo.DeclaringType );
            var attribute = propertyInfo.GetOneAttribute<NotifyOfChangesAttribute>();
            return attribute.NotifyChangeFor;
        }
    }
}