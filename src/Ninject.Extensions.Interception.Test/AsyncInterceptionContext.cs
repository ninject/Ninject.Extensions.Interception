namespace Ninject.Extensions.Interception
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Xunit;

    public abstract class AsyncInterceptionContext : InterceptionTestContext
    {
        [Fact]
        public async Task AsyncMethodsCanBeIntercepted()
        {
            using (var kernel = this.CreateDefaultInterceptionKernel())
            {
                BeforeAfterCallOrderInterceptor.Reset();

                kernel.Bind<AsyncService>().ToSelf().Intercept().With<BeforeAfterCallOrderInterceptor>();
                var service = kernel.Get<AsyncService>();

                await service.DoAsync();

                BeforeAfterCallOrderInterceptor.Order.Should().Be("Before_Action_AfterCompleted_");
            }
        }

        [Fact]
        public async Task AsyncMethodsWithReturnValueCanBeIntercepted()
        {
            using (var kernel = this.CreateDefaultInterceptionKernel())
            {
                BeforeAfterCallOrderInterceptor.Reset();

                kernel.Bind<AsyncService>().ToSelf().Intercept().With<BeforeAfterCallOrderInterceptor>();
                var service = kernel.Get<AsyncService>();

                var result = await service.DoAsyncInt();

                BeforeAfterCallOrderInterceptor.Order.Should().Be("Before_Action_AfterCompleted_");
                result.Should().Be(42);
            }
        }

        [Fact]
        public async Task AsyncMethodsWithReturnValue_InterceptorsCanChangeTheResult()
        {
            using (var kernel = this.CreateDefaultInterceptionKernel())
            {
                BeforeAfterCallOrderInterceptor.Reset();

                kernel.Bind<AsyncService>().ToSelf().Intercept().With<IncreaseResultInterceptor>();
                var service = kernel.Get<AsyncService>();

                var result = await service.DoAsyncInt();

                BeforeAfterCallOrderInterceptor.Order.Should().Be("Before_Action_AfterCompleted_");
                result.Should().Be(43);
            }
        }

        public class AsyncService
        {
            public virtual async Task<int> DoAsyncInt()
            {
                int result = await this.DoInt();
                return result;
            }

            public virtual async Task DoAsync()
            {
                await this.Do();
            }

            private Task Do()
            {
                BeforeAfterCallOrderInterceptor.ActionCalled();
                return Task.Delay(100);
            }

            private Task<int> DoInt()
            {
                BeforeAfterCallOrderInterceptor.ActionCalled();
                return Task.Delay(100).ContinueWith((t, o) => 42, null);
            }
        }

        public class BeforeAfterCallOrderInterceptor : AsyncInterceptor
        {
            public static string Order
            {
                get; set;
            }

            public static void Reset()
            {
                Order = string.Empty;
            }

            public static void ActionCalled()
            {
                Order += "Action_";
            }

            protected override void BeforeInvoke(IInvocation invocation)
            {
                Order += "Before_";
            }

            protected override void AfterInvoke(IInvocation invocation, Task task)
            {
                if (task.IsCompleted)
                {
                    Order += "AfterCompleted_";
                }
                else
                {
                    Order += "BeforeCompleted_";
                }
            }
        }

        public class IncreaseResultInterceptor : BeforeAfterCallOrderInterceptor
        {
            protected override void AfterInvoke(IInvocation invocation)
            {
                invocation.ReturnValue = ((int)invocation.ReturnValue + 1);
            }
        }
    }
}