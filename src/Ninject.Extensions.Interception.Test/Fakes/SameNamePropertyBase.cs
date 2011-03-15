namespace Ninject.Extensions.Interception.Fakes
{
    public class SameNamePropertyBase
    {
        public int TestProperty { get; set; }
    
        public string this[string i]
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