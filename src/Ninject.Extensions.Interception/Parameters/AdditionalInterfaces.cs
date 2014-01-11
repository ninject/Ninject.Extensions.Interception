namespace Ninject.Extensions.Interception.Parameters
{
    using System;
    using Ninject.Activation;
    using Ninject.Parameters;
    using Ninject.Planning.Targets;

    /// <summary>
    /// Additional Interfaces
    /// </summary>
    public class AdditionalInterfaces : IParameter
    {
        private Type[] additionalInterfaces;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalInterfaces"/> class.
        /// </summary>
        /// <param name="additionalInterfaces">The additional interface types</param>
        public AdditionalInterfaces(Type[] additionalInterfaces)
        {
            this.additionalInterfaces = additionalInterfaces;
        }

        /// <summary>
        /// Get the interface array
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target</param>
        /// <returns>The interface array</returns>
        public object GetValue(IContext context, ITarget target)
        {
            return this.additionalInterfaces;
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the parameter should be inherited into child requests.
        /// </summary>
        public bool ShouldInherit
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the object equals the specified object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>True</c> if the objects are equal; otherwise <c>false</c></returns>
        public bool Equals(IParameter other)
        {
            throw new NotImplementedException();
        }
    }
}
