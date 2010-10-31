namespace Ninject.Extensions.Interception.Fakes
{
    public class Mock : IMock
    {
        public Mock( string myProperty )
        {
            MyProperty = myProperty;
        }

        public virtual string MyProperty { get; set; }

        public virtual string GetMyProperty()
        {
            return MyProperty;
        }

        public virtual void SetMyProperty( string myProperty )
        {
            MyProperty = myProperty;
        }
    }
}