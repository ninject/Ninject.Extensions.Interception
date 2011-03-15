namespace Ninject.Extensions.Interception.Fakes
{
    public class SameNameProperty : SameNamePropertyBase
    {
        public new string TestProperty { get; set; }

        public string this[int i]
        {
            get
            {
                return string.Empty;
            }
            
            set
            {
            }
        }

        public new string this[string i]
        {
            get
            {
                return string.Empty;
            }

            set
            {
            }
        }
    }
}