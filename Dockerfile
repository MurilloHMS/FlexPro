FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
EXPOSE 80
EXPOSE 81
WORKDIR /app

COPY FlexPro.Client.sln ./
COPY FlexPro.Client/FlexPro.Client.csproj ./FlexPro.Client/
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/out


FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/out/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
