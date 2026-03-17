FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# AIService doesn't need Shared project (but keeping commented for reference)
# COPY Shared/Shared.csproj Shared/
# RUN dotnet restore Shared/Shared.csproj

# Copy the project file correctly (with folder path)
COPY AIService/AIService.csproj AIService/
RUN dotnet restore AIService/AIService.csproj

# Copy everything else
COPY AIService/ AIService/
# Only copy Shared if needed (uncomment if required)
# COPY Shared/ Shared/

# Build and publish
WORKDIR /src/AIService
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AIService.dll"]