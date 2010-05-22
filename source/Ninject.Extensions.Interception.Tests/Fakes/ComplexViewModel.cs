using System;
using Ninject.Extensions.Interception.Attributes;

namespace Ninject.Extensions.Interception.Tests.Fakes
{
    public class ComplexViewModel : ViewModelBase, IEquatable<ComplexViewModel>
    {
        [NotifyOfChanges]
        public virtual ComplexType Complex { get; set; }

        #region IEquatable<ComplexViewModel> Members

        public bool Equals(ComplexViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Complex, Complex);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ComplexViewModel)) return false;
            return Equals((ComplexViewModel) obj);
        }

        public override int GetHashCode()
        {
            return (Complex != null ? Complex.GetHashCode() : 0);
        }

        public static bool operator ==(ComplexViewModel left, ComplexViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComplexViewModel left, ComplexViewModel right)
        {
            return !Equals(left, right);
        }
    }
}