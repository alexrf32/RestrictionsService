# Etapa base: imagen de tiempo de ejecución de ASP.NET Core 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de construcción: imagen del SDK de .NET 8.0 para compilar el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RestrictionsService.csproj", "./"]
RUN dotnet restore "RestrictionsService.csproj"

# Copia todo el código de la aplicación al contenedor
COPY . .
WORKDIR "/src/"
RUN dotnet build "RestrictionsService.csproj" -c Release -o /app/build

# Etapa de publicación: genera los archivos necesarios para ejecutar la aplicación
FROM build AS publish
RUN dotnet publish "RestrictionsService.csproj" -c Release -o /app/publish

# Etapa final: imagen de tiempo de ejecución para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestrictionsService.dll"]
