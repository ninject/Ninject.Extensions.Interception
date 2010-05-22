#region Using Directives

#endregion

namespace Ninject.Extensions.Interception.Tests.Interceptors
{
    public class FlagInterceptor : SimpleInterceptor
    {
        public static bool WasCalled { get; private set; }

        protected override void BeforeInvoke( IInvocation invocation )
        {
            WasCalled = true;
        }

        public static void Reset()
        {
            WasCalled = false;
        }
    }
}