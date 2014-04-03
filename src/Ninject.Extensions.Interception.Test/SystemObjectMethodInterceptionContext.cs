using FluentAssertions;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Extensions.Interception.Request;
using Xunit;

namespace Ninject.Extensions.Interception
{
    public abstract class SystemObjectMethodInterceptionContext : InterceptionTestContext
    {
        public static bool InterceptionFlag = false;
        private readonly IKernel kernel;

        protected SystemObjectMethodInterceptionContext()
        {
            InterceptionFlag = false;

            kernel = base.CreateDefaultInterceptionKernel();
        }
 
        [Fact]
        public void InterceptionUsingAttribute_InterceptsMethods()
        {
            kernel.Bind<IHaveInterceptAttribute>().To<HaveInterceptAttribute>();

            kernel.Get<IHaveInterceptAttribute>().DoSomething();
            
            InterceptionFlag.Should().BeTrue();
        }
 
        [Fact]
        public void InterceptionUsingAttribute_DoesNotInterceptSystemObjectMethods()
        {
            kernel.Bind<IHaveInterceptAttribute>().To<HaveInterceptAttribute>();

            kernel.Get<IHaveInterceptAttribute>().GetHashCode();
            
            InterceptionFlag.Should().BeFalse();
        }

        [Fact]
        public void InterceptionUsingAttribute_DoesInterceptOverridenSystemObjectMethods()
        {
            kernel.Bind<IHaveInterceptAttribute>().To<HaveInterceptAndOverrideGetHashCodeAttribute>();

            kernel.Get<IHaveInterceptAttribute>().GetHashCode();

            InterceptionFlag.Should().BeTrue();
        }
        
        [Fact]
        public void InterceptionUsingBindingExtension_InterceptsMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttribute>().Intercept().With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().DoSomething();
            
            InterceptionFlag.Should().BeTrue();
        }

        [Fact]
        public void InterceptionUsingBindingExtension_DoesNotInterceptSystemObjectMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttribute>().Intercept().With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().GetHashCode();

            InterceptionFlag.Should().BeFalse();
        }

        [Fact]
        public void InterceptionUsingBindingExtension_DoesInterceptOverriddenSystemObjectMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttributeButOverrideGetHashCode>().Intercept().With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().GetHashCode();

            InterceptionFlag.Should().BeTrue();
        }
        
        [Fact]
        public void InterceptionUsingBindingExtension_WithInterceptAllMethodsPredicate_DoesInterceptSystemObjectMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttribute>()
                .Intercept(mi => true).With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().GetHashCode();

            InterceptionFlag.Should().BeTrue();
        }

        [Fact]
        public void InterceptionUsingBindingExtension_WithInterceptNothingPredicate_DoesNotInterceptMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttribute>()
                .Intercept(mi => false).With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().DoSomething();

            InterceptionFlag.Should().BeFalse();
        }
        
        [Fact]
        public void InterceptionUsingKernelExtension_InterceptsMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttribute>();
            kernel
                .Intercept(ctx => typeof(IHaveNoInterceptAttribute).IsAssignableFrom(ctx.Plan.Type))
                .With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().DoSomething();
            InterceptionFlag.Should().BeTrue();
        }

        [Fact]
        public void InterceptionUsingKernelExtension_DoesNotInterceptSystemObjectMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttribute>();
            kernel
                .Intercept(ctx => typeof(IHaveNoInterceptAttribute).IsAssignableFrom(ctx.Plan.Type))
                .With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().GetHashCode();
            InterceptionFlag.Should().BeFalse();
        }

        [Fact]
        public void InterceptionUsingKernelExtension_DoesInterceptOverriddenSystemObjectMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttributeButOverrideGetHashCode>();
            kernel
                .Intercept(ctx => typeof(IHaveNoInterceptAttribute).IsAssignableFrom(ctx.Plan.Type))
                .With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().GetHashCode();
            InterceptionFlag.Should().BeTrue();
        }

        [Fact]
        public void InterceptionUsingKernelExtension_WithInterceptAllMethodsPredicate_DoesInterceptSystemObjectMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttribute>();
            kernel
                .Intercept(
                    ctx => typeof(IHaveNoInterceptAttribute).IsAssignableFrom(ctx.Plan.Type),
                    mi => true
                )
                .With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().GetHashCode();
            InterceptionFlag.Should().BeTrue();
        }

        [Fact]
        public void InterceptionUsingKernelExtension_WithInterceptNothingPredicate_DoesNotInterceptMethods()
        {
            kernel.Bind<IHaveNoInterceptAttribute>().To<HaveNoInterceptAttribute>();
            kernel
                .Intercept(
                    ctx => typeof(IHaveNoInterceptAttribute).IsAssignableFrom(ctx.Plan.Type),
                    mi => false
                )
                .With<MethodInterceptor>();

            kernel.Get<IHaveNoInterceptAttribute>().DoSomething();
            InterceptionFlag.Should().BeFalse();
        }

        public interface IHaveInterceptAttribute
        {
            void DoSomething();
        }
        
        [ChangeFlag]
        public class HaveInterceptAttribute : IHaveInterceptAttribute
        {
            public void DoSomething()
            {
            }
        }

        public class HaveInterceptAndOverrideGetHashCodeAttribute : HaveInterceptAttribute
        {
            public override int GetHashCode()
            {
                return base.GetHashCode() + 1;
            }
        }

        public interface IHaveNoInterceptAttribute
        {
            void DoSomething();
        }

        public class HaveNoInterceptAttribute : IHaveNoInterceptAttribute
        {
            public virtual void DoSomething()
            {
            }
        }

        public class HaveNoInterceptAttributeButOverrideGetHashCode : HaveNoInterceptAttribute
        {
            public override int GetHashCode()
            {
                return base.GetHashCode() + 1;
            }
        }        
 
        public class ChangeFlagAttribute : InterceptAttribute
        {
            public override IInterceptor CreateInterceptor(IProxyRequest request)
            {
                return new MethodInterceptor();
            }
        }
 
        public class MethodInterceptor : IInterceptor
        {
            public void Intercept(IInvocation invocation)
            {
                InterceptionFlag = true;
                invocation.Proceed();
            }
        }
    }
}