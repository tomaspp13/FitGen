FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FitGen.API/FitGen.API.csproj", "FitGen.API/"]
COPY ["FitGen.Application/FitGen.Aplicacion.csproj", "FitGen.Application/"]
COPY ["FitGen.Domain/FitGen.Dominio.csproj", "FitGen.Domain/"]
COPY ["FitGen.Infrastructure/FitGen.Infraestructura.csproj", "FitGen.Infrastructure/"]

RUN dotnet restore "FitGen.API/FitGen.API.csproj"

COPY . .
RUN dotnet publish "FitGen.API/FitGen.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FitGen.API.dll"]