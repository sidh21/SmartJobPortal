FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["JobService.csproj", "."]
RUN dotnet restore

# Copy everything else and publish
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime image - FIXED for Render
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# REMOVED: EXPOSE 80 - Render provides dynamic port
# REMOVED: ENV ASPNETCORE_URLS=http://+:80 - Will use PORT env var

ENTRYPOINT ["dotnet", "JobService.dll"]