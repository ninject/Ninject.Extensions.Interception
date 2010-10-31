namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Tests.Attributes;

    public class ObjectWithGenericMethod : IGenericMethod
    {
        [Flag]
        public virtual string ConvertGeneric<T>( T obj )
        {
            return obj.ToString();
        }

        public virtual string Convert( object obj )
        {
            return obj.ToString();
        }
    }
}