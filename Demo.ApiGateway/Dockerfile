FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build-env
WORKDIR /src
COPY ./Demo.ApiGateway.csproj Demo.ApiGateway/
RUN dotnet restore Demo.ApiGateway/Demo.ApiGateway.csproj
COPY . ./Demo.ApiGateway/
WORKDIR /src/Demo.ApiGateway
RUN dotnet build Demo.ApiGateway.csproj -c Release -o /app

FROM build-env AS publish
RUN dotnet publish Demo.ApiGateway.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Demo.ApiGateway.dll"]