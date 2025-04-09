# Use the ASP.NET 8 runtime image for the base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Kalakobana.Auth.API/Kalakobana.Auth.API.csproj", "./"]
RUN dotnet restore "./Kalakobana.Auth.API.csproj"

# Copy the rest of the source code and build the application
COPY . .
WORKDIR "/src/Kalakobana.Auth.API"
RUN dotnet build "Kalakobana.Auth.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Kalakobana.Auth.API.csproj" -c Release -o /app/publish

# Use the base image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kalakobana.Auth.API.dll"]