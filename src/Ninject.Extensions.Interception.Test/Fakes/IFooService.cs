#if !NETCOREAPP2_0
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