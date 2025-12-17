# MSINS API: A Comprehensive Backend Service for Managing Institutional Content and Operations

MSINS API is a robust ASP.NET Core Web API that provides a centralized backend service for managing institutional content, user authentication, and various operational aspects. The API offers comprehensive functionality for handling media uploads, user management, event coordination, and content distribution while ensuring secure access through JWT authentication and rate limiting.

The API provides essential features including:
- JWT-based authentication and authorization with refresh token support
- File upload management with size and format validation
- Content management for banners, events, executives, and initiatives
- Rate limiting to prevent abuse
- API versioning for backward compatibility
- Comprehensive exception handling and logging
- Swagger documentation for API exploration

## Repository Structure
```
MSINS_API/
├── Authorization/                 # Custom authorization handlers and requirements
├── Configuration/                 # Application configuration and dependency injection setup
├── Controllers/                   # API endpoints organized by feature
│   ├── AboutUsController.cs      # Handles about us content management
│   ├── AuthController.cs         # Manages authentication
│   └── ...                       # Other feature-specific controllers
├── Data/                         # Database context and configurations
├── Exceptions/                   # Custom exception handling
│   ├── Handler/                  # Exception handlers for different types
│   └── HandlerClass/            # Custom exception classes
├── Models/                       # Data models
│   ├── Request/                  # Request DTOs
│   └── Response/                # Response DTOs
├── POCO/                        # Plain Old CLR Objects for configuration
├── Repositories/                # Data access layer
│   ├── Implementation/         # Concrete repository implementations
│   └── Interface/             # Repository interfaces
└── Services/                  # Business logic layer
    ├── Implementation/       # Service implementations
    └── Interface/           # Service interfaces
```

## Usage Instructions
### Prerequisites
- .NET 6.0 SDK or later
- SQL Server 2019 or later
- Visual Studio 2022 or compatible IDE
- SSL certificate for HTTPS

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd MSINS_API
```

2. Update the connection string in appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<server>;Database=<database>;Trusted_Connection=True;"
  }
}
```

3. Install dependencies and build:
```bash
dotnet restore
dotnet build
```

4. Apply database migrations:
```bash
dotnet ef database update
```

### Quick Start
1. Start the API:
```bash
dotnet run
```

2. Access Swagger documentation:
```
https://localhost:5001/swagger
```

3. Generate an authentication token:
```bash
POST /api/v1/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password"
}
```

### More Detailed Examples

1. Upload a banner:
```bash
POST /api/v1/banner
Authorization: Bearer <token>
Content-Type: multipart/form-data

{
  "bannerType": "Home",
  "bannerName": "Welcome Banner",
  "imageFile": <file>,
  "isActive": true
}
```

2. Retrieve events:
```bash
GET /api/v1/events?pageSize=10&pageIndex=1
Authorization: Bearer <token>
```

### Troubleshooting

1. JWT Token Issues
- Error: "Token validation failed"
  - Check token expiration
  - Verify issuer and audience settings
  - Debug logs: `/Logs/app-log.txt`

2. File Upload Failures
- Error: "Invalid file format"
  - Check allowed formats in service configuration
  - Verify file size limits
  - Check upload directory permissions

3. Rate Limiting
- Error: "Rate limit exceeded"
  - Default limit: 100 requests per minute
  - Wait for the cooldown period
  - Check RateLimiterSettings in configuration

## Data Flow
The API follows a layered architecture for processing requests and managing data.

```ascii
Client Request → Controller → Service Layer → Repository Layer → Database
     ↑                ↓            ↓              ↓               ↓
     └────────────Response←──Business Logic←──Data Access←───Data Storage
```

Key component interactions:
1. Controllers validate requests and handle HTTP communication
2. Services implement business logic and orchestrate operations
3. Repositories handle data access and storage
4. File uploads are processed through dedicated services
5. JWT tokens manage authentication state
6. Rate limiting middleware controls request frequency
7. Exception handlers provide consistent error responses
8. Logging captures operational data at all levels