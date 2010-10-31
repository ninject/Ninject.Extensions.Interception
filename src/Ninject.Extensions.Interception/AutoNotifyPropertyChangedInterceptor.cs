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

using System.Collections.Generic;
using System.Reflection;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Infrastructure.Language;

#endregion

namespace Ninject.Extensions.Interception
{
    /// <summary>
    /// Provides interceptor capabilities for integration in the <see cref="IAutoNotifyPropertyChanged"/> interception scheme.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    public class AutoNotifyPropertyChangedInterceptor<TViewModel>
        : IAutoNotifyPropertyChangedInterceptor<TViewModel> where TViewModel : IAutoNotifyPropertyChanged
    {
        #region IAutoNotifyPropertyChangedInterceptor<TViewModel> Members

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            bool valuesAreEqual = ArePropertyValuesEqual(invocation);
            invocation.Proceed();
            if ( valuesAreEqual )
            {
                return;
            }
            MethodInfo methodInfo = invocation.Request.Method;
            var model = (TViewModel) invocation.Request.Proxy;
            model.OnPropertyChanged(methodInfo.GetPropertyFromMethod(methodInfo.DeclaringType).Name);

            ChangeNotificationForDependentProperties(methodInfo, model);
        }

        #endregion

        private static bool ArePropertyValuesEqual(IInvocation invocation)
        {
            PropertyInfo getter = invocation.Request.Method.GetPropertyFromMethod();
            object current = getter.GetValue(invocation.Request.Target, null);
            object value = invocation.Request.Arguments[0];
            return Equals(current, value);
        }

        private static void ChangeNotificationForDependentProperties(MethodInfo methodInfo,
                                                                     IAutoNotifyPropertyChanged model)
        {
            if (NoAdditionalProperties(methodInfo))
            {
                return;
            }

            IEnumerable<string> properties = GetAdditionalPropertiesToNotifyOfChanges(methodInfo);

            foreach (string propertyName in properties)
            {
                model.OnPropertyChanged(propertyName);
            }
        }

        private static bool NoAdditionalProperties(MethodInfo methodInfo)
        {
            PropertyInfo propertyInfo = methodInfo.GetPropertyFromMethod(methodInfo.DeclaringType);
            return (propertyInfo == null || propertyInfo.GetOneAttribute<NotifyOfChangesAttribute>() == null);
        }

        private static IEnumerable<string> GetAdditionalPropertiesToNotifyOfChanges(MethodInfo methodInfo)
        {
            PropertyInfo propertyInfo = methodInfo.GetPropertyFromMethod(methodInfo.DeclaringType);
            var attribute = propertyInfo.GetOneAttribute<NotifyOfChangesAttribute>();
            return attribute.NotifyChangeFor;
        }
    }
}