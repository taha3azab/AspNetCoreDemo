FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore



# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Copy the created database
RUN cp ./Demo.db ./out/Demo.db
RUN dotnet ef database update

# Ensure that we generate and migrate the database 
# WORKDIR /app/out
# RUN dotnet ef database update

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Demo.API.dll"]