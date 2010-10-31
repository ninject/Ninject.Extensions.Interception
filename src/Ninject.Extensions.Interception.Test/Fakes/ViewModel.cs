namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Attributes;

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