// -------------------------------------------------------------------------------------------------
// <copyright file="AutoNotifyPropertyChangedInterceptor.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010 Enkari, Ltd. All rights reserved.
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception
{
    using System.Collections.Generic;
    using System.Reflection;

    using Ninject.Extensions.Interception.Attributes;
    using Ninject.Extensions.Interception.Infrastructure.Language;

    /// <summary>
    /// Provides interceptor capabilities for integration in the <see cref="IAutoNotifyPropertyChanged"/> interception scheme.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    public class AutoNotifyPropertyChangedInterceptor<TViewModel>
        : IAutoNotifyPropertyChangedInterceptor<TViewModel>
        where TViewModel : IAutoNotifyPropertyChanged
    {
        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            bool valuesAreEqual = ArePropertyValuesEqual(invocation);
            invocation.Proceed();
            if (valuesAreEqual)
            {
                return;
            }

            MethodInfo methodInfo = invocation.Request.Method;
            var model = (TViewModel)invocation.Request.Proxy;
            model.OnPropertyChanged(methodInfo.GetPropertyFromMethod(methodInfo.DeclaringType).Name);

            ChangeNotificationForDependentProperties(methodInfo, model);
        }

        private static bool ArePropertyValuesEqual(IInvocation invocation)
        {
            PropertyInfo getter = invocation.Request.Method.GetPropertyFromMethod();
            object current = getter.GetValue(invocation.Request.Target, null);
            object value = invocation.Request.Arguments[0];
            return Equals(current, value);
        }

        private static void ChangeNotificationForDependentProperties(
            MethodInfo methodInfo,
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
            return propertyInfo == null || propertyInfo.GetOneAttribute<NotifyOfChangesAttribute>() == null;
        }

        private static IEnumerable<string> GetAdditionalPropertiesToNotifyOfChanges(MethodInfo methodInfo)
        {
            PropertyInfo propertyInfo = methodInfo.GetPropertyFromMethod(methodInfo.DeclaringType);
            var attribute = propertyInfo.GetOneAttribute<NotifyOfChangesAttribute>();
            return attribute.NotifyChangeFor;
        }
    }
}