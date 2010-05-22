using System;

namespace Ninject.Extensions.Interception.Tests.Fakes
{
    public class SimpleType : IEquatable<SimpleType>
    {
        public int Id { get; set; }

        #region IEquatable<SimpleType> Members

        public bool Equals(SimpleType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SimpleType)) return false;
            return Equals((SimpleType) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(SimpleType left, SimpleType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SimpleType left, SimpleType right)
        {
            return !Equals(left, right);
        }
    }
}