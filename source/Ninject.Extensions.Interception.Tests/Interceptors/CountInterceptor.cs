#region Using Directives



#endregion

namespace Ninject.Extensions.Interception.Tests.Interceptors
{
    public class CountInterceptor : SimpleInterceptor
    {
        public static int Count { get; set; }

        protected override void BeforeInvoke( IInvocation invocation )
        {
            Count++;
        }

        public static void Reset()
        {
            Count = 0;
        }
    }
}