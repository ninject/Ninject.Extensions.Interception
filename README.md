# Ninject.Extensions.Interception

[![Build status](https://ci.appveyor.com/api/projects/status/nj6mp01qeem166y8?svg=true)](https://ci.appveyor.com/project/Ninject/ninject-extensions-interception)
[![codecov](https://codecov.io/gh/ninject/Ninject.Extensions.Interception/branch/master/graph/badge.svg)](https://codecov.io/gh/ninject/Ninject.Extensions.Interception)
[![NuGet Version](http://img.shields.io/nuget/v/Ninject.Extensions.Interception.svg?style=flat)](https://www.nuget.org/packages/Ninject.Extensions.Interception/) 
[![NuGet Download](http://img.shields.io/nuget/dt/Ninject.Extensions.Interception.svg?style=flat)](https://www.nuget.org/packages/Ninject.Extensions.Interception/) 

This extension adds support for interception to Ninject.

For example, the WCF client proxy can be intercepted as below.

```C#
[ServiceContract]
public interface IFooService
{
    [OperationContract]
    void Foodo();
}

var interceptor =
    new ActionInterceptor(
        invocation => Console.WriteLine($"Executing {invocation.Request.Method}."));

kernel.Bind<IFooService>()
    .ToMethod(context => ChannelFactory<IFooService>.CreateChannel("*"))
    .Intercept(typeof(ICommunicationObject))
    .With(interceptor);
```