# Ninject.Extensions.Interception

[![NuGet Version](http://img.shields.io/nuget/v/Ninject.Extensions.Interception.svg?style=flat)](https://www.nuget.org/packages/Ninject.Extensions.Interception/) 
[![Build status](https://ci.appveyor.com/api/projects/status/so14qvg5mim275pf?svg=true)](https://ci.appveyor.com/project/scott-xu/ninject-extensions-interception)
[![codecov](https://codecov.io/gh/ninject/Ninject.Extensions.Interception/branch/master/graph/badge.svg)](https://codecov.io/gh/ninject/Ninject.Extensions.Interception)

ChannelProxies can be intercepted now:    
```C#
    [ServiceContract]
    public interface IFooService
    {
        [OperationContract]
        void Foodo();
    }

    ActionInterceptor interceptor =
        new ActionInterceptor( invocation => Console.WriteLine("Executing {0}.", invocation.Request.Method) );

    kernel.Bind<IFooService>()
        .ToMethod(context => ChannelFactory<IFooService>.CreateChannel(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost/FooService")))
        .Intercept(typeof(ICommunicationObject))
        .With(interceptor);
```