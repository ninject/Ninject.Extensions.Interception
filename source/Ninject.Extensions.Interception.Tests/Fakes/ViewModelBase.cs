#region Using Directives

using System.ComponentModel;

#endregion

namespace Ninject.Extensions.Interception.Tests.Fakes
{
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