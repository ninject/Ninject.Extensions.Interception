namespace Ninject.Extensions.Interception.Fakes
{
    public class RequestsConstructorInjection : IMock
    {
        private readonly SimpleObject _child;

        public RequestsConstructorInjection()
        {
        }

        [Inject]
        public RequestsConstructorInjection( SimpleObject child )
        {
            this._child = child;
        }

        public virtual SimpleObject Child
        {
            get { return this._child; }
        }
    }
}