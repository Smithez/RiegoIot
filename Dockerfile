# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore as a single step (simpler for multi-project solutions)
COPY . .

# Restore and publish the web project
RUN dotnet restore "SistemaRiegoIoT.sln"
RUN dotnet publish "DashboardRiego.Web/DashboardRiego.Web.csproj" -c Release -o /app/publish /p:TrimUnusedDependencies=true

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose default HTTP port
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Start the application
ENTRYPOINT ["dotnet", "DashboardRiego.Web.dll"]
