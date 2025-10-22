# API Endpoints Quick Reference

Base URL: `https://localhost:7005`

## 👥 Users Endpoints

### Create User
```http
POST /Users
Content-Type: application/json

{
  "userName": "john_doe",
  "password": "password123"
}
```

### Update User
```http
PUT /Users/1
Content-Type: application/json

{
  "userName": "john_updated",
  "password": "newpassword"
}
```

### Delete User
```http
DELETE /Users/1
```

### Get Single User
```http
GET /Users/1
```

### Get All Users
```http
GET /Users
GET /Users?userNameContains=john
```

---

## 📝 Posts Endpoints

### Create Post
```http
POST /Posts
Content-Type: application/json

{
  "title": "My First Post",
  "body": "This is the content of my post",
  "userId": 1
}
```

### Update Post
```http
PUT /Posts/1
Content-Type: application/json

{
  "title": "Updated Title",
  "body": "Updated content"
}
```

### Delete Post
```http
DELETE /Posts/1
```

### Get Single Post
```http
GET /Posts/1
GET /Posts/1?includeAuthor=true
GET /Posts/1?includeComments=true
GET /Posts/1?includeAuthor=true&includeComments=true
```

### Get All Posts
```http
GET /Posts
GET /Posts?titleContains=first
GET /Posts?userId=1
GET /Posts?userName=john
GET /Posts?titleContains=post&userId=1
```

---

## 💬 Comments Endpoints

### Create Comment
```http
POST /Comments
Content-Type: application/json

{
  "body": "Great post!",
  "userId": 1,
  "postId": 1
}
```

### Update Comment
```http
PUT /Comments/1
Content-Type: application/json

{
  "body": "Updated comment text"
}
```

### Delete Comment
```http
DELETE /Comments/1
```

### Get Single Comment
```http
GET /Comments/1
GET /Comments/1?includeAuthor=true
```

### Get All Comments
```http
GET /Comments
GET /Comments?userId=1
GET /Comments?userName=john
GET /Comments?postId=1
GET /Comments?postId=1&userId=1
```

---

## 📋 Response Status Codes

| Code | Meaning | When it occurs |
|------|---------|----------------|
| 200 | OK | Successful GET request |
| 201 | Created | Successful POST request |
| 204 | No Content | Successful PUT/DELETE request |
| 400 | Bad Request | Invalid data or missing required fields |
| 404 | Not Found | Resource with given ID doesn't exist |
| 500 | Internal Server Error | Unexpected server error |

---

## 🧪 Testing Workflow Example

### 1. Create a user
```http
POST /Users
{"userName": "alice", "password": "pass123"}
```
Response: `{"id": 1, "userName": "alice"}`

### 2. Create another user
```http
POST /Users
{"userName": "bob", "password": "pass456"}
```
Response: `{"id": 2, "userName": "bob"}`

### 3. Create a post
```http
POST /Posts
{"title": "Hello World", "body": "My first post!", "userId": 1}
```
Response: `{"id": 1, "title": "Hello World", "body": "My first post!", "userId": 1}`

### 4. Add a comment
```http
POST /Comments
{"body": "Nice post Alice!", "userId": 2, "postId": 1}
```
Response: `{"id": 1, "body": "Nice post Alice!", "userId": 2, "postId": 1}`

### 5. Get post with author and comments
```http
GET /Posts/1?includeAuthor=true&includeComments=true
```
Response:
```json
{
  "id": 1,
  "title": "Hello World",
  "body": "My first post!",
  "userId": 1,
  "author": {
    "id": 1,
    "userName": "alice"
  },
  "comments": [
    {
      "id": 1,
      "body": "Nice post Alice!",
      "userId": 2,
      "postId": 1
    }
  ]
}
```

### 6. Get all posts by alice
```http
GET /Posts?userName=alice
```

### 7. Get all comments on post 1
```http
GET /Comments?postId=1
```

---

## 💡 Tips

- Use Swagger UI at `https://localhost:7005/swagger` for interactive testing
- Create users first, then posts, then comments (due to foreign key constraints)
- Remember to use the returned IDs from POST requests
- Query parameters are optional - you can combine multiple filters
- The `includeAuthor` and `includeComments` parameters help reduce API calls
- All data is saved to JSON files in the WebAPI project directory
