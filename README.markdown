ChannelProxies can be intercepted now:    
	
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
