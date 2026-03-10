# Trading Platform - Clean Architecture Documentation

## Overview

This project follows **Clean Architecture** and **Domain-Driven Design (DDD)** principles with strict separation of concerns.

## Project Structure

```
TradingEngineAPI/
├── TradingEngineApi/                 # API Layer (Composition Root)
│   ├── Program.cs    # Startup configuration
│   ├── Controllers/       # HTTP endpoints
│   ├── appsettings.json              # Configuration
│   └── TradingPlatform.csproj    # API project file
│
├── TradingPlatform.Application/      # Application Layer (Use Cases)
│   ├── DependencyInjection.cs        # Service registration
│   ├── Interfaces/          # Service contracts
│   │   ├── IRepository.cs            # Generic repository interface
│   │   └── IUnitOfWork.cs # Unit of Work pattern
│   ├── Services/    # Business logic / Use cases
│   ├── Queries/           # CQRS queries (optional)
│   └── Commands/       # CQRS commands (optional)
│
├── TradingPlatform.Infrastructure/   # Infrastructure Layer (Implementation)
│   ├── DependencyInjection.cs   # Service registration
│   ├── Persistence/
│   │   ├── TradingDbContext.cs       # EF Core DbContext
│   │   ├── Repositories/    # Repository implementations
│   │   ├── UnitOfWork/         # Unit of Work implementation
│   │   ├── Configurations/           # Entity configurations (IEntityTypeConfiguration)
│   │   └── Migrations/               # EF Core migrations
│   └── TradingPlatform.Infrastructure.csproj
│
└── TradingPlatform.Domain/  # Domain Layer (Business Logic)
    ├── Entities/     # Domain entities (Aggregates)
 ├── Common/
    │   └── BaseEntity.cs             # Base class for all entities
    ├── Enums/                 # Domain enums
    ├── Events/        # Domain events (optional)
    └── TradingPlatform.Domain.csproj
```

## Dependency Flow

```
API (Composition Root)
 ↓
Application (Use Cases & Interfaces)
 ↓
Infrastructure (Implementations) → Domain (Business Logic)
```

**Rules:**
- **Domain**: No dependencies (pure business logic)
- **Application**: Only depends on Domain (defines contracts)
- **Infrastructure**: Implements Application interfaces, depends on Domain
- **API**: References Application and Infrastructure, orchestrates DI

## Key Components

### 1. Domain Layer

**Purpose**: Define pure business logic and entities.

- **BaseEntity**: Abstract base class for all domain entities
  - `Id`: Unique identifier (Guid)
  - `CreatedAt`: Timestamp of creation
  - `UpdatedAt`: Timestamp of last modification

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

**Files**: `TradingPlatform.Domain/Common/BaseEntity.cs`

### 2. Application Layer

**Purpose**: Define contracts and orchestrate business use cases.

#### IRepository<TEntity>

Generic repository interface for data access operations.

```csharp
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

#### IUnitOfWork

Manages transactions across multiple repositories.

```csharp
public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

**Files**:
- `TradingPlatform.Application/Interfaces/IRepository.cs`
- `TradingPlatform.Application/Interfaces/IUnitOfWork.cs`
- `TradingPlatform.Application/DependencyInjection.cs`

### 3. Infrastructure Layer

**Purpose**: Implement data access, external services, and technical concerns.

#### TradingDbContext

Entity Framework Core DbContext that manages all database operations.

```csharp
public class TradingDbContext : DbContext
{
    public TradingDbContext(DbContextOptions<TradingDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingDbContext).Assembly);
    }
}
```

#### Repository<TEntity>

Generic implementation of IRepository using EF Core.

**Files**:
- `TradingPlatform.Infrastructure/Persistence/TradingDbContext.cs`
- `TradingPlatform.Infrastructure/Persistence/Repositories/Repository.cs`
- `TradingPlatform.Infrastructure/Persistence/UnitOfWork/UnitOfWork.cs`
- `TradingPlatform.Infrastructure/Persistence/Configurations/` (Entity configurations)

### 4. API Layer

**Purpose**: HTTP endpoints and composition root (dependency injection setup).

**Program.cs** orchestrates dependency injection:

```csharp
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
```

## Dependency Injection Setup

### Application Layer Registration

```csharp
// TradingPlatform.Application/DependencyInjection.cs
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    // Register MediatR for CQRS pattern
    // services.AddMediatR(typeof(DependencyInjection).Assembly);
    
  return services;
}
```

