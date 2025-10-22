# 🚀 Quick Start Guide - Part 4

**Never used Swagger before? Start here!**

---

## Step 1: Open Terminal

1. Press `Windows Key + R`
2. Type `cmd` and press Enter
3. Or use the terminal in Visual Studio/Rider

---

## Step 2: Navigate to Project

Copy and paste this into terminal:
```bash
cd "C:\Users\Youna\OneDrive - ViaUC\3 SEM\DNP 1\Aflevering01"
```

Press Enter.

---

## Step 3: Restore Packages

Type this:
```bash
dotnet restore
```

Press Enter. Wait for "Restore succeeded" message.

---

## Step 4: Build Project

Type this:
```bash
dotnet build
```

Press Enter. You should see "Build succeeded. 0 Warning(s)".

**If you see errors:** Check TROUBLESHOOTING.md

---

## Step 5: Navigate to WebAPI

Type this:
```bash
cd WebAPI
```

Press Enter.

---

## Step 6: Run the API

Type this:
```bash
dotnet run
```

Press Enter. You should see something like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7005
      Now listening on: http://localhost:5000
```

**Important:** Note the `https://localhost:XXXX` number (yours might be different than 7005)

**Keep this terminal open!** Don't close it while testing.

---

## Step 7: Open Swagger

1. Open your web browser (Chrome, Edge, Firefox, etc.)
2. Type this in the address bar (use YOUR port number from Step 6):
```
https://localhost:7005/swagger
```
3. Press Enter

**If it doesn't load:** Try `http://localhost:5000/swagger` instead

You should see a page titled "Swagger UI" with three sections: Users, Posts, Comments

---

## Step 8: Your First Test - Create a User

1. On the Swagger page, find **"Users"** section
2. Click on **"POST /Users"** to expand it
3. Click the blue **"Try it out"** button on the right
4. You'll see a text box with example JSON
5. Replace it with this:
```json
{
  "userName": "alice",
  "password": "password123"
}
```
6. Click the blue **"Execute"** button
7. Scroll down to see the response

**Success looks like:**
- Response code: `201`
- Response body:
```json
{
  "id": 1,
  "userName": "alice"
}
```

**📝 Write down this ID: 1**

---

## Step 9: Get Your User

1. Scroll up and find **"GET /Users/{id}"**
2. Click on it to expand
3. Click **"Try it out"**
4. In the "id" box, type: `1` (or whatever ID you got in Step 8)
5. Click **"Execute"**

**Success looks like:**
- Response code: `200`
- Same user data as before

---

## Step 10: Create a Post

1. Find **"Posts"** section
2. Click on **"POST /Posts"**
3. Click **"Try it out"**
4. Replace the JSON with:
```json
{
  "title": "My First Post",
  "body": "Hello, this is my first forum post!",
  "userId": 1
}
```
**Important:** Use the user ID from Step 8!

5. Click **"Execute"**

**Success looks like:**
- Response code: `201`
- Response body:
```json
{
  "id": 1,
  "title": "My First Post",
  "body": "Hello, this is my first forum post!",
  "userId": 1
}
```

**📝 Write down this post ID: 1**

---

## Step 11: Get Your Post with Author

1. Find **"GET /Posts/{id}"**
2. Click to expand
3. Click **"Try it out"**
4. Enter post id: `1`
5. **Check the box** next to `includeAuthor` 
6. Click **"Execute"**

**Success looks like:**
```json
{
  "id": 1,
  "title": "My First Post",
  "body": "Hello, this is my first forum post!",
  "userId": 1,
  "author": {
    "id": 1,
    "userName": "alice"
  },
  "comments": null
}
```

Notice the `author` object now includes the username!

---

## Step 12: Add a Comment

1. Find **"Comments"** section
2. Click on **"POST /Comments"**
3. Click **"Try it out"**
4. Replace the JSON with:
```json
{
  "body": "Great first post!",
  "userId": 1,
  "postId": 1
}
```
5. Click **"Execute"**

**Success looks like:**
- Response code: `201`
- Comment data with an ID

---

## Step 13: Get Post with Comments

1. Go back to **"GET /Posts/{id}"**
2. Click **"Try it out"**
3. Enter post id: `1`
4. **Check BOTH boxes:**
   - ✅ includeAuthor
   - ✅ includeComments
