#region Using Directives

#endregion

#region Using Directives

using System.Collections.Generic;

#endregion

namespace Ninject.Extensions.Interception.Tests
{
    public abstract class AutoNotifyPropertyChangedContext<TInterceptionModule> :
        InterceptionTestContext<TInterceptionModule>
        where TInterceptionModule : InterceptionModule, new()
    {
        public AutoNotifyPropertyChangedContext()
        {
            Kernel = CreateDefaultInterceptionKernel();
            PropertyChanges = new List<string>();
            LastPropertyToChange = null;
        }

        public IKernel Kernel { get; set; }

        internal string LastPropertyToChange { get; set; }
        public List<string> PropertyChanges { get; set; }
    }
}