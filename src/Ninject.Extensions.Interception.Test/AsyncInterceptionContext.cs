namespace Ninject.Extensions.Interception
{
    using System;
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
        public async Task AsyncMethods_InterceptorsCanCatchException()
        {
            using (var kernel = this.CreateDefaultInterceptionKernel())
            {
                BeforeAfterCallOrderInterceptor.Reset();

                kernel.Bind<AsyncService>().ToSelf().Intercept().With<BeforeAfterCallOrderInterceptor>();
                var service = kernel.Get<AsyncService>();

                await service.DoAsyncThrow();

                BeforeAfterCallOrderInterceptor.Order.Should().Be("Before_Action_HandleException_AfterCompleted_");
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

        [Fact]
        public async Task AsyncMethodsWithReturnValue_InterceptorsCanCatchException()
        {
            using (var kernel = this.CreateDefaultInterceptionKernel())
            {
                BeforeAfterCallOrderInterceptor.Reset();

                kernel.Bind<AsyncService>().ToSelf().Intercept().With<BeforeAfterCallOrderInterceptor>();
                var service = kernel.Get<AsyncService>();

                var result = await service.DoAsyncIntThrow();

                BeforeAfterCallOrderInterceptor.Order.Should().Be("Before_Action_HandleException_AfterCompleted_");
                result.Should().Be(default);
            }
        }

        [Fact]
        public async Task AsyncMethodsWithReturnValue_InterceptorsSetDefaultReturnValueOnFault()
        {
            using (var kernel = this.CreateDefaultInterceptionKernel())
            {
                BeforeAfterCallOrderInterceptor.Reset();

                kernel.Bind<AsyncService>().ToSelf().Intercept().With<IncreaseResultInterceptor>();
                var service = kernel.Get<AsyncService>();

                var result = await service.DoAsyncIntThrow();

                BeforeAfterCallOrderInterceptor.Order.Should().Be("Before_Action_HandleException_AfterCompleted_");
                result.Should().Be(1);
            }
        }

        public class AsyncService
        {
            public virtual async Task DoAsync()
            {
                await this.Do();
            }

            public virtual async Task<int> DoAsyncInt()
            {
                int result = await this.DoInt();
                return result;
            }

            public virtual async Task DoAsyncThrow()
            {
                await this.DoThrow();
            }

            public virtual async Task<int> DoAsyncIntThrow()
            {
                int result = await this.DoIntThrow();
                return result;
            }

            private Task Do()
            {
                BeforeAfterCallOrderInterceptor.ActionCalled();
                return Task.Delay(100);
            }

            private Task DoThrow()
            {
                BeforeAfterCallOrderInterceptor.ActionCalled();
                throw new Exception();
            }

            private Task<int> DoInt()
            {
                BeforeAfterCallOrderInterceptor.ActionCalled();
                return Task.Delay(100).ContinueWith((t, o) => 42, null);
            }

            private Task<int> DoIntThrow()
            {
                BeforeAfterCallOrderInterceptor.ActionCalled();
                throw new Exception();
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

            protected override void HandleException(IInvocation invocation, Exception e)
            {
                Order += "HandleException_";
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