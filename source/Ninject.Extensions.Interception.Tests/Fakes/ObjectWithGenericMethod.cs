#region Using Directives

using Ninject.Extensions.Interception.Tests.Attributes;

#endregion

namespace Ninject.Extensions.Interception.Tests.Fakes
{
    public class ObjectWithGenericMethod : IGenericMethod
    {
        #region IGenericMethod Members

        [Flag]
        public virtual string ConvertGeneric<T>( T obj )
        {
            return obj.ToString();
        }

        #endregion

        public virtual string Convert( object obj )
        {
            return obj.ToString();
        }
    }
}