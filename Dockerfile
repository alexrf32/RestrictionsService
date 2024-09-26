# Imagen base para el entorno de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen base para el entorno de desarrollo y construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RestrictionsService.csproj", "./"]
RUN dotnet restore "RestrictionsService.csproj"

COPY . .
WORKDIR "/src/"
RUN dotnet build "RestrictionsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestrictionsService.csproj" -c Release -o /app/publish

# Imagen final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestrictionsService.dll"]
