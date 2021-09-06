#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["health-check-aspnet-core/health-check-aspnet-core.csproj", "health-check-aspnet-core/"]
RUN dotnet restore "health-check-aspnet-core/health-check-aspnet-core.csproj"
COPY . .
WORKDIR "/src/health-check-aspnet-core"
RUN dotnet build "health-check-aspnet-core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "health-check-aspnet-core.csproj" -c Release -o /app/publish

FROM base AS final
# To have working always working swagger UI we have to always be in Development mode:
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "health-check-aspnet-core.dll"]