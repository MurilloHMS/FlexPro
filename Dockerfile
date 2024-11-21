
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FlexPro/FlexPro.csproj", "FlexPro/"]
RUN dotnet restore "FlexPro/FlexPro.csproj"
COPY . .
WORKDIR "/src/FlexPro"
RUN dotnet build "FlexPro.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 2: Publicar o projeto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FlexPro.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 3: Criar a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlexPro.dll"]
