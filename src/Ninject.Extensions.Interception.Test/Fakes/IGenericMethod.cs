namespace Ninject.Extensions.Interception.Fakes
{
    public interface IGenericMethod
    {
        string ConvertGeneric<T>(string s,  T obj );
    }
}