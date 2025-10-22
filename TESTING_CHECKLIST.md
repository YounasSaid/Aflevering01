# ✅ Part 4 Completion Checklist

## Before You Start Testing

### 1. Verify Project Structure
- [ ] All DTO files exist in `ApiContracts/` folder
- [ ] All Controller files exist in `WebAPI/Controllers/` folder
- [ ] `WebAPI.csproj` includes Swashbuckle.AspNetCore package
- [ ] `Program.cs` has been updated with Swagger configuration

### 2. Build the Solution
```bash
cd "C:\Users\Youna\OneDrive - ViaUC\3 SEM\DNP 1\Aflevering01"
dotnet restore
dotnet build
```

Expected output: `Build succeeded. 0 Warning(s)`

### 3. Run the Web API
```bash
cd WebAPI
dotnet run
```

Expected output should include:
```
Now listening on: https://localhost:XXXX
```

### 4. Open Swagger UI
- [ ] Open browser
- [ ] Navigate to `https://localhost:XXXX/swagger` (use the port from step 3)
- [ ] Swagger UI loads successfully
- [ ] You see three controllers: Users, Posts, Comments

---

## Testing Each Controller

### ✅ UsersController Tests

#### Test 1: Create User
- [ ] Expand `POST /Users`
- [ ] Click "Try it out"
- [ ] Enter test data:
```json
{
  "userName": "testuser",
  "password": "test123"
}
```
- [ ] Click "Execute"
- [ ] Response code is `201 Created`
- [ ] Response includes user ID and username
- [ ] Note the user ID: _______

#### Test 2: Get All Users
- [ ] Expand `GET /Users`
- [ ] Click "Try it out"
- [ ] Click "Execute"
- [ ] Response code is `200 OK`
- [ ] Response includes the user created in Test 1

#### Test 3: Get Single User
- [ ] Expand `GET /Users/{id}`
- [ ] Enter the user ID from Test 1
- [ ] Click "Execute"
- [ ] Response code is `200 OK`
- [ ] Correct user data returned

#### Test 4: Search Users
- [ ] Expand `GET /Users`
- [ ] Enter `userNameContains: test`
- [ ] Click "Execute"
- [ ] Only users with "test" in username are returned

#### Test 5: Update User
- [ ] Expand `PUT /Users/{id}`
- [ ] Enter user ID from Test 1
- [ ] Enter updated data:
```json
{
  "userName": "updateduser",
  "password": "newpass123"
}
```
- [ ] Click "Execute"
- [ ] Response code is `204 No Content`

#### Test 6: Delete User (Skip for now - we need this user for post tests)

---

### ✅ PostsController Tests

#### Test 7: Create Post
- [ ] Expand `POST /Posts`
- [ ] Click "Try it out"
- [ ] Enter test data (use user ID from Test 1):
```json
{
  "title": "My Test Post",
  "body": "This is a test post content",
  "userId": 1
}
```
- [ ] Click "Execute"
- [ ] Response code is `201 Created`
- [ ] Response includes post ID, title, body, and userId
- [ ] Note the post ID: _______

#### Test 8: Create Post with Invalid User
- [ ] Expand `POST /Posts`
- [ ] Enter test data with non-existent user:
```json
{
  "title": "Invalid Post",
  "body": "This should fail",
  "userId": 999
}
```
- [ ] Click "Execute"
- [ ] Response code is `400 Bad Request`
- [ ] Error message mentions user not found

#### Test 9: Get Single Post
- [ ] Expand `GET /Posts/{id}`
- [ ] Enter post ID from Test 7
- [ ] Click "Execute"
- [ ] Response code is `200 OK`
- [ ] Correct post data returned

#### Test 10: Get Post with Author
- [ ] Expand `GET /Posts/{id}`
- [ ] Enter post ID from Test 7
- [ ] Check `includeAuthor: true`
- [ ] Click "Execute"
- [ ] Response includes author object with username

#### Test 11: Get All Posts
- [ ] Expand `GET /Posts`
- [ ] Click "Try it out"
- [ ] Click "Execute"
- [ ] Response code is `200 OK`
- [ ] All posts are returned

#### Test 12: Filter Posts by Title
- [ ] Expand `GET /Posts`
- [ ] Enter `titleContains: Test`
- [ ] Click "Execute"
- [ ] Only posts with "Test" in title are returned

#### Test 13: Filter Posts by User
- [ ] Expand `GET /Posts`
- [ ] Enter `userId: 1`
- [ ] Click "Execute"
- [ ] Only posts by user 1 are returned

#### Test 14: Update Post
- [ ] Expand `PUT /Posts/{id}`
- [ ] Enter post ID from Test 7
- [ ] Enter updated data:
```json
{
  "title": "Updated Post Title",
  "body": "Updated post content"
}
```
- [ ] Click "Execute"
- [ ] Response code is `204 No Content`

---

### ✅ CommentsController Tests

#### Test 15: Create Comment
- [ ] Expand `POST /Comments`
- [ ] Click "Try it out"
- [ ] Enter test data (use user ID and post ID from previous tests):
```json
{
  "body": "This is a great post!",
  "userId": 1,
  "postId": 1
}
```
- [ ] Click "Execute"
- [ ] Response code is `201 Created`
- [ ] Response includes comment ID, body, userId, and postId
- [ ] Note the comment ID: _______

#### Test 16: Create Comment with Invalid Post
- [ ] Expand `POST /Comments`
- [ ] Enter test data with non-existent post:
```json
{
  "body": "Invalid comment",
  "userId": 1,
  "postId": 999
}
```
- [ ] Click "Execute"
- [ ] Response code is `400 Bad Request`
- [ ] Error message mentions post not found

