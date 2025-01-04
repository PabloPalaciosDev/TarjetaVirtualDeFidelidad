# Imagen base de ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen base de SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SistemaDeFidelidad.csproj", "./"]
RUN dotnet restore "SistemaDeFidelidad.csproj"
COPY . .
RUN dotnet publish -c Release -o /app

# Copia los archivos compilados a la imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "SistemaDeFidelidad.dll"]
