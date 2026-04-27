FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia tudo do contexto (a raiz do projeto) para a imagem
COPY . .

WORKDIR /src/src/GoodBurger.Api
RUN dotnet restore "GoodBurger.Api.csproj"
RUN dotnet build "GoodBurger.Api.csproj" -c Release -o /app/build
RUN dotnet publish "GoodBurger.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Dependências necessárias para PostgreSQL
RUN apt-get update && apt-get install -y \
    libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "GoodBurger.Api.dll"]