# 🔧 Troubleshooting Guide - Part 4

## Build and Compilation Issues

### Error: "The type or namespace name could not be found"

**Symptoms:** Build fails with missing namespace errors

**Solutions:**
1. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

2. Verify project references in `WebAPI.csproj`:
   ```xml
   <ProjectReference Include="..\Server\RepositoryContracts\RepositoryContracts.csproj" />
   <ProjectReference Include="..\FileRepositories\FileRepositories.csproj" />
   <ProjectReference Include="..\Entities\Entities.csproj" />
   <ProjectReference Include="..\ApiContracts\ApiContracts.csproj" />
   ```

3. Clean and rebuild:
   ```bash
   dotnet clean
   dotnet build
   ```

### Error: "Swashbuckle.AspNetCore not found"

**Symptoms:** Build fails mentioning Swagger or Swashbuckle

**Solution:**
Add the package to `WebAPI.csproj`:
```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0"/>
```

Then restore:
```bash
dotnet restore
```

---

## Runtime Issues

### Issue: API won't start - "Address already in use"

**Symptoms:**
```
Failed to bind to address https://127.0.0.1:7005: address already in use
```

**Solutions:**
1. Find and kill the process using the port:
   ```bash
   # Windows
   netstat -ano | findstr :7005
   taskkill /PID <process_id> /F
   ```

2. Or change the port in `WebAPI/Properties/launchSettings.json`:
   ```json
   "applicationUrl": "https://localhost:7006;http://localhost:5001"
   ```

### Issue: Swagger UI shows "Failed to load API definition"

**Symptoms:** Swagger page loads but shows an error instead of API documentation

**Solutions:**
1. Check that `Program.cs` has these lines:
   ```csharp
   builder.Services.AddEndpointsApiExplorer();
   builder.Services.AddSwaggerGen();
   ```

2. And in the app configuration:
   ```csharp
   if (app.Environment.IsDevelopment())
   {
       app.UseSwagger();
       app.UseSwaggerUI();
   }
   ```

3. Verify all controllers have `[ApiController]` and `[Route("[controller]")]` attributes

### Issue: "Null reference exception" when calling repository methods

**Symptoms:** API crashes with NullReferenceException

**Solutions:**
1. Verify repositories are registered in `Program.cs`:
   ```csharp
   builder.Services.AddScoped<IUserRepository, UserFileRepository>();
   builder.Services.AddScoped<IPostRepository, PostFileRepository>();
   builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();
   ```

2. Check that controllers receive repositories through constructor:
   ```csharp
   public UsersController(IUserRepository userRepo)
   {
       this.userRepo = userRepo;
   }
   ```

---

## Data and Persistence Issues

### Issue: JSON files are not being created

**Symptoms:** No `users.json`, `posts.json`, or `comments.json` files exist

**Solutions:**
1. Check that you're running the API from the correct directory:
   ```bash
   cd WebAPI
   dotnet run
   ```

2. Files are created in the current working directory. Check:
   ```bash
   # The files should be in WebAPI folder
   ls users.json posts.json comments.json
   ```

3. Verify FileRepository constructors create files:
   ```csharp
   if (!File.Exists(filePath))
   {
       File.WriteAllText(filePath, "[]");
   }
   ```

### Issue: Data is lost between runs

**Symptoms:** Created users/posts/comments disappear when restarting the API

**Solutions:**
1. Verify JSON files exist and contain data
2. Check file paths in repository classes are relative (not absolute)
3. Ensure you're running from the same directory each time

### Issue: "User with ID X not found" when user exists

**Symptoms:** Error says user doesn't exist, but you just created it

**Solutions:**
1. Check the returned ID from the create operation - use that ID
2. Verify the JSON file was updated:
   ```bash
   cat users.json  # Linux/Mac
   type users.json  # Windows
   ```
3. Restart the API to reload data from files

---

## API Endpoint Issues

### Issue: 404 Not Found on valid endpoint

**Symptoms:** GET /Users returns 404

**Solutions:**
1. Verify the API is running on the correct port
2. Check controller routing:
   ```csharp
   [Route("[controller]")]  // Will be /Users for UsersController
   ```
3. Ensure controllers are in the `Controllers` folder
4. Verify `app.MapControllers()` is called in `Program.cs`

### Issue: 400 Bad Request when creating post/comment

**Symptoms:**
```json
{
  "error": "User with id 1 not found"
}
```

**Solutions:**
1. Create a user first:
   ```http
   POST /Users
   {"userName": "test", "password": "test"}
   ```
2. Use the returned ID in your post/comment creation
3. Verify user exists:
   ```http
   GET /Users/1
   ```

### Issue: Query parameters don't filter results

**Symptoms:** GET /Users?userNameContains=john returns all users

