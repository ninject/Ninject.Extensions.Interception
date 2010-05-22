#region Using Directives

using Ninject.Extensions.Interception.Attributes;

#endregion

namespace Ninject.Extensions.Interception.Tests.Fakes
{
    public class ViewModel : ViewModelBase
    {
        [NotifyOfChanges( "City", "State" )]
        public virtual int ZipCode { get; set; }


        public virtual string City
        {
            get { return string.Empty; }
        }

        public virtual string Address { get; set; }

        [DoNotNotifyOfChanges]
        public virtual string DoNotNotifyChanges { get; set; }
    }
}