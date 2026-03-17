FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# AIService might need Shared? If yes, uncomment these lines:
# COPY Shared/Shared.csproj Shared/
# RUN dotnet restore Shared/Shared.csproj

COPY AIService.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AIService.dll"]