namespace Ninject.Extensions.Interception
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Xunit;

    public abstract class AsyncInterceptionContext : InterceptionTestContext, IDisposable
    {
        public AsyncInterceptionContext()
        {
            CallRecordingInterceptor.Reset();
            Kernel = this.CreateDefaultInterceptionKernel();
        }

        IKernel Kernel { get; set; }

        public void Dispose()
        {
            Kernel.Dispose();
        }

        [Fact]
        public void CanInterceptSynchronousMethods()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<CallRecordingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            service.Do();

            CallRecordingInterceptor.Order.Should().Be("CallRecordingBefore_Action_CallRecordingAfter_CallRecordingComplete_");
        }

        [Fact]
        public void CanHandleErrorsInSynchronousMethods()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<CallRecordingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            service.DoThrow();

            CallRecordingInterceptor.Order.Should().Be("CallRecordingBefore_Action_CallRecordingHandleException_CallRecordingComplete_");
        }

        [Fact]
        public void CanRethrowErrorsInSynchronousMethods()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<RethrowingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            Assert.Throws<Exception>(() => service.DoThrow());

            CallRecordingInterceptor.Order.Should().Be("RethrowingBefore_Action_RethrowingHandleException_RethrowingComplete_");
        }

        [Fact]
        public async Task CanInterceptAsyncMethods()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<CallRecordingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            await service.DoAsync();

            CallRecordingInterceptor.Order.Should().Be("CallRecordingBefore_Action_CallRecordingAfter_CallRecordingAfterIsCompleted_CallRecordingComplete_CallRecordingCompleteIsCompleted_");
        }

        [Fact]
        public async Task CanInterceptAsyncMethodsWithReturnCode()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<CallRecordingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            var result = await service.DoIntAsync();

            CallRecordingInterceptor.Order.Should().Be("CallRecordingBefore_Action_CallRecordingAfter_CallRecordingAfterIsCompleted_CallRecordingComplete_CallRecordingCompleteIsCompleted_");
            result.Should().Be(42);
        }

        [Fact]
        public async Task CanInterceptAsyncMethodsWithReturnCodeAndChangeTheResult()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<IncreaseResultInterceptor>();
            var service = Kernel.Get<AsyncService>();

            var result = await service.DoIntAsync();

            CallRecordingInterceptor.Order.Should().Be("IncreaseResultBefore_Action_IncreaseResultAfter_IncreaseResultAfterIsCompleted_IncreaseResultComplete_IncreaseResultCompleteIsCompleted_");
            result.Should().Be(43);
        }

        [Fact]
        public async Task CanHandleErrorsInAsyncMethods()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<CallRecordingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            await service.DoThrowAsync();

            CallRecordingInterceptor.Order.Should().Be("CallRecordingBefore_Action_CallRecordingHandleException_CallRecordingComplete_CallRecordingCompleteIsFaulted_");
        }

        [Fact]
        public async Task CanRethrowErrorsInAsyncMethods()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<RethrowingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            await Assert.ThrowsAsync<Exception>(() => service.DoThrowAsync());

            CallRecordingInterceptor.Order.Should().Be("RethrowingBefore_Action_RethrowingHandleException_RethrowingComplete_RethrowingCompleteIsFaulted_");
        }

        [Fact]
        public async Task DoesNotProceedIfBeforeFails()
        {
            Kernel.Bind<AsyncService>().ToSelf().Intercept().With<ThrowsBeforeInterceptor>();
            var service = Kernel.Get<AsyncService>();

            await Assert.ThrowsAsync<TaskCanceledException>(() => service.DoThrowAsync());

            CallRecordingInterceptor.Order.Should().Be("ThrowsBeforeBefore_ThrowsBeforeComplete_ThrowsBeforeCompleteIsCanceled_");
        }

        [Fact]
        public async Task FirstInterceptorCanSetReturnCode()
        {
            var binding = Kernel.Bind<AsyncService>().ToSelf();
            binding.Intercept().With<IncreaseResultInterceptor>();
            binding.Intercept().With<CallRecordingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            var result = await service.DoIntAsync();

            CallRecordingInterceptor.Order.Should().Be("IncreaseResultBefore_CallRecordingBefore_Action_CallRecordingAfter_CallRecordingAfterIsCompleted_CallRecordingComplete_CallRecordingCompleteIsCompleted_IncreaseResultAfter_IncreaseResultAfterIsCompleted_IncreaseResultComplete_IncreaseResultCompleteIsCompleted_");
            result.Should().Be(43);
        }

        [Fact]
        public async Task SecondInterceptorCanSetReturnCode()
        {
            var binding = Kernel.Bind<AsyncService>().ToSelf();
            binding.Intercept().With<CallRecordingInterceptor>();
            binding.Intercept().With<IncreaseResultInterceptor>();
            var service = Kernel.Get<AsyncService>();

            var result = await service.DoIntAsync();

            CallRecordingInterceptor.Order.Should().Be("CallRecordingBefore_IncreaseResultBefore_Action_IncreaseResultAfter_IncreaseResultAfterIsCompleted_IncreaseResultComplete_IncreaseResultCompleteIsCompleted_CallRecordingAfter_CallRecordingAfterIsCompleted_CallRecordingComplete_CallRecordingCompleteIsCompleted_");
            result.Should().Be(43);
        }

        [Fact]
        public async Task BothInterceptorsCanSetReturnCode()
        {
            var binding = Kernel.Bind<AsyncService>().ToSelf();
            binding.Intercept().With<IncreaseResultInterceptor>();
            binding.Intercept().With<IncreaseResultInterceptor>();
            var service = Kernel.Get<AsyncService>();

            var result = await service.DoIntAsync();

            CallRecordingInterceptor.Order.Should().Be("IncreaseResultBefore_IncreaseResultBefore_Action_IncreaseResultAfter_IncreaseResultAfterIsCompleted_IncreaseResultComplete_IncreaseResultCompleteIsCompleted_IncreaseResultAfter_IncreaseResultAfterIsCompleted_IncreaseResultComplete_IncreaseResultCompleteIsCompleted_");
            result.Should().Be(44);
        }

        [Fact]
        public async Task FirstInterceptorCanHandleErrorsAndSetReturnCode()
        {
            var binding = Kernel.Bind<AsyncService>().ToSelf();
            binding.Intercept().With<IncreaseResultInterceptor>();
            binding.Intercept().With<RethrowingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            var result = await service.DoIntThrowAsync();

            CallRecordingInterceptor.Order.Should().Be("IncreaseResultBefore_RethrowingBefore_Action_RethrowingHandleException_RethrowingComplete_RethrowingCompleteIsFaulted_IncreaseResultHandleException_IncreaseResultComplete_IncreaseResultCompleteIsFaulted_");
            result.Should().Be(1);
        }

        [Fact]
        public async Task SecondInterceptorCanHandleErrorsAndSetReturnCode()
        {
            var binding = Kernel.Bind<AsyncService>().ToSelf();
            binding.Intercept().With<RethrowingInterceptor>();
            binding.Intercept().With<IncreaseResultInterceptor>();
            var service = Kernel.Get<AsyncService>();

            var result = await service.DoIntThrowAsync();

            CallRecordingInterceptor.Order.Should().Be("RethrowingBefore_IncreaseResultBefore_Action_IncreaseResultHandleException_IncreaseResultComplete_IncreaseResultCompleteIsFaulted_RethrowingAfter_RethrowingAfterIsCompleted_RethrowingComplete_RethrowingCompleteIsCompleted_");
            result.Should().Be(1);
        }

        [Fact]
        public async Task BothInterceptorsCanRethrowErrors()
        {
            var binding = Kernel.Bind<AsyncService>().ToSelf();
            binding.Intercept().With<RethrowingInterceptor>();
            binding.Intercept().With<RethrowingInterceptor>();
            var service = Kernel.Get<AsyncService>();

            await Assert.ThrowsAsync<Exception>(() => service.DoThrowAsync());

            CallRecordingInterceptor.Order.Should().Be("RethrowingBefore_RethrowingBefore_Action_RethrowingHandleException_RethrowingComplete_RethrowingCompleteIsFaulted_RethrowingHandleException_RethrowingComplete_RethrowingCompleteIsFaulted_");
        }

        public class AsyncService
        {
            public virtual void Do()
            {
                CallRecordingInterceptor.ActionCalled();
            }

            public virtual void DoThrow()
            {
                Do();
                throw new Exception();
            }

            public virtual async Task DoAsync()
            {
                Do();
                await Task.Delay(1);
            }

            public virtual async Task<int> DoIntAsync()
            {
                Do();
                return await Task.Delay(1).ContinueWith((t, o) => 42, null);
            }

            public virtual async Task DoThrowAsync(Exception e = null)
            {
                Do();
                await this.ThrowAsync(e);
            }

            public virtual async Task<int> DoIntThrowAsync(Exception e = null)
            {
                Do();
                return await this.IntThrowAsync(e);
            }

            private Task ThrowAsync(Exception e = null)
            {
                throw e ?? new Exception();
            }

            private Task<int> IntThrowAsync(Exception e = null)
            {
                throw e ?? new Exception();
            }
        }

        public class CallRecordingInterceptor : AsyncInterceptor
        {
            public static string Order
            {
                get; set;
            }

            protected virtual string Name => "CallRecording";

            protected void Record(string callName)
            {
                Order += Name + callName + "_";
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
                Record("Before");
            }

            protected override void AfterInvoke(IInvocation invocation)
            {
                Record("After");
            }

            protected override void AfterInvoke(IInvocation invocation, Task task)
            {
                Record("After" + StateFor(task));
            }

            protected override bool HandleException(IInvocation invocation, Exception e)
            {
                Record("HandleException");
                return true;
            }

            protected override void CompleteInvoke(IInvocation invocation)
            {
                Record("Complete");
            }

            protected override void CompleteInvoke(IInvocation invocation, Task task)
            {
                Record("Complete" + StateFor(task));
            }

            private string StateFor(Task task)
            {
                if (task.IsCanceled)
                {
                    return "IsCanceled";
                }
                if (task.IsFaulted)
                {
                    return "IsFaulted";
                }
                if (task.IsCompleted)
                {
                    return "IsCompleted";
                }
                return string.Empty;
            }
        }

        public class ThrowsBeforeInterceptor : CallRecordingInterceptor
        {
            protected override string Name => "ThrowsBefore";

            protected override void BeforeInvoke(IInvocation invocation)
            {
                base.BeforeInvoke(invocation);
                throw new Exception();
            }
        }

        public class RethrowingInterceptor : CallRecordingInterceptor
        {
            protected override string Name => "Rethrowing";

            protected override bool HandleException(IInvocation invocation, Exception e)
            {
                base.HandleException(invocation, e);
                return false;
            }
        }

        public class IncreaseResultInterceptor : CallRecordingInterceptor
        {
            protected override string Name => "IncreaseResult";

            protected override bool HandleException(IInvocation invocation, Exception e)
            {
                base.HandleException(invocation, e);
                IncrementReturnValue(invocation);
                return true;
            }

            protected override void AfterInvoke(IInvocation invocation)
            {
                base.AfterInvoke(invocation);
                IncrementReturnValue(invocation);
            }

            private void IncrementReturnValue(IInvocation invocation)
            {
                invocation.ReturnValue = (int)invocation.ReturnValue + 1;
            }
        }
    }
}