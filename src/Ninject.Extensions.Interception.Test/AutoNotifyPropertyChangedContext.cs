namespace Ninject.Extensions.Interception
{
    using System.Collections.Generic;

    public abstract class AutoNotifyPropertyChangedContext : InterceptionTestContext
    {
        public AutoNotifyPropertyChangedContext()
        {
            this.Kernel = this.CreateDefaultInterceptionKernel();
            this.PropertyChanges = new List<string>();
            this.LastPropertyToChange = null;
        }

        public IKernel Kernel { get; set; }

        internal string LastPropertyToChange { get; set; }
        public List<string> PropertyChanges { get; set; }
    }
}