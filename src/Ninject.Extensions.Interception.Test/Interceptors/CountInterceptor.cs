namespace Ninject.Extensions.Interception.Interceptors
{
    public class CountInterceptor : SimpleInterceptor
    {
        public CountInterceptor() : this( 0 )
        {
        }

        public CountInterceptor( int initialValue )
        {
            Count = initialValue;
        }

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