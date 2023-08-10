#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MassTransitPoc.Api/MassTransitPoc.Api.csproj", "MassTransitPoc.Api/"]
RUN dotnet restore "MassTransitPoc.Api/MassTransitPoc.Api.csproj"
COPY . .
WORKDIR "/src/MassTransitPoc.Api"
RUN dotnet build "MassTransitPoc.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransitPoc.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MassTransitPoc.Api.dll"]