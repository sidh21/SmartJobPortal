FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file with correct path (including folder)
COPY Gateway/Gateway.csproj Gateway/
RUN dotnet restore Gateway/Gateway.csproj

# Copy everything else
COPY Gateway/ Gateway/

# Build and publish
WORKDIR /src/Gateway
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]