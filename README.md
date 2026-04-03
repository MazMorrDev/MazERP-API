# MazERP - Enterprise Resource Planning API

This project is a RESTful API developed with .NET Core as a practical exercise to demonstrate my backend development skills. The system implements a basic ERP with essential functionalities for business management.

## 🛠️ Technologies and Tools Used

- **.NET Core 10.0** - Main framework for API development
- **Entity Framework Core** - ORM for database management
- **PostgreSQL** - Relational database management system
- **JWT (JSON Web Tokens)** - Secure authentication and authorization
- **CORS** - Access policy configuration from different origins
- **xUnit** - Unit testing framework
- **Swagger/OpenAPI** - Automatic API documentation
- **DotNetEnv** - Environment variable management

## 🏗️ Architecture and Design Patterns Implemented

### Layered Architecture

- **Controllers**: HTTP request and route handling
- **Services**: Encapsulated business logic
- **Repositories**: Data access abstraction (implicit in services)
- **Models**: Entities representing database tables
- **DTOs (Data Transfer Objects)**: Objects for data transfer between layers
- **Mappers**: Conversion between entities and DTOs
- **Enums**: Enumerated types for fixed values
- **Middleware**: Custom components for the ASP.NET Core pipeline

### SOLID Principles and Best Practices

- **Separation of concerns**: Each layer has a well-defined purpose
- **Dependency injection**: Centralized dependency management via DI container
- **Dependency inversion principle**: Dependencies are abstracted through interfaces
- **DRY (Don't Repeat Yourself)**: Code reuse through services and mappers
- **Data validation**: Use of validation attributes in DTOs

## 🔑 Features and Implemented Functionalities

### Authentication and Authorization

- User registration and login with password encryption
- JWT token generation and validation
- Custom middleware for role-based authorization
- Endpoint protection using `[Authorize]` attributes

### Company Management

- Complete CRUD operations for companies
- User association to companies with different roles
- Company-by-user queries and vice versa

### Inventory Management

- Product, warehouse, and stock control
- Inventory movements (inbound and outbound)
- Product assignment to points of sale
- Batch control and basic traceability

### Sales and Purchases Management

- Sales registration with different types and payment methods
- Supplier purchase management
- Product returns with defined statuses and actions
- Categorized operational expenses

### Other Modules

- Supplier management
- Point of sale control
- Financial movement history
- Basic inventory and sales reports

## 📊 Project Structure

```text
MazERP-API-Enterprise Resource Planning API/
├── API/
│   ├── Controllers/          # RESTful endpoints
│   ├── Services/             # Business logic
│   │   ├── Implementation/   # Service implementations
│   │   └── Interfaces/       # Service contracts
│   ├── Models/               # Database entities
│   ├── DTOs/                 # Data transfer objects
│   ├── Utils/                # Utilities and helpers
│   │   ├── Mappers/          # Entity-DTO mapping
│   │   └── PaginatedResult.cs# Generic paginated result
│   ├── Config/               # Application configuration
│   ├── Middleware/           # Custom middleware
│   ├── Enums/                # Enumerations
│   ├── Context/              # Entity Framework context
│   └── Migrations/           # Database migrations
│
├── MazERP Integration Tests/ # Integration tests with Bruno
│   ├── collections/          # API test collections
│   │   ├── Company/          # Company management tests
│   │   ├── Users/            # Authentication and user tests
│   │   ├── Suppliers/        # Supplier management tests
│   │   ├── Products/         # Product management tests
│   │   ├── Inventory/        # Inventory management tests
│   │   ├── SellPoints/       # Point of sale tests
│   │   ├── Sells/            # Sales tests
│   │   ├── Buys/             # Purchase tests
│   │   └── Warehouses/       # Warehouse tests
│   ├── environments/         # Test environments (dev/production)
│   │   └── GlobalTestEnv.yml # Global environment variables
│   └── workspace.yml         # Bruno workspace configuration
│
├── UnitTests/                # Unit testing project (xUnit)
│   ├── UnitTest1.cs          # Unit test file (in development)
│   └── UnitTests.csproj      # Test project
│
├── MazErpAPI.sln             # Visual Studio solution
└── README.md                 # This file
```

## 🧪 Code Quality and Testing

- **Unit Tests**: Separate project for business logic testing
- **Code coverage**: Focus on testing critical cases and edge cases
- **Integration Tests**: Postman/Bruno collections to validate endpoints
- **Error handling**: Consistent responses and appropriate HTTP status codes
- **Logging**: Basic implementation for event tracking

## 🚀 How to Run the Project

### Prerequisites

- .NET SDK 10.0 or higher
- PostgreSQL 12 or higher
- Git

### Steps to Run

1. Clone the repository:

   ```bash
   git clone <repository-url>
   cd MazERP-API
   ```

2. Configure environment variables:

   ```bash
   cp .env.example .env
   # Edit .env with your settings
   ```

3. Apply database migrations:

   ```bash
   dotnet ef database update
   ```

4. Run the application:

   ```bash
   dotnet run --project API/MazErpAPI.csproj
   ```

5. Access Swagger documentation:

   <http://localhost:5148/swagger>

## 🎯 Demonstrated Learning Objectives

This project demonstrates proficiency in:

1. **RESTful API development** with design best practices
2. **Secure authentication and authorization** implementation
3. **Database management** with Entity Framework Core (migrations, queries, relationships)
4. **Software architecture** separating concerns into distinct layers
5. **Dependency injection** and use of service containers
6. **Object mapping** between layers using DTO and Mapper patterns
7. **Data validation** in both input and business logic
8. **Error handling** and appropriate HTTP responses
9. **Environment configuration** for different environments (development/production)
10. **Software testing** both unit and integration
11. **API documentation** using Swagger/OpenAPI
12. **Use of enumerations** for domain-specific values
13. **Custom middleware implementation** for cross-cutting functionalities
14. **Project management** with Visual Studio solution and multiple projects

## 📝 Additional Notes

This project was developed as a learning exercise and practice to consolidate backend development knowledge with .NET Core. Although it implements complete functionalities of a basic ERP, it is primarily designed for educational purposes to demonstrate:

- Understanding of advanced object-oriented programming concepts
- Application of architectural design patterns
- Secure authentication and authorization handling
- Best practices in professional API development
- Organization and structuring of medium-complexity projects

The code follows C# and .NET naming conventions, with explanatory comments in the more complex sections and a clear structure that facilitates maintenance and future extension.

---

*Developed as part of my backend development skills portfolio.*.
