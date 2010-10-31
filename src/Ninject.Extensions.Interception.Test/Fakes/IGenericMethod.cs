namespace Ninject.Extensions.Interception.Fakes
{
    public interface IGenericMethod
    {
        string ConvertGeneric<T>( T obj );
    }
}