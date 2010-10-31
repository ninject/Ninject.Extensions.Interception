#if !SILVERLIGHT
namespace Ninject.Extensions.Interception.Fakes
{
    using System;
    using System.ServiceModel;

    [ServiceContract]
    public interface IFooService
    {
        [OperationContract]
        IAsyncResult Foodo(AsyncCallback cb, object arg);
    }
}
#endif