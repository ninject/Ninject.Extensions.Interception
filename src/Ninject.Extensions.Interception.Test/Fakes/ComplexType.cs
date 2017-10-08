namespace Ninject.Extensions.Interception.Fakes
{
    using System;

    public class ComplexType : IEquatable<ComplexType>
    {
        public ComplexType()
        {
            this.Name = GetType().Name;
            this.Simple = new SimpleType();
        }

        public string Name { get; set; }
        public SimpleType Simple { get; set; }

        #region IEquatable<ComplexType> Members

        public bool Equals(ComplexType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, this.Name) && Equals(other.Simple, this.Simple);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ComplexType)) return false;
            return Equals((ComplexType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Name != null ? this.Name.GetHashCode() : 0)*397) ^ (this.Simple != null ? this.Simple.GetHashCode() : 0);
            }
        }

        public static bool operator ==(ComplexType left, ComplexType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComplexType left, ComplexType right)
        {
            return !Equals(left, right);
        }
    }
}