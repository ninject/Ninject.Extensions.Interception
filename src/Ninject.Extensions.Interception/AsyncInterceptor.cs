#if !NET_35 && !SILVERLIGHT
namespace Ninject.Extensions.Interception
{
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// A simple definition of an interceptor, which can take action both before and after
    /// the invocation proceeds and supports async methods.
    /// </summary>
    public abstract class AsyncInterceptor : IInterceptor
    {
        private static MethodInfo continueWithMethodInfo = typeof(AsyncInterceptor).GetMethod("ContinueWith", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            this.BeforeInvoke(invocation);
            invocation.Proceed();
            this.AfterInvokeInternal(invocation);
        }
        
        private void AfterInvokeInternal(IInvocation invocation)
        {
            var task = invocation.ReturnValue as Task;
            if (task != null)
            {
                var taskType = task.GetType();
                if (taskType.IsGenericType)
                {
                    var resultType = task.GetType().GetGenericArguments()[0];
                    var mi = continueWithMethodInfo.MakeGenericMethod(resultType);
                    invocation.ReturnValue = mi.Invoke(this, new object[] { task, invocation });
                    return;
                }

                invocation.ReturnValue = task.ContinueWith(
                    t =>
                        {
                            invocation.ReturnValue = null;
                            this.AfterInvoke(invocation);
                            this.AfterInvoke(invocation, t);
                        });
            }
        }

        private Task<TResult> ContinueWith<TResult>(Task<TResult> task, IInvocation invocation)
        {
            return task.ContinueWith(
                t =>
                    {
                        invocation.ReturnValue = t.Result;
                        this.AfterInvoke(invocation);
                        this.AfterInvoke(invocation, t);
                        return (TResult)invocation.ReturnValue;
                    });
        }

        /// <summary>
        /// Takes some action before the invocation proceeds.
        /// </summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void BeforeInvoke(IInvocation invocation)
        {
        }

        /// <summary>
        /// Takes some action after the invocation proceeds. 
        /// </summary>
        /// <remarks>Use one AfterInvoke method overload.</remarks>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void AfterInvoke(IInvocation invocation)
        {
        }

        /// <summary>
        /// Takes some action after the invocation proceeds.
        /// </summary>
        /// <remarks>Use one AfterInvoke method overload.</remarks>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        /// <param name="task">The task that was executed.</param>
        protected virtual void AfterInvoke(IInvocation invocation, Task task)
        {
        }
    }
}
#endif