### Infrastructure Layer Registration

```csharp
// TradingPlatform.Infrastructure/DependencyInjection.cs
public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
{
    // Register DbContext
    services.AddDbContext<TradingDbContext>(options =>
        options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"),
   sqlOptions => sqlOptions.MigrationsAssembly("TradingPlatform.Infrastructure")));

    // Register Unit of Work pattern
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Register generic repository
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    return services;
}
```

## Entity Configuration Pattern

Each entity should have a corresponding configuration using `IEntityTypeConfiguration<T>`:

```csharp
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
  builder.Property(o => o.Id).HasDefaultValueSql("NEWID()");
        builder.Property(o => o.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        
        // Additional mappings...
    }
}
```

**Location**: `TradingPlatform.Infrastructure/Persistence/Configurations/`

All configurations are automatically discovered and applied via `modelBuilder.ApplyConfigurationsFromAssembly()`.

## Database Migrations

### Creating Migrations

```bash
cd TradingEngineApi
dotnet ef migrations add MigrationName --project ../TradingPlatform.Infrastructure
```

### Applying Migrations

```bash
cd TradingEngineApi
dotnet ef database update --project ../TradingPlatform.Infrastructure
```

### Configuration

- **Migrations Assembly**: `TradingPlatform.Infrastructure`
- **DbContext**: `TradingDbContext`
- **Connection String**: Configured in `appsettings.json`

## Usage Example

### Creating an Entity

```csharp
// In TradingPlatform.Domain/Entities/Order.cs
public class Order : BaseEntity
{
    public string Symbol { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
 public OrderStatus Status { get; set; }
}
```

### Creating an Entity Configuration

```csharp
// In TradingPlatform.Infrastructure/Persistence/Configurations/OrderConfiguration.cs
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
  builder.HasKey(o => o.Id);
        builder.Property(o => o.Symbol).IsRequired().HasMaxLength(10);
        builder.Property(o => o.Quantity).HasPrecision(18, 8);
     builder.Property(o => o.Price).HasPrecision(18, 8);
    }
}
```

### Creating a Repository Interface

```csharp
// In TradingPlatform.Application/Interfaces/IOrderRepository.cs
public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);
}
```

### Implementing a Repository

```csharp
// In TradingPlatform.Infrastructure/Persistence/Repositories/OrderRepository.cs
public class OrderRepository : Repository<Order>, IOrderRepository
{
public OrderRepository(TradingDbContext context) : base(context) { }

    public async Task<IEnumerable<Order>> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(o => o.Symbol == symbol).ToListAsync(cancellationToken);
    }
}
```

### Using in Application Service

```csharp
// In TradingPlatform.Application/Services/OrderService.cs
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
  _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
  {
        var order = new Order
      {
      Symbol = request.Symbol,
 Quantity = request.Quantity,
Price = request.Price
        };

        var repository = _unitOfWork.GetRepository<Order>();
        await repository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order;
    }
}
```

### Using in Controller

```csharp
// In TradingEngineApi/Controllers/OrdersController.cs
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
  {
   _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _orderService.CreateOrderAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }
}
```

## Best Practices

1. **Keep entities pure**: No business logic in entities, only data properties
2. **Use value objects**: Encapsulate complex properties
3. **Aggregate boundaries**: Design aggregate roots for transaction consistency
4. **Repository pattern**: Always access data through repositories, not DbContext
5. **Unit of Work**: Use for transactional consistency across multiple operations
6. **Entity configuration**: Use `IEntityTypeConfiguration` for centralized mapping
7. **Migrations**: Create migrations for every schema change
8. **Testing**: Mock repositories and Unit of Work for unit tests
9. **Cancellation tokens**: Always support CancellationToken for async operations
10. **Null safety**: Use nullable reference types for better null checking

## Configuration Files

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TradingDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Project References

**API → Application** (Composition Root)
**API → Infrastructure** (Composition Root)
**Application → Domain** (Use Cases)
**Infrastructure → Application** (Implements)
**Infrastructure → Domain** (Business Logic)

## Next Steps

1. **Define domain entities** in `TradingPlatform.Domain/Entities/`
2. **Create entity configurations** in `TradingPlatform.Infrastructure/Persistence/Configurations/`
3. **Create specialized repositories** extending `IRepository<T>`
4. **Implement application services** using repositories and Unit of Work
5. **Create controllers** using application services
6. **Create initial migration**: `dotnet ef migrations add InitialCreate`
7. **Run migration**: `dotnet ef database update`