5. Click **"Execute"**

**Success looks like:**
```json
{
  "id": 1,
  "title": "My First Post",
  "body": "Hello, this is my first forum post!",
  "userId": 1,
  "author": {
    "id": 1,
    "userName": "alice"
  },
  "comments": [
    {
      "id": 1,
      "body": "Great first post!",
      "userId": 1,
      "postId": 1,
      "author": null
    }
  ]
}
```

Now you see both the author AND the comments!

---

## Step 14: Get All Posts

1. Find **"GET /Posts"** (without {id})
2. Click to expand
3. Click **"Try it out"**
4. Leave all fields empty
5. Click **"Execute"**

**You should see a list of all posts:**
```json
[
  {
    "id": 1,
    "title": "My First Post",
    "body": "Hello, this is my first forum post!",
    "userId": 1,
    "author": null,
    "comments": null
  }
]
```

---

## Step 15: Filter Posts by Title

1. Still in **"GET /Posts"**
2. Click **"Try it out"** if needed
3. In the `titleContains` field, type: `First`
4. Click **"Execute"**

**Only posts with "First" in the title are returned!**

---

## 🎉 Congratulations!

You've successfully:
- ✅ Started the Web API
- ✅ Opened Swagger UI
- ✅ Created a user
- ✅ Created a post
- ✅ Added a comment
- ✅ Retrieved data with includes
- ✅ Filtered results

---

## What You Just Learned

- **POST** = Create something new
- **GET** = Retrieve/read something
- **PUT** = Update something (not tested yet)
- **DELETE** = Remove something (not tested yet)

Response codes:
- **200 OK** = Success for GET
- **201 Created** = Success for POST
- **204 No Content** = Success for PUT/DELETE
- **400 Bad Request** = Your data was wrong
- **404 Not Found** = The thing doesn't exist

---

## Try These Next

### Update a User (PUT)
1. Find **"PUT /Users/{id}"**
2. Enter user id: `1`
3. Update the JSON:
```json
{
  "userName": "alice_updated",
  "password": "newpassword"
}
```
4. Execute
5. Expected: `204 No Content`

### Create Another User
1. POST /Users with different username
2. Create a post with that user's ID
3. Get all posts - you'll see posts from different users

### Test Filtering
Try these in GET /Posts:
- `userId=1` - Only posts by user 1
- `titleContains=post` - Posts with "post" in title
- Both together!

---

## Common Mistakes

### ❌ "User with id X not found"
**Problem:** You used a user ID that doesn't exist
**Solution:** Create the user first, use that ID

### ❌ Can't access Swagger
**Problem:** Wrong URL or API not running
**Solution:** Check terminal for correct port, make sure API is running

### ❌ JSON syntax error
**Problem:** Missing comma, quote, or bracket
**Solution:** Copy examples exactly, check for typos

### ❌ Nothing happens when clicking Execute
**Problem:** API might have crashed
**Solution:** Check terminal for errors, restart API

---

## Stopping the API

When you're done testing:
1. Go to the terminal where the API is running
2. Press `Ctrl + C`
3. Type `Y` if asked to terminate

---

## Next Time You Want to Test

1. Open terminal
2. Navigate: `cd "C:\Users\Youna\OneDrive - ViaUC\3 SEM\DNP 1\Aflevering01\WebAPI"`
3. Run: `dotnet run`
4. Open Swagger: `https://localhost:XXXX/swagger`

**Your data is saved!** Users, posts, and comments are stored in JSON files and will still be there.

---

## Where is the Data Stored?

Look in the `WebAPI` folder:
- `users.json` - All users
- `posts.json` - All posts
- `comments.json` - All comments

You can open these files with Notepad to see your data!

---

## Need More Help?

- **Detailed testing:** See TESTING_CHECKLIST.md
- **All endpoints:** See API_QUICK_REFERENCE.md
- **Problems:** See TROUBLESHOOTING.md
- **Understanding:** See PART4_README.md

---

## You're Ready! 🚀

You now know how to:
- Start the API
- Use Swagger UI
- Create, read data
- Test with query parameters
- Verify your implementation works

**Great job!** Part 4 is working! ✨
