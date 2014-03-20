Version 3.0.2
-------------
- Bugfix: #26: Wrong interceptor is chosen when multiple bindings exist for same service type
- Bugfix: Don't crash if there's a special method that is less than 4 characters long, some obvuscated code has this
- Bugfix: #15: Methods from derived classes can be invoked without getting an Interface not found exception
- Added: Interface Proxies allow to intercept "System.Object" methods (ToString, GetHashCode, Equals) now.
- Added: AsyncInterceptor as base class for interceptors that support interception of async methods.
- Added: `Intercept<T>()` can now be used after `OnActivation()` syntax.
- Added: Proxies can now implement additional interfaces. `Intercept<T>()` is now `Intercept<T>(params Type[] additionalInterfaces)`.

Version 3.0.0.0
---------------
Added: Support for out and ref values on the intercepted methods
Removed: No web builds. All builds are have not reference to web anymore
Changed: Interface Proxies are created for bound interfaces rather than a class proxy for its implementation.
Bugfix: Fixing multithreading issue with advice registry. Updating nuget. Fixing fluent assertions build task.


Version 2.2.1.0
---------------
Bugfix: An AmbiguousMatchExcpetion was trown for classes that have multiple properties with the same name.
