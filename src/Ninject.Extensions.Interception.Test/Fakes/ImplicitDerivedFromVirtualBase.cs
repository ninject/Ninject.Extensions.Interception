namespace Ninject.Extensions.Interception.Fakes
{
    public class ImplicitDerivedFromVirtualBase : VirtualBase, IDerived
    {
        void IDerived.DoDerived()
        {
        }
    }
}
