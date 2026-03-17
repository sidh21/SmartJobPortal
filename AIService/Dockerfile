FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy Shared project first (AIService needs it!)
COPY Shared/Shared.csproj Shared/
RUN dotnet restore Shared/Shared.csproj

# Copy AIService project
COPY AIService/AIService.csproj AIService/
RUN dotnet restore AIService/AIService.csproj

# Copy all source code
COPY Shared/ Shared/
COPY AIService/ AIService/

# Build and publish
WORKDIR /src/AIService
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AIService.dll"]