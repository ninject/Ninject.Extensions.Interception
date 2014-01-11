namespace Ninject.Extensions.Interception.Parameters
{
    using System;
    using Ninject.Activation;
    using Ninject.Parameters;
    using Ninject.Planning.Targets;

    public class AdditionalInterfaces : IParameter
    {
        private Type[] additionalInterfaces;

        public AdditionalInterfaces(Type[] additionalInterfaces)
        {
            this.additionalInterfaces = additionalInterfaces;
        }

        public object GetValue(IContext context, ITarget target)
        {
            return this.additionalInterfaces;
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public bool ShouldInherit
        {
            get { return false; }
        }

        public bool Equals(IParameter other)
        {
            throw new NotImplementedException();
        }
    }
}
