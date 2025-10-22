# Part 4 - REST Web API - Completed

## What Has Been Implemented

### 1. DTOs (Data Transfer Objects) in ApiContracts project
- **CreateUserDto** - For creating new users
- **UpdateUserDto** - For updating existing users
- **UserDto** - For returning user data (without password)
- **CreatePostDto** - For creating new posts
- **UpdatePostDto** - For updating existing posts
- **PostDto** - For returning post data (with optional author and comments)
- **CreateCommentDto** - For creating new comments
- **UpdateCommentDto** - For updating existing comments
- **CommentDto** - For returning comment data (with optional author)

### 2. Controllers in WebAPI project

#### UsersController
- **POST /Users** - Create a new user
- **PUT /Users/{id}** - Update an existing user
- **DELETE /Users/{id}** - Delete a user
- **GET /Users/{id}** - Get a single user by ID
- **GET /Users** - Get all users with optional filtering:
  - `?userNameContains=text` - Filter users by username containing text

#### PostsController
- **POST /Posts** - Create a new post
- **PUT /Posts/{id}** - Update an existing post
- **DELETE /Posts/{id}** - Delete a post
- **GET /Posts/{id}** - Get a single post by ID with optional includes:
  - `?includeAuthor=true` - Include author information
  - `?includeComments=true` - Include all comments on the post
- **GET /Posts** - Get all posts with optional filtering:
  - `?titleContains=text` - Filter posts by title containing text
  - `?userId=1` - Filter posts by user ID
  - `?userName=text` - Filter posts by username containing text

#### CommentsController
- **POST /Comments** - Create a new comment
- **PUT /Comments/{id}** - Update an existing comment
- **DELETE /Comments/{id}** - Delete a comment
- **GET /Comments/{id}** - Get a single comment by ID with optional includes:
  - `?includeAuthor=true` - Include author information
- **GET /Comments** - Get all comments with optional filtering:
  - `?userId=1` - Filter comments by user ID
  - `?userName=text` - Filter comments by username containing text
  - `?postId=1` - Filter comments by post ID

### 3. Updated Program.cs
- Added Swagger/OpenAPI support for API documentation
- Registered all repository implementations for dependency injection
- Configured the HTTP request pipeline

## How to Run and Test

### Step 1: Restore NuGet Packages
Open terminal in the project root and run:
```bash
dotnet restore
```

### Step 2: Build the Solution
```bash
dotnet build
```

### Step 3: Run the Web API
Navigate to the WebAPI project folder:
```bash
cd WebAPI
dotnet run
```

The API should start and display something like:
```
Now listening on: https://localhost:7005
```

### Step 4: Access Swagger UI
Open your browser and go to:
```
https://localhost:7005/swagger
```

This will open the Swagger UI where you can test all endpoints interactively.

## Testing the API with Swagger

### Example 1: Create a User
1. In Swagger, expand **POST /Users**
2. Click "Try it out"
3. Enter the following JSON in the request body:
```json
{
  "userName": "john_doe",
  "password": "password123"
}
```
4. Click "Execute"
5. You should get a `201 Created` response with the created user (note the ID)

### Example 2: Create a Post
1. Expand **POST /Posts**
2. Click "Try it out"
3. Enter the following JSON (use the user ID from Example 1):
```json
{
  "title": "My First Post",
  "body": "This is the content of my first post!",
  "userId": 1
}
```
4. Click "Execute"
5. You should get a `201 Created` response

### Example 3: Get a Post with Author and Comments
1. Expand **GET /Posts/{id}**
2. Click "Try it out"
3. Enter the post ID (e.g., 1)
4. Check both `includeAuthor` and `includeComments` boxes
5. Click "Execute"
6. You should see the post with author information

### Example 4: Add a Comment
1. Expand **POST /Comments**
2. Click "Try it out"
3. Enter the following JSON:
```json
{
  "body": "Great post!",
  "userId": 1,
  "postId": 1
}
```
4. Click "Execute"

### Example 5: Get All Posts by a User
1. Expand **GET /Posts**
2. Click "Try it out"
3. Enter userId parameter (e.g., 1)
4. Click "Execute"

## Data Persistence

All data is stored in JSON files in the WebAPI project directory:
- `users.json`
- `posts.json`
- `comments.json`

These files are automatically created when you first run the API.

## Error Handling

The API includes proper error handling:
- **400 Bad Request** - When creating a post/comment with non-existent user/post
- **404 Not Found** - When trying to access a resource that doesn't exist
- **409 Conflict** - When trying to create a user with an existing username

## Architecture Overview

```
Aflevering01/
├── ApiContracts/ (Shared DTOs)
├── Entities/ (Domain models)
├── Server/
│   ├── RepositoryContracts/ (Repository interfaces)
│   ├── FileRepositories/ (JSON file implementations)
│   ├── CLI/ (Command line interface)
│   └── WebAPI/ (REST API controllers)
```

## Next Steps

Now that Part 4 is complete, you can:
1. Test all endpoints using Swagger
2. Create users, posts, and comments
3. Try different query parameters for filtering
4. Proceed to Part 5 (Blazor frontend)

## Troubleshooting

### If Swagger doesn't open:
- Check the console for the actual URL (port might be different)
- Try `http://localhost:5000/swagger` if HTTPS doesn't work

### If you get "User not found" errors:
- Create a user first using POST /Users
- Note the returned user ID
- Use that ID when creating posts or comments

### If the API doesn't start:
- Make sure no other application is using port 7005
- Check that all dependencies are restored: `dotnet restore`
- Rebuild the solution: `dotnet build`
