pushd src
rm -rf bin/ obj/
dotnet restore
dotnet publish -c Release -r ubuntu.16.04-x64
pushd bin/Release/netcoreapp1.1
find ubuntu.16.04-x64 | xargs zip spongebot.zip
popd
popd
mkdir -p bin
cp src/bin/Release/netcoreapp1.1/spongebot.zip bin/spongebot.zip