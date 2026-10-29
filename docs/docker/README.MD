# Docker — THMTecnologicalSolutions.Portfolio

## Visão geral

Este projeto utiliza Docker para executar uma Product API desenvolvida em ASP.NET Core 10, seguindo princípios de Clean Architecture.

A aplicação é executada em um container Linux e gerenciada por Docker Compose.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Docker
- Docker Compose
- Multi-stage build
- Health Checks

## Pré-requisitos

- Docker Desktop instalado e em execução
- Docker Compose disponível

Não é necessário instalar o SDK .NET na máquina para executar a aplicação via Docker.

## Configuração

Na raiz do repositório, copie o arquivo `.env.example` para `.env`.

No PowerShell:

```powershell
Copy-Item .env.example .env
```

Configure as variáveis:

```dotenv
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_HTTP_PORTS=8080
API_EXTERNAL_PORT=8090
```

## Construir e executar

Na raiz do projeto:

```powershell
docker compose up -d --build
```

## Acessar a API

Swagger (Development):

http://localhost:8090/swagger

Health Check:

http://localhost:8090/health

A porta externa depende da variável `API_EXTERNAL_PORT`.

## Comandos úteis

Listar containers:

```powershell
docker compose ps
```

Visualizar logs:

```powershell
docker compose logs -f product-api
```

Parar os serviços:

```powershell
docker compose stop
```

Iniciar novamente:

```powershell
docker compose start
```

Parar e remover os containers e redes do Compose:

```powershell
docker compose down
```

Reconstruir após alterações:

```powershell
docker compose up -d --build
```

Monitorar recursos:

```powershell
docker stats thm-portfolio-api
```

## Segurança e otimização

A aplicação utiliza:

- Multi-stage build para separar compilação e execução.
- Imagem oficial do ASP.NET Core Runtime.
- Usuário `app`, sem privilégios de root.
- `.dockerignore` para reduzir o contexto de build.
- Limites de CPU e memória no Docker Compose.
- Configurações por variáveis de ambiente.

## Health Check

A API disponibiliza o endpoint `/health`, que retorna `Healthy` quando a verificação básica está funcionando.

Atualmente, essa verificação não inclui dependências externas, como banco de dados ou mensageria.

## Próximas evoluções

- Publicação na AWS.
- Persistência com PostgreSQL.
- Mensageria com RabbitMQ.
- Frontend Angular.
- Autenticação centralizada.
- Kubernetes.
- CI/CD.
