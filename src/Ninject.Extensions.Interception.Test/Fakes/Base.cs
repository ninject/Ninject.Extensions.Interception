namespace Ninject.Extensions.Interception.Fakes
{
    using Ninject.Extensions.Interception.Attributes;

    public class Base : IBase
    {
        [Count]
        public void DoBase()
        {
        }
    }
}