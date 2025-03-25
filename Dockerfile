# Esta fase é usada para compilar o projeto Blazor
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o arquivo .csproj do Blazor e restaura as dependências
COPY ["FlexPro.Client/FlexPro.Client.csproj", "FlexPro.Client/"]
RUN dotnet restore "./FlexPro.Client/FlexPro.Client.csproj"

# Copia o restante do código
COPY . ./
WORKDIR "/src/FlexPro.Client"

# Compila o projeto Blazor WASM
RUN dotnet build "./FlexPro.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o Blazor WASM
FROM build AS publish
RUN dotnet publish "./FlexPro.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase serve os arquivos Blazor WASM com o Nginx
FROM nginx:alpine AS final
# Copia os arquivos estáticos gerados pela publicação para o diretório do nginx
COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html

# Expõe a porta 80 para o nginx
EXPOSE 80

# Comando para iniciar o Nginx e servir a aplicação Blazor WASM
CMD ["nginx", "-g", "daemon off;"]
