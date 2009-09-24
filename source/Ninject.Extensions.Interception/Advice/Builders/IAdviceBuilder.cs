#region Using Directives

using System;
using Ninject.Extensions.Interception.Advice;

#endregion

namespace Ninject.Interception
{
    /// <summary>
    /// Builds information associated with advice.
    /// </summary>
    public interface IAdviceBuilder : IDisposable
    {
        /// <summary>
        /// Gets the advice the builder should manipulate.
        /// </summary>
        IAdvice Advice { get; }
    }
}