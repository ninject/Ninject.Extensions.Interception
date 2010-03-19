..\contrib\nant\nant.exe -help
..\contrib\nant\nant.exe -v -buildfile:Ninject.build clean %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build clean %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build package-source %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build clean %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build cdp2 "-D:build.config=release" "-D:build.platform=silverlight-3.0" "-D:build.proxy=cdp2" package-bin %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build linfu "-D:build.config=release" "-D:build.platform=silverlight-3.0" "-D:build.proxy=linfu" package-bin %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build clean %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build linfu "-D:build.config=release" "-D:build.platform=mono-2.0" "-D:build.proxy=linfu" package-bin %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build clean %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build linfu "-D:build.config=release" "-D:build.platform=net-3.5" "-D:build.proxy=linfu" package-bin %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build clean %1 %2 %3 %4 %5 %6 %7 %8
..\contrib\nant\nant.exe -v -buildfile:Ninject.build cdp2 "-D:build.config=release" "-D:build.platform=net-3.5" "-D:build.proxy=cdp2" package-bin %1 %2 %3 %4 %5 %6 %7 %8