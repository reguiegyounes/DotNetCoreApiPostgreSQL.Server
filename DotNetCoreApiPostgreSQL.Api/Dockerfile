#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DotNetCoreApiPostgreSQL.Api/DotNetCoreApiPostgreSQL.Api.csproj", "DotNetCoreApiPostgreSQL.Api/"]
RUN dotnet restore "DotNetCoreApiPostgreSQL.Api/DotNetCoreApiPostgreSQL.Api.csproj"
COPY . .
WORKDIR "/src/DotNetCoreApiPostgreSQL.Api"
RUN dotnet build "DotNetCoreApiPostgreSQL.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetCoreApiPostgreSQL.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetCoreApiPostgreSQL.Api.dll"]