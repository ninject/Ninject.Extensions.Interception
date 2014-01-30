namespace Ninject.Extensions.Interception.Fakes
{
    public class ImplicitDerived : Base, IDerived
    {
        void IDerived.DoDerived()
        {
        }
    }
}
