# Zalmo Project Architecture

## Project Overview
Windows desktop app for epub/pdf reading built with .NET

## Folder Structure

```
zalmo/
├── src/
│   ├── Zalmo.Core/                    # Core business logic & domain
│   ├── Zalmo.Infrastructure/          # Data access & external services
│   ├── Zalmo.UI/                      # Desktop UI (.NET MAUI)
│   ├── Zalmo.Tests/
│   │   └── Zalmo.Infrastructure.Tests # Infrastructure unit tests
│   └── Zalmo.Application.sln          # Solution file
├── .gitignore
└── README.md
```

## Architecture Pattern
- **Monolith** with layered architecture (Core, Infrastructure, UI)
- **MVVM** for Desktop Client (MAUI)
- **Dependency Injection** for service management

## Key Layers

### Core (Zalmo.Core)
- Domain entities and business logic
- Pure business rules independent of infrastructure

### Infrastructure (Zalmo.Infrastructure)
- Data access layer
- External service integrations
- Repository patterns

### UI (Zalmo.UI)
- .NET MAUI Desktop application
- MVVM pattern implementation
- User interface components

### Tests (Zalmo.Tests)
- Unit tests for infrastructure layer
- Test data generation (AutoFixture)
- Mock dependencies (NSubstitute)

## Technology Stack
- **.NET 10**
- **Minimal API** for backend
- **EF Core** for database queries
- **.NET MAUI** for Desktop UI
- **JSON file** as database
- **Microsoft.Extension.Logging** for structured logging
