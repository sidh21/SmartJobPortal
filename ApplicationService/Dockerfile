FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy Shared project first
COPY Shared/Shared.csproj Shared/
RUN dotnet restore Shared/Shared.csproj

# Copy ApplicationService project
COPY ApplicationService/ApplicationService.csproj ApplicationService/
RUN dotnet restore ApplicationService/ApplicationService.csproj

# Copy all source code
COPY Shared/ Shared/
COPY ApplicationService/ ApplicationService/

# Build and publish
WORKDIR /src/ApplicationService
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApplicationService.dll"]