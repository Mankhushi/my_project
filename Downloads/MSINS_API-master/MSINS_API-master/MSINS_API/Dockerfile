# Use the official ASP.NET Core 8 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

RUN mkdir -p /app/consultation

# Use the official .NET SDK 8 image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["MSINS_API.csproj", "./"]
RUN dotnet restore "./MSINS_API.csproj"

# Copy the rest of the application files
COPY . .

# Build the application
RUN dotnet build "MSINS_API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MSINS_API.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "MSINS_API.dll"]
