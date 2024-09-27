# Imagen base para el entorno de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Imagen base para el entorno de desarrollo y construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solo el archivo del proyecto y restaura dependencias
COPY ["RestrictionsService.csproj", "./"]
RUN dotnet restore "RestrictionsService.csproj"

# Copia todos los archivos del proyecto
COPY . .

# Construye el proyecto
RUN dotnet build "RestrictionsService.csproj" -c Release -o /app/build

# Publica el proyecto
FROM build AS publish
RUN dotnet publish "RestrictionsService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "RestrictionsService.dll"]
