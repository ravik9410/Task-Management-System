# Use the official .NET 6.0 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK 6.0 to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["UserManagementService/UserManagementService.csproj", "UserManagementService/"]
RUN dotnet restore "UserManagementService/UserManagementService.csproj"

# Copy the entire project and build it
COPY . .
WORKDIR "/src/UserManagementService"
RUN dotnet build "UserManagementService.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "UserManagementService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagementService.dll"]
