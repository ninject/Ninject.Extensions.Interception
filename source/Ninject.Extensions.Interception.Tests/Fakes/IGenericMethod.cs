namespace Ninject.Extensions.Interception.Tests.Fakes
{
    public interface IGenericMethod
    {
        string ConvertGeneric<T>( T obj );
    }
}