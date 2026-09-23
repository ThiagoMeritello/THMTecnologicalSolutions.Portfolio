# THM Technological Solutions - Portfolio

A portfolio project designed to demonstrate the development of a modern
distributed application using .NET, Angular, Docker, messaging,
authentication, cloud infrastructure and Kubernetes.

The project is being developed incrementally, with each stage introducing
new architectural and infrastructure concepts.

## Product API

The first application developed in this portfolio is a REST API responsible
for product management and stock operations.

### Technologies

- .NET 10
- ASP.NET Core Web API
- C#
- Clean Architecture principles
- FluentValidation
- Swagger / OpenAPI
- xUnit
- NSubstitute
- ASP.NET Core Integration Testing

Future stages will introduce:

- PostgreSQL
- Entity Framework Core
- Angular
- Docker
- RabbitMQ
- Kafka
- Authentication / Authorization
- AWS
- Kubernetes

## Architecture

The solution follows a layered architecture with clear separation of
responsibilities.

```text
                 ┌────────────────────┐
                 │        API         │
                 │ Controllers / HTTP │
                 └─────────┬──────────┘
                           │
                           ▼
                 ┌────────────────────┐
                 │    Application     │
                 │     Use Cases      │
                 └─────────┬──────────┘
                           │
                           ▼
                 ┌────────────────────┐
                 │       Domain       │
                 │ Entities / Rules   │
                 └────────────────────┘

                 ┌────────────────────┐
                 │   Infrastructure   │
                 │   Repositories     │
                 └─────────┬──────────┘
                           │
                           └── implements Application abstractions
```

### Domain

Contains the core business entities and business rules.

The `Product` entity protects rules such as:

- Product name is required.
- Price cannot be negative.
- Stock cannot be negative.
- Stock can only be changed through stock operations.
- Products can be activated and deactivated.
- Deletion uses soft delete.

### Application

Contains the application's use cases.

Examples:

- Create product
- Get all products
- Get product by ID
- Update product
- Deactivate product
- Add stock
- Remove stock

The Application layer depends on repository abstractions instead of
infrastructure implementations.

### Infrastructure

Contains infrastructure implementations.

The current version uses an in-memory repository:

`InMemoryProductRepository`

This is intentional for the first stage of the project.

A persistent database implementation will be introduced in a later stage
without changing the core business rules.

### API

Responsible for exposing the application through HTTP.

It contains:

- REST controllers
- Global exception handling
- Swagger / OpenAPI
- Health checks
- HTTP status code mapping

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/v1/products` | Returns all products |
| GET | `/api/v1/products/{id}` | Returns a product by ID |
| POST | `/api/v1/products` | Creates a product |
| PUT | `/api/v1/products/{id}` | Updates a product |
| DELETE | `/api/v1/products/{id}` | Deactivates a product |
| POST | `/api/v1/products/{id}/stock/add` | Adds product stock |
| POST | `/api/v1/products/{id}/stock/remove` | Removes product stock |

## HTTP Responses

The API uses appropriate HTTP status codes:

| Status | Usage |
|---|---|
| `200 OK` | Successful query/update |
| `201 Created` | Product created |
| `204 No Content` | Product deactivated |
| `400 Bad Request` | Validation or business rule violation |
| `404 Not Found` | Resource not found |
| `500 Internal Server Error` | Unexpected application error |

A successful product creation also returns a `Location` header pointing to
the newly created resource.

## Validation

Two validation levels are used.

### Request Validation

FluentValidation validates incoming requests before executing the use case.

Examples:

- Required fields
- Maximum lengths
- Negative prices
- Invalid stock quantities

### Domain Validation

The Domain independently protects business invariants.

This ensures that business rules remain valid even when the Domain is used
outside an HTTP request.

## Error Handling

The API uses centralized exception handling.

```text
ValidationException
        ↓
400 Bad Request

DomainException
        ↓
400 Bad Request

NotFoundException
        ↓
404 Not Found

Unexpected Exception
        ↓
500 Internal Server Error
```

Responses follow the ASP.NET Core Problem Details format.

## Stock Management

Stock is intentionally not directly modified through the product update
endpoint.

Instead of:

```text
PUT product
stockQuantity = 100
```

the API exposes explicit stock operations:

```text
POST /stock/add
POST /stock/remove
```

This allows stock changes to represent business operations rather than
arbitrary property updates.

The Domain also prevents stock from becoming negative.

## Soft Delete

Products are not physically removed.

Calling:

```http
DELETE /api/v1/products/{id}
```

sets:

```text
IsActive = false
```

The product remains available for auditing and future lifecycle operations.

## Testing

The solution contains three testing levels.

### Domain Tests

Tests business rules directly without external dependencies.

Examples:

- Product creation
- Negative price
- Add stock
- Remove stock
- Insufficient stock
- Activation
- Deactivation

### Application Tests

Tests use cases and application orchestration.

NSubstitute is used to mock repository dependencies.

### Integration Tests

Tests the complete HTTP flow using ASP.NET Core integration testing.

```text
HTTP Request
     ↓
Controller
     ↓
Application
     ↓
Repository
     ↓
HTTP Response
```

Integration scenarios include:

- Product creation
- Validation errors
- Product queries
- Resource not found
- Product update
- Soft delete

## Running the Project

### Requirements

- .NET 10 SDK

Clone the repository and restore dependencies:

```bash
dotnet restore
```

Build:

```bash
dotnet build
```

Run all automated tests:

```bash
dotnet test
```

Run the API:

```bash
dotnet run --project src/THMTecnologicalSolutions.Portfolio.Api
```

The application URL will be displayed in the terminal.

## Swagger

When running in the Development environment, Swagger is available at:

```text
/swagger
```

Swagger provides interactive documentation for all Product API endpoints.

## Health Check

The API exposes:

```http
GET /health
```

A healthy application returns:

```text
Healthy
```

The endpoint will later be used by container and orchestration infrastructure
such as Docker, AWS and Kubernetes.

## Current Persistence

Products are currently stored in memory.

Restarting the API removes all stored products.

This is intentional for the initial version of the portfolio.

A database will be introduced in a future stage.

## Roadmap

The portfolio will evolve incrementally.

- [x] Product Domain
- [x] Product REST API
- [x] Request validation
- [x] Global exception handling
- [x] Stock management
- [x] Soft delete
- [x] Swagger / OpenAPI
- [x] Health Check
- [x] Unit tests
- [x] Integration tests
- [ ] Docker
- [ ] AWS deployment
- [ ] PostgreSQL
- [ ] Entity Framework Core
- [ ] RabbitMQ
- [ ] Email Worker
- [ ] Angular frontend
- [ ] Authentication service
- [ ] JWT authentication
- [ ] Authorization
- [ ] Kafka
- [ ] Kubernetes
- [ ] CI/CD

## Project Goal

The goal of this repository is not only to build a functional application,
but to demonstrate the evolution of a software system from a simple REST API
into a distributed, containerized and cloud-hosted architecture.

Each stage introduces new concepts while keeping the existing architecture
maintainable and testable.