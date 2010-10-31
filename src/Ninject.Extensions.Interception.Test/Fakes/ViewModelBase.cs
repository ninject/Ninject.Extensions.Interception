namespace Ninject.Extensions.Interception.Fakes
{
    using System.ComponentModel;

    public class ViewModelBase : IAutoNotifyPropertyChanged
    {
        #region IAutoNotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged( string propertyName )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if ( handler != null )
            {
                handler( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        #endregion
    }
}