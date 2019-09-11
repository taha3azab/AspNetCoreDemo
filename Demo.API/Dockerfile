FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build-env
WORKDIR /src
COPY ./Demo.API.csproj Demo.API/
RUN dotnet restore Demo.API/Demo.API.csproj
COPY . ./Demo.API/
WORKDIR /src/Demo.API
RUN dotnet build Demo.API.csproj -c Release -o /app


FROM build-env AS publish
RUN dotnet publish Demo.API.csproj -c Release -o /app
# Copy the created database
#COPY Demo.db ./app
RUN dotnet ef database update

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Demo.API.dll"]