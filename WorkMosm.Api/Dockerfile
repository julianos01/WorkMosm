# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY *.csproj ./
RUN dotnet restore

# Copiamos el resto del código y compilamos
COPY . .
RUN dotnet publish -c Release -o /app/publish

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiamos el resultado del build
COPY --from=build /app/publish .

# Puerto por defecto
EXPOSE 8080

# Variable típica para ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080

# Arranque de la app
ENTRYPOINT ["dotnet", "TuProyecto.Api.dll"]
