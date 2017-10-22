// -------------------------------------------------------------------------------------------------
// <copyright file="NotifyOfChangesAttribute.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Interception.Attributes
{
    using System;
    using System.Linq;

    using Ninject.Extensions.Interception.Request;

    /// <summary>
    /// Provides interceptor's details on how this property or class should participate in <see cref="IAutoNotifyPropertyChanged"/> events.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotifyOfChangesAttribute : InterceptAttributeBase
    {
        private static readonly Type DefaultInterceptorType = typeof(AutoNotifyPropertyChangedInterceptor<>);
        private static readonly Type DefaultServiceType = typeof(IAutoNotifyPropertyChangedInterceptor<>);

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyOfChangesAttribute"/> class.
        /// </summary>
        public NotifyOfChangesAttribute()
            : this(new string[] { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyOfChangesAttribute"/> class.
        /// </summary>
        /// <param name="notifyChangeFor">The dependent properties to also trigger events for.</param>
        public NotifyOfChangesAttribute(params string[] notifyChangeFor)
        {
            this.NotifyChangeFor = notifyChangeFor;
        }

        /// <summary>
        /// Gets the additional properties to notify changes for.
        /// </summary>
        /// <value>The notify change for.</value>
        public string[] NotifyChangeFor { get; private set; }

        /// <summary>
        /// Creates the interceptor associated with the attribute.
        /// </summary>
        /// <param name="request">The request that is being intercepted.</param>
        /// <returns>The interceptor.</returns>
        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            Type targetType = request.Target.GetType();
            Type serviceType = request.Kernel.GetBindings(DefaultServiceType).Any()
                                   ? DefaultServiceType
                                   : DefaultInterceptorType;
            Type closedInterceptorType = serviceType.MakeGenericType(targetType);
            var interceptor = (IInterceptor)request.Context.Kernel.Get(closedInterceptorType);
            return interceptor;
        }
    }
}