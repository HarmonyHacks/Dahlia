pushd "%~dp0"

%FrameworkDir%\%FrameworkVersion%\msbuild build.csproj

popd
