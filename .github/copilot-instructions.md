# AI Coding Agent Instructions for WebApiDemo

## Project Overview
**WebApiDemo** is a .NET 8 Web API for stock portfolio management with JWT authentication. Users can manage stock holdings in portfolios, add comments to stocks, and perform CRUD operations with role-based access control (Admin/User roles).

## Architecture & Data Flow

### Core Components
- **Models** ([Models/](Models/)): Domain entities - `Stock`, `Comment`, `AppUser` (extends IdentityUser), and `Portfolio` (junction table)
- **Repositories** ([Repositorys/](Repositorys/)): Data access layer implementing repository pattern - one interface per entity
- **Services** ([Service/](Service/)): Business logic - currently `TokenService` for JWT generation
- **Controllers** ([Controllers/](Controllers/)): 4 API endpoints: `StockController`, `CommentController`, `PortfolioController`, `AccountController`
- **Mappers** ([Mappers/](Mappers/)): Extension methods for DTO conversion (e.g., `Model.ToStockDto()`)

### Database & Relationships
- **DbContext**: [ApplicationDbContext.cs](Data/ApplicationDbContext.cs) extends `IdentityDbContext<AppUser>` for user management
- **Portfolio**: Composite key entity linking AppUser ↔ Stock (many-to-many relationship)
- **Comments**: Child of Stock; navigation property loaded via `.Include(c => c.Comments)` in repository queries
- **SQL Server** connection configured in appsettings.json (`conStr` connection string)

## Critical Patterns

### DTO & Mapper Convention
All API endpoints use Data Transfer Objects (Dtos/) and extension method mappers:
- **Model → DTO**: `stock.ToStockDto()` (in [Mappers/StockMappers.cs](Mappers/StockMappers.cs))
- **DTO → Model**: `createStockRequestDto.ToStockFromCreateDto()`
- Apply same pattern when adding new models: create DTO folder, implement mapper methods

### Repository Pattern
- All data queries must use repository interfaces (not direct DbContext in controllers)
- Async method naming: `GetByIdAsync()`, `GetAllAsync()`, `CreateAsync()`, etc.
- Example: [Repositorys/StockRepository.cs](Repositorys/StockRepository.cs) includes `.Include()` for related entities
- Always check `Task<T?>` return types for null scenarios

### Authentication & Authorization
- **JWT-based**: Configured in [Program.cs](Program.cs) with issuer/audience from JWT config
- **[Authorize]** attribute on protected endpoints (e.g., StockController requires auth)
- **Public endpoints**: AccountController login/register have no [Authorize]
- **Role-based access**: Admin/User roles configured in `OnModelCreating` but enforcement varies by endpoint

### Entity Framework Patterns
- **Reference loop handling**: Newtonsoft configured to ignore circular references ([Program.cs](Program.cs) lines 22-25)
- **Decimal precision**: Stock financial fields use `[Column(TypeName = "decimal(18,2)")]`
- **Default values**: Models initialize collections and numeric fields to prevent null reference exceptions

## Development Workflows

### Build & Run
```bash
dotnet build
dotnet run
```
- API runs with Swagger UI enabled by default
- Debug configuration in [Properties/launchSettings.json](Properties/launchSettings.json)

### Database Migrations
- EF Core tools installed; migration files in [Migrations/](Migrations/)
- Add new migration: `dotnet ef migrations add MigrationName`
- Update database: `dotnet ef database update`

### Testing API Endpoints
- Use [WebApiDemo.http](WebApiDemo.http) file for endpoint testing
- JWT Bearer token required in Authorization header for protected endpoints

## Code Style & Conventions

- **Namespace structure**: Matches folder names (e.g., `WebApiDemo.Repositorys` for repository classes)
- **Note**: Folder name is `Repositorys` (non-standard plural) - maintain this naming
- **Extension methods**: Implement mappers as extension methods on model classes
- **Query objects**: Use `StockQueryObject` ([HelperFilter/StockQueryObject.cs](HelperFilter/StockQueryObject.cs)) for filter parameters
- **Property initialization**: Strings default to `string.Empty`, numbers to `0`/`0M`

## Key Files by Purpose

| Purpose | File |
|---------|------|
| DI & middleware config | [Program.cs](Program.cs) |
| JWT token generation | [Service/TokenService.cs](Service/TokenService.cs) |
| User authentication | [Controllers/AccountController.cs](Controllers/AccountController.cs) |
| Entity relationships | [Data/ApplicationDbContext.cs](Data/ApplicationDbContext.cs) |
| Stock CRUD ops | [Controllers/StockController.cs](Controllers/StockController.cs) + [Repositorys/StockRepository.cs](Repositorys/StockRepository.cs) |

## Dependencies
- **Entity Framework Core 8.0.22** (SQL Server provider)
- **ASP.NET Core Identity 8.0.22** (user/role management)
- **JWT Bearer Authentication 8.0.22**
- **Newtonsoft.Json** (custom serialization settings)
- **Swagger/Swashbuckle** (API documentation)