**Solutions:**
1. Check the controller implementation:
   ```csharp
   if (!string.IsNullOrWhiteSpace(userNameContains))
   {
       query = query.Where(u => u.UserName.ToLower().Contains(userNameContains.ToLower()));
   }
   ```

2. Verify the parameter name matches:
   ```csharp
   [FromQuery] string? userNameContains = null
   ```

3. Use correct parameter syntax in URL:
   ```
   /Users?userNameContains=john  ✓
   /Users?username=john          ✗
   ```

---

## JSON Serialization Issues

### Issue: Circular reference error

**Symptoms:**
```
JsonException: A possible object cycle was detected
```

**Solutions:**
This shouldn't happen with the current implementation since we use DTOs, but if it does:

1. Verify you're returning DTOs, not entities:
   ```csharp
   return Ok(new UserDto { ... });  // ✓ Correct
   return Ok(user);                  // ✗ Might cause issues
   ```

2. Don't include navigation properties in entities yet (wait for EF Core)

### Issue: Property names don't match

**Symptoms:** JSON has different casing than expected

**Solutions:**
ASP.NET Core uses camelCase by default. This is correct:
```json
{
  "id": 1,
  "userName": "john",
  "userId": 1
}
```

---

## Testing Issues with Swagger

### Issue: "Try it out" button doesn't work

**Symptoms:** Can't interact with endpoints in Swagger

**Solutions:**
1. Refresh the page
2. Clear browser cache
3. Try a different browser
4. Check browser console for JavaScript errors

### Issue: Request body shows wrong format

**Symptoms:** Swagger expects XML instead of JSON

**Solution:**
Verify `Program.cs` has:
```csharp
builder.Services.AddControllers();
```
Not:
```csharp
builder.Services.AddControllersWithViews();
```

---

## Performance Issues

### Issue: API is slow

**Symptoms:** Requests take several seconds

**Solutions:**
1. Check file sizes - large JSON files slow down operations:
   ```bash
   ls -lh *.json
   ```

2. Consider pagination for GetMany endpoints (optional enhancement)

3. Verify async/await is used correctly:
   ```csharp
   public async Task<ActionResult> MyAction()
   {
       var data = await repo.GetSingleAsync(id);  // ✓
       var data = repo.GetSingleAsync(id).Result;  // ✗ Blocking
   }
   ```

---

## Git and Version Control Issues

### Issue: JSON data files are being committed

**Symptoms:** users.json, posts.json in git status

**Solution:**
Add to `.gitignore`:
```
*.json
!appsettings.json
!appsettings.*.json
```

Then remove from git:
```bash
git rm --cached users.json posts.json comments.json
```

---

## Common Error Messages Explained

### "User with ID 'X' not found"
- **Cause:** Trying to get/update/delete a non-existent user
- **Fix:** Verify the user ID exists with GET /Users

### "Post with ID 'X' not found"
- **Cause:** Trying to access a non-existent post
- **Fix:** Create post first or use correct ID

### "Username 'X' is already taken"
- **Cause:** Trying to create user with existing username
- **Fix:** Use a different username

### "User with id X not found" (when creating post)
- **Cause:** Referenced user doesn't exist
- **Fix:** Create user first, then use that user's ID

---

## Debug Tips

### Enable detailed errors:

In `Program.cs`:
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Add this
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

### Check JSON file contents:

```bash
# Pretty-print JSON
cat users.json | python -m json.tool

# Or on Windows with PowerShell
Get-Content users.json | ConvertFrom-Json | ConvertTo-Json
```

### Test with curl instead of Swagger:

```bash
# Create user
curl -X POST https://localhost:7005/Users \
  -H "Content-Type: application/json" \
  -d '{"userName":"test","password":"test123"}' \
  -k

# Get users
curl https://localhost:7005/Users -k
```

---

## Getting Help

If you're still stuck:

1. **Check the error message carefully** - it usually tells you what's wrong
2. **Look at the response status code** - 400, 404, 500 each mean different things
3. **Verify your request data** - check JSON syntax, required fields
4. **Check the console output** - the API logs errors there
5. **Review the assignment README** - step-by-step instructions
6. **Compare with working examples** - use the API_QUICK_REFERENCE.md

### Useful commands for debugging:

```bash
# See what's running on a port (Windows)
netstat -ano | findstr :7005

# Check if files exist
dir *.json  # Windows
ls *.json   # Linux/Mac

# View file contents
type users.json   # Windows
cat users.json    # Linux/Mac

# Restart fresh
dotnet clean
del *.json  # Delete data files
dotnet build
dotnet run
```

---

## Still Having Issues?

Create a minimal test case:

1. Stop the API
2. Delete all JSON files
3. Start the API
4. Try ONLY this sequence:
   - Create 1 user
   - Get that user
   - Create 1 post with that user ID
   - Get that post

If this works, your setup is correct. If not, check:
- Controller code
- Repository code
- Project references
- NuGet packages

Good luck! 🍀
