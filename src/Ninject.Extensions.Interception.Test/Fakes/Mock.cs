namespace Ninject.Extensions.Interception.Fakes
{
    public class Mock : IMock
    {
        public Mock( string myProperty )
        {
            this.MyProperty = myProperty;
        }

        public virtual string MyProperty { get; set; }

        public virtual string GetMyProperty()
        {
            return this.MyProperty;
        }

        public virtual void SetMyProperty( string myProperty )
        {
            this.MyProperty = myProperty;
        }
    }
}