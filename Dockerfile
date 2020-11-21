FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY ./bin/Release/netcoreapp3.1/publish/ .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Drink4Burpee.dll