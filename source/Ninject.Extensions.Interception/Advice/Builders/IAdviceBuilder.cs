#region Using Directives

using System;

#endregion

namespace Ninject.Extensions.Interception.Advice.Builders
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