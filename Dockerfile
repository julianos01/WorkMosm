FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WorkMosm.Api/WorkMosm.Api.csproj WorkMosm.Api/
RUN dotnet restore "WorkMosm.Api/WorkMosm.Api.csproj"

COPY . .

RUN dotnet publish "WorkMosm.Api/WorkMosm.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN addgroup --system appgroup && adduser --system appuser --ingroup appgroup
USER appuser

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "WorkMosm.Api.dll"]

