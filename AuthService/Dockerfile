FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy Shared project (from root)
COPY Shared/Shared.csproj Shared/
RUN dotnet restore Shared/Shared.csproj

# Copy AuthService project
COPY AuthService/AuthService.csproj AuthService/
RUN dotnet restore AuthService/AuthService.csproj

# Copy all source code
COPY Shared/ Shared/
COPY AuthService/ AuthService/

# Build and publish AuthService
WORKDIR /src/AuthService
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "AuthService.dll"]