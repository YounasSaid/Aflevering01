# 🏗️ Architecture Diagram - Part 4 Complete

## System Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                        CLIENT LAYER                              │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │         Swagger UI / Browser / Postman                    │   │
│  │  https://localhost:7005/swagger                           │   │
│  └──────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
                              │
                              │ HTTP/HTTPS
                              │ JSON
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      WEB API LAYER (Port 7005)                   │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │                    Program.cs                             │   │
│  │  • Dependency Injection Configuration                     │   │
│  │  • Swagger/OpenAPI Setup                                  │   │
│  │  • Repository Registration                                │   │
│  └──────────────────────────────────────────────────────────┘   │
│                              │                                   │
│  ┌───────────────┬──────────┴───────────┬───────────────────┐   │
│  │               │                      │                    │   │
│  ▼               ▼                      ▼                    │   │
│  ┌─────────┐  ┌─────────┐          ┌──────────┐            │   │
│  │ Users   │  │ Posts   │          │Comments  │            │   │
│  │Controller│  │Controller│          │Controller│            │   │
│  └─────────┘  └─────────┘          └──────────┘            │   │
│      │             │                     │                   │   │
│      │ Uses DTOs   │ Uses DTOs          │ Uses DTOs         │   │
│      ▼             ▼                     ▼                   │   │
└──────┼─────────────┼─────────────────────┼───────────────────┘
       │             │                     │
       │    ┌────────┴────────┐           │
       │    │                 │           │
       ▼    ▼                 ▼           ▼
┌─────────────────────────────────────────────────────────────────┐
│              REPOSITORY CONTRACTS (Interfaces)                   │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────┐  │
│  │ IUserRepository  │  │ IPostRepository  │  │ICommentRepo  │  │
│  │                  │  │                  │  │              │  │
│  │ • AddAsync       │  │ • AddAsync       │  │• AddAsync    │  │
│  │ • UpdateAsync    │  │ • UpdateAsync    │  │• UpdateAsync │  │
│  │ • DeleteAsync    │  │ • DeleteAsync    │  │• DeleteAsync │  │
│  │ • GetSingleAsync │  │ • GetSingleAsync │  │• GetSingle.. │  │
│  │ • GetMany        │  │ • GetMany        │  │• GetMany     │  │
│  └──────────────────┘  └──────────────────┘  └──────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                    Implemented by
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│           FILE REPOSITORIES (Implementations)                    │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────┐  │
│  │UserFileRepository│  │PostFileRepository│  │CommentFile.. │  │
│  │                  │  │                  │  │Repository    │  │
│  │ Uses JSON        │  │ Uses JSON        │  │Uses JSON     │  │
│  │ Serialization    │  │ Serialization    │  │Serialization │  │
│  └──────────────────┘  └──────────────────┘  └──────────────┘  │
│           │                     │                     │          │
└───────────┼─────────────────────┼─────────────────────┼──────────┘
            │                     │                     │
            ▼                     ▼                     ▼
┌─────────────────────────────────────────────────────────────────┐
│                   PERSISTENCE LAYER (JSON Files)                 │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐      │
│  │ users.json   │    │ posts.json   │    │comments.json │      │
│  │              │    │              │    │              │      │
│  │ [            │    │ [            │    │ [            │      │
│  │   {          │    │   {          │    │   {          │      │
│  │     "id": 1, │    │     "id": 1, │    │     "id": 1, │      │
│  │     "user..  │    │     "title"  │    │     "body"   │      │
│  │   }          │    │   }          │    │   }          │      │
│  │ ]            │    │ ]            │    │ ]            │      │
│  └──────────────┘    └──────────────┘    └──────────────┘      │
└─────────────────────────────────────────────────────────────────┘
```

---

## Data Flow Examples

### Example 1: Create User

```
1. Client sends POST request
   POST /Users
   {
     "userName": "john",
     "password": "pass123"
   }
        │
        ▼
2. UsersController receives request
   • Creates User entity from CreateUserDto
   • Validates username is available
        │
        ▼
3. Calls IUserRepository.AddAsync(user)
        │
        ▼
4. UserFileRepository implementation
   • Reads users.json
   • Generates new ID
   • Adds user to list
   • Writes back to users.json
        │
        ▼
5. Returns User with ID
        │
        ▼
6. Controller converts to UserDto
   • Excludes password
   • Returns 201 Created
        │
        ▼
7. Client receives UserDto
   {
     "id": 1,
     "userName": "john"
   }
```

### Example 2: Get Post with Author and Comments

```
1. Client sends GET request
   GET /Posts/1?includeAuthor=true&includeComments=true
        │
        ▼
2. PostsController receives request
   • Calls postRepo.GetSingleAsync(1)
        │
        ▼
3. PostFileRepository reads posts.json
   • Finds post with id=1
   • Returns Post entity
        │
        ▼
4. Controller checks includeAuthor flag
   • Calls userRepo.GetSingleAsync(post.UserId)
   • Gets author information
        │
        ▼
5. Controller checks includeComments flag
   • Calls commentRepo.GetMany()
   • Filters by postId
   • Gets list of comments
        │
        ▼
6. Controller builds PostDto
   • Includes post data
   • Includes author (UserDto)
   • Includes comments (List<CommentDto>)
        │
        ▼
