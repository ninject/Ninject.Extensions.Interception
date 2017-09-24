// -------------------------------------------------------------------------------------------------
// <copyright file="AutoNotifyInterceptorRegistrationStrategy.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Planning.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Ninject.Extensions.Interception.Advice;
    using Ninject.Extensions.Interception.Attributes;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Extensions.Interception.Planning.Directives;
    using Ninject.Extensions.Interception.Registry;
    using Ninject.Planning;

    /// <summary>
    /// Examines the implementation type via reflection and registers any static interceptors
    /// that are defined via attributes.
    /// </summary>
    public class AutoNotifyInterceptorRegistrationStrategy : InterceptorRegistrationStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoNotifyInterceptorRegistrationStrategy"/> class.
        /// </summary>
        /// <param name="adviceFactory">The advice factory.</param>
        /// <param name="adviceRegistry">The advice registry.</param>
        public AutoNotifyInterceptorRegistrationStrategy(IAdviceFactory adviceFactory, IAdviceRegistry adviceRegistry)
            : base(adviceFactory, adviceRegistry)
        {
        }

        /// <summary>
        /// Contributes to the specified plan.
        /// </summary>
        /// <param name="plan">The plan that is being generated.</param>
        public override void Execute(IPlan plan)
        {
            if (!typeof(IAutoNotifyPropertyChanged).IsAssignableFrom(plan.Type))
            {
                return;
            }

            IEnumerable<MethodInfo> candidates = this.GetCandidateMethods(plan.Type);

            this.RegisterClassInterceptors(plan.Type, plan, candidates);

            foreach (MethodInfo method in candidates)
            {
                PropertyInfo property = method.GetPropertyFromMethod(method.DeclaringType);
                NotifyOfChangesAttribute[] attributes = property.GetAllAttributes<NotifyOfChangesAttribute>();

                if (attributes.Length == 0)
                {
                    continue;
                }

                this.RegisterMethodInterceptors(plan.Type, method, attributes);

                // Indicate that instances of the type should be proxied.
                if (!plan.Has<ProxyDirective>())
                {
                    plan.Add(new ProxyDirective());
                }
            }
        }

        /// <summary>
        /// Registers static interceptors defined by attributes on the class for all candidate
        /// methods on the class, execept those decorated with a <see cref="DoNotInterceptAttribute"/>.
        /// </summary>
        /// <param name="type">The type whose activation plan is being manipulated.</param>
        /// <param name="plan">The activation plan that is being manipulated.</param>
        /// <param name="candidates">The candidate methods to intercept.</param>
        protected override void RegisterClassInterceptors(Type type, IPlan plan, IEnumerable<MethodInfo> candidates)
        {
            NotifyOfChangesAttribute[] attributes = type.GetAllAttributes<NotifyOfChangesAttribute>();

            if (attributes.Length == 0)
            {
                return;
            }

            foreach (MethodInfo method in candidates)
            {
                if (!method.HasAttribute<DoNotNotifyOfChangesAttribute>())
                {
                    this.RegisterMethodInterceptors(type, method, attributes);
                }
            }

            // Indicate that instances of the type should be proxied.
            if (!plan.Has<ProxyDirective>())
            {
                plan.Add(new ProxyDirective());
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="MethodInfo"/> should be intercepted.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns>
        ///     <c>true</c> if the method should be intercepted; <c>false</c> otherwise.
        /// </returns>
        protected override bool ShouldIntercept(MethodInfo methodInfo)
        {
            if (!IsPropertySetter(methodInfo))
            {
                return false;
            }

            if (IsDecoratedWithDoNotNotifyChangesAttribute(methodInfo))
            {
                return false;
            }

            return base.ShouldIntercept(methodInfo);
        }

        private static bool IsPropertySetter(MethodBase methodInfo)
        {
            return methodInfo.IsSpecialName && methodInfo.Name.StartsWith("set_");
        }

        private static bool IsDecoratedWithDoNotNotifyChangesAttribute(MethodInfo methodInfo)
        {
            PropertyInfo propertyInfo = methodInfo.GetPropertyFromMethod(methodInfo.DeclaringType);
            return propertyInfo != null && propertyInfo.GetOneAttribute<DoNotNotifyOfChangesAttribute>() != null;
        }
    }
}