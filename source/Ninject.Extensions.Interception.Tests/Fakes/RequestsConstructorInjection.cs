namespace Ninject.Extensions.Interception.Tests.Fakes
{
    public class RequestsConstructorInjection : IMock
    {
        private SimpleObject _child;

        public RequestsConstructorInjection()
        {
        }

        [Inject]
        public RequestsConstructorInjection( SimpleObject child )
        {
            _child = child;
        }

        public virtual SimpleObject Child
        {
            get { return _child; }
        }
    }
}