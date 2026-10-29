# ETAPA 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

# Copiar os arquivos dos projetos
COPY ["src/THMTecnologicalSolutions.Portfolio.Api/THMTecnologicalSolutions.Portfolio.Api.csproj", "src/THMTecnologicalSolutions.Portfolio.Api/"]
COPY ["src/THMTecnologicalSolutions.Portfolio.Application/THMTecnologicalSolutions.Portfolio.Application.csproj", "src/THMTecnologicalSolutions.Portfolio.Application/"]
COPY ["src/THMTecnologicalSolutions.Portfolio.Domain/THMTecnologicalSolutions.Portfolio.Domain.csproj", "src/THMTecnologicalSolutions.Portfolio.Domain/"]
COPY ["src/THMTecnologicalSolutions.Portfolio.Infrastructure/THMTecnologicalSolutions.Portfolio.Infrastructure.csproj", "src/THMTecnologicalSolutions.Portfolio.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "src/THMTecnologicalSolutions.Portfolio.Api/THMTecnologicalSolutions.Portfolio.Api.csproj"

# Copiar todo o código
COPY . .

# Publicar a aplicação
RUN dotnet publish "src/THMTecnologicalSolutions.Portfolio.Api/THMTecnologicalSolutions.Portfolio.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# ETAPA 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# Copiar somente os arquivos publicados
COPY --from=build --chown=app:app /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

# Executar como usuário sem privilégios
USER app

ENTRYPOINT ["dotnet", "THMTecnologicalSolutions.Portfolio.Api.dll"]