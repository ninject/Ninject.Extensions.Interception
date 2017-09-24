# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Support NETSTANDARD2.0

### Removed
- Support for .NET 3.5 .NET 4.0 and Silverlight

## [3.2.0]

### Added
- Interface Proxies allow to intercept "System.Object" methods (ToString, GetHashCode, Equals) now.
- AsyncInterceptor as base class for interceptors that support interception of async methods.
- `Intercept<T>()` can now be used after `OnActivation()` syntax.
- Proxies can now implement additional interfaces. `Intercept<T>()` is now `Intercept<T>(params Type[] additionalInterfaces)`.

### Fixed
- #26: Wrong interceptor is chosen when multiple bindings exist for same service type
- Don't crash if there's a special method that is less than 4 characters long, some obvuscated code has this
- #15: Methods from derived classes can be invoked without getting an Interface not found exception

## [3.0.0.0]

### Added
- Support for out and ref values on the intercepted methods

### Changed
- Interface Proxies are created for bound interfaces rather than a class proxy for its implementation.

### Removed
- No web builds. All builds are have not reference to web anymore

### Fixed
- Fixing multithreading issue with advice registry. Updating nuget. Fixing fluent assertions build task.


## [2.2.1.0]

### Fixed
- An AmbiguousMatchExcpetion was trown for classes that have multiple properties with the same name.