7. Returns 200 OK with complete PostDto
```

---

## Component Responsibilities

### Controllers (WebAPI Layer)
**Responsibilities:**
- Receive HTTP requests
- Validate input
- Call business logic/repositories
- Handle exceptions
- Return HTTP responses
- Convert entities to DTOs

**Does NOT:**
- Access files directly
- Contain business logic (minimal)
- Know about JSON serialization

### Repositories (Persistence Layer)
**Responsibilities:**
- Manage data persistence
- CRUD operations
- Data validation (exists/not exists)
- Generate IDs
- Handle file I/O

**Does NOT:**
- Know about HTTP
- Return DTOs (returns entities)
- Handle presentation logic

### DTOs (Shared Layer)
**Responsibilities:**
- Define API contracts
- Transport data between layers
- Hide sensitive information
- Provide clean interface

**Does NOT:**
- Contain logic
- Know about persistence
- Have relationships

---

## Request/Response Flow

### Successful Request Flow

```
Browser/Client
    │
    │ 1. HTTP Request (POST /Users)
    │    Content-Type: application/json
    │    Body: CreateUserDto
    │
    ▼
Kestrel Web Server (ASP.NET Core)
    │
    │ 2. Route to Controller
    │
    ▼
UsersController
    │
    │ 3. Validate & Process
    │
    ▼
IUserRepository (Interface)
    │
    │ 4. Dependency Injection
    │
    ▼
UserFileRepository (Implementation)
    │
    │ 5. File Operations
    │
    ▼
users.json (File System)
    │
    │ 6. Data Persisted
    │
    ▼
UserFileRepository
    │
    │ 7. Return Entity
    │
    ▼
UsersController
    │
    │ 8. Convert to UserDto
    │    Return 201 Created
    │
    ▼
JSON Serializer
    │
    │ 9. Serialize to JSON
    │
    ▼
Kestrel Web Server
    │
    │ 10. HTTP Response
    │
    ▼
Browser/Client (Receives UserDto as JSON)
```

### Error Request Flow

```
Browser/Client
    │
    │ 1. HTTP Request (POST /Posts)
    │    userId: 999 (doesn't exist)
    │
    ▼
PostsController
    │
    │ 2. Validate user exists
    │    Calls userRepo.GetSingleAsync(999)
    │
    ▼
UserFileRepository
    │
    │ 3. User not found
    │    Throws InvalidOperationException
    │
    ▼
PostsController (catch block)
    │
    │ 4. Catches exception
    │    Returns 400 BadRequest
    │    Message: "User with id 999 not found"
    │
    ▼
Browser/Client (Receives error response)
```

---

## Technology Stack

```
┌──────────────────────────────────────┐
│         ASP.NET Core 9.0             │
│  • Web API Framework                 │
│  • Kestrel Web Server                │
│  • Dependency Injection              │
└──────────────────────────────────────┘
                │
    ┌───────────┴───────────┐
    │                       │
    ▼                       ▼
┌─────────┐           ┌──────────┐
│ Swagger │           │  JSON    │
│ OpenAPI │           │.NET      │
└─────────┘           └──────────┘
```

### Packages Used:
- **Microsoft.AspNetCore.OpenApi** (9.0.8)
- **Swashbuckle.AspNetCore** (7.2.0)
- **System.Text.Json** (Built-in)

---

## Deployment View

```
Development Environment:
┌─────────────────────────────────────────┐
│  Your Computer                          │
│  ┌────────────────────────────────────┐ │
│  │  Visual Studio / Rider / VS Code   │ │
│  └────────────────────────────────────┘ │
│  ┌────────────────────────────────────┐ │
│  │  .NET 9.0 SDK                      │ │
│  └────────────────────────────────────┘ │
│  ┌────────────────────────────────────┐ │
│  │  Kestrel Server (localhost:7005)   │ │
│  │    ├── WebAPI.dll                  │ │
│  │    ├── users.json                  │ │
│  │    ├── posts.json                  │ │
│  │    └── comments.json               │ │
│  └────────────────────────────────────┘ │
└─────────────────────────────────────────┘
            │
            │ Browser
            ▼
┌─────────────────────────────────────────┐
│  https://localhost:7005/swagger         │
└─────────────────────────────────────────┘
```

---

## Design Patterns Used

1. **Repository Pattern**
   - IUserRepository, IPostRepository, ICommentRepository
   - Abstraction of data access

2. **Dependency Injection**
   - Constructor injection of repositories
   - Loose coupling

3. **DTO Pattern**
   - Separate objects for data transfer
   - API contract separation

4. **RESTful Architecture**
   - Resource-based URLs
   - HTTP verbs for operations
   - Status codes for responses

5. **Async/Await Pattern**
   - Non-blocking I/O operations
   - Better scalability

---

## Security Considerations

### Current Implementation:
- ⚠️ Passwords stored in plain text
- ⚠️ No authentication/authorization
- ⚠️ All endpoints publicly accessible

### To Be Added (Part 6):
- ✅ JWT authentication
- ✅ Password hashing
- ✅ Role-based authorization
- ✅ HTTPS enforcement

---

## Performance Characteristics

### Current Setup:
- **Fast for small datasets** (< 1000 entities)
- **Memory efficient** (data loaded on demand)
- **Simple debugging** (can view JSON files)

### Limitations:
- Not suitable for production
- No concurrent access handling
- Full file read/write on each operation

### Future Improvements (Part 7):
- Database with indexing
- Connection pooling
- Query optimization
- Caching

---

This architecture provides a solid foundation for your forum application and follows .NET best practices! 🎯
