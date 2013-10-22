namespace Ninject.Extensions.Interception.ProxyFactory
{
    using System;
    using System.Reflection;

    using Castle.DynamicProxy;

    public class ObjectMethodsInvocation : IInvocation
    {
        public ObjectMethodsInvocation(object proxy, object target, MethodInfo method, object[] arguments)
        {
            this.Proxy = proxy;
            this.InvocationTarget = target;
            this.Method = method;
            this.MethodInvocationTarget = method;
            this.Arguments = arguments;
        }

        public object GetArgumentValue(int index)
        {
            throw new NotImplementedException();
        }

        public MethodInfo GetConcreteMethod()
        {
            return this.Method;
        }

        public MethodInfo GetConcreteMethodInvocationTarget()
        {
            throw new NotImplementedException();
        }

        public void Proceed()
        {
            this.Method.Invoke(this.InvocationTarget, this.Arguments);
        }

        public void SetArgumentValue(int index, object value)
        {
            throw new NotImplementedException();
        }

        public object[] Arguments { get; private set; }
        public Type[] GenericArguments { get { return new Type[0]; } }
        public object InvocationTarget { get; private set; }
        public MethodInfo Method { get; private set; }
        public MethodInfo MethodInvocationTarget { get; private set; }
        public object Proxy { get; private set; }
        public object ReturnValue { get; set; }
        public Type TargetType { get { return this.InvocationTarget.GetType(); } }
    }
}