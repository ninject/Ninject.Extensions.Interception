using Ninject.Extensions.Interception.Attributes;

namespace Ninject.Extensions.Interception.Tests.Fakes
{
    [NotifyOfChanges]
    public class ViewModelWithClassNotify : ViewModelBase
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