#### Test 17: Get Single Comment
- [ ] Expand `GET /Comments/{id}`
- [ ] Enter comment ID from Test 15
- [ ] Click "Execute"
- [ ] Response code is `200 OK`
- [ ] Correct comment data returned

#### Test 18: Get Comment with Author
- [ ] Expand `GET /Comments/{id}`
- [ ] Enter comment ID from Test 15
- [ ] Check `includeAuthor: true`
- [ ] Click "Execute"
- [ ] Response includes author object

#### Test 19: Get All Comments
- [ ] Expand `GET /Comments`
- [ ] Click "Try it out"
- [ ] Click "Execute"
- [ ] Response code is `200 OK`
- [ ] All comments are returned

#### Test 20: Filter Comments by Post
- [ ] Expand `GET /Comments`
- [ ] Enter `postId: 1`
- [ ] Click "Execute"
- [ ] Only comments for post 1 are returned

#### Test 21: Filter Comments by User
- [ ] Expand `GET /Comments`
- [ ] Enter `userId: 1`
- [ ] Click "Execute"
- [ ] Only comments by user 1 are returned

#### Test 22: Update Comment
- [ ] Expand `PUT /Comments/{id}`
- [ ] Enter comment ID from Test 15
- [ ] Enter updated data:
```json
{
  "body": "Updated comment text"
}
```
- [ ] Click "Execute"
- [ ] Response code is `204 No Content`

#### Test 23: Delete Comment
- [ ] Expand `DELETE /Comments/{id}`
- [ ] Enter comment ID from Test 15
- [ ] Click "Execute"
- [ ] Response code is `204 No Content`
- [ ] Try to get the same comment - should return `404 Not Found`

---

### ✅ Advanced Tests

#### Test 24: Get Post with Author and Comments
- [ ] Create a new comment on post 1 (repeat Test 15)
- [ ] Expand `GET /Posts/{id}`
- [ ] Enter post ID: 1
- [ ] Check both `includeAuthor: true` and `includeComments: true`
- [ ] Click "Execute"
- [ ] Response includes both author and comments array

#### Test 25: Multiple Query Parameters
- [ ] Expand `GET /Posts`
- [ ] Enter `titleContains: Test` AND `userId: 1`
- [ ] Click "Execute"
- [ ] Results match both criteria

#### Test 26: Filter Comments by Username
- [ ] Expand `GET /Comments`
- [ ] Enter `userName: test` (or part of the username you created)
- [ ] Click "Execute"
- [ ] Only comments by matching users are returned

---

## Verify Data Persistence

### Test 27: Data Survives Restart
- [ ] Stop the WebAPI (Ctrl+C in terminal)
- [ ] Check that these files exist in `WebAPI/` folder:
  - [ ] `users.json`
  - [ ] `posts.json`
  - [ ] `comments.json`
- [ ] Open `users.json` - verify your test data is there
- [ ] Restart the WebAPI: `dotnet run`
- [ ] Open Swagger again
- [ ] Get all users - your data should still be there

---

## Assignment Requirements Verification

### Step 4.4 - Implement Controllers ✅
- [ ] UsersController with all CRUD operations
- [ ] PostsController with all CRUD operations
- [ ] CommentsController with all CRUD operations
- [ ] Correct HTTP verbs used (POST, GET, PUT, DELETE)
- [ ] REST routing conventions followed
- [ ] DTOs used instead of entities

### Step 4.5 - GetMany Query Parameters ✅
- [ ] Users: `userNameContains` filter works
- [ ] Posts: `titleContains` filter works
- [ ] Posts: `userId` filter works
- [ ] Posts: `userName` filter works
- [ ] Comments: `userId` filter works
- [ ] Comments: `userName` filter works
- [ ] Comments: `postId` filter works

### Additional Requirements ✅
- [ ] Business logic: User existence validation on post/comment creation
- [ ] Business logic: Username uniqueness validation
- [ ] Optional includes: `includeAuthor` and `includeComments` work
- [ ] Error handling: 400 for bad requests
- [ ] Error handling: 404 for not found
- [ ] Swagger documentation available and working

---

## Common Issues and Solutions

### Issue: "Port already in use"
**Solution:** Change the port in `launchSettings.json` or stop other applications using that port

### Issue: "User not found" when creating post
**Solution:** Create a user first, note the ID, then use that ID for the post

### Issue: Swagger page doesn't load
**Solution:** 
- Check the actual URL in the console output
- Try http instead of https
- Verify Swashbuckle.AspNetCore package is installed

### Issue: Build errors
**Solution:**
- Run `dotnet restore`
- Run `dotnet clean`
- Run `dotnet build`

### Issue: JSON files not created
**Solution:** The files are created in the directory where you run `dotnet run`. Make sure you're in the WebAPI folder.

---

## Final Checklist

- [ ] All 26+ tests passed
- [ ] Swagger UI working correctly
- [ ] All endpoints return correct status codes
- [ ] Data persists after restart
- [ ] Query parameters work as expected
- [ ] Error handling works properly
- [ ] All assignment requirements met

---

## 🎉 Congratulations!

If all tests passed, Part 4 is complete! You now have a fully functional REST Web API with:
- Complete CRUD operations for Users, Posts, and Comments
- Query parameter filtering
- Optional data includes
- Proper error handling
- Swagger documentation
- File-based data persistence

**You're ready to proceed to Part 5 (Blazor)!**
