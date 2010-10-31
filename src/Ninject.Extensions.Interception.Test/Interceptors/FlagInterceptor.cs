namespace Ninject.Extensions.Interception.Interceptors
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