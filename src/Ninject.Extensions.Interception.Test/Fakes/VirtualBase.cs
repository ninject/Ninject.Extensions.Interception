namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Attributes;

    public class VirtualBase : IBase
    {
        [Count]
        public virtual void DoBase()
        {
        }
    }
}