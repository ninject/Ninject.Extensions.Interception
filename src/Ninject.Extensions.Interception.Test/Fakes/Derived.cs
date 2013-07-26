namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Attributes;

    public class Derived : Base, IDerived
    {
        [Count]
        public void DoDerived()
        {
        }
    }
}