namespace Ninject.Extensions.Interception.Tests.Fakes
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IFooService
    {
        [OperationContract]
        void Foodo();
    }
}