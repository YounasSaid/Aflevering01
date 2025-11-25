# ✅ PART 7 - KOMPLET! 

## 🎉 Hvad jeg har gjort:

### 1. ✅ Oprettet EfcRepositories projekt komplet med:
- EfcRepositories.csproj (med alle NuGet packages)
- AppContext.cs (DbContext)
- EfcUserRepository.cs
- EfcPostRepository.cs  
- EfcCommentRepository.cs

### 2. ✅ Opdateret alle Entities med navigation properties:
- User.cs (med Posts og Comments navigation properties)
- Post.cs (med User og Comments navigation properties)
- Comment.cs (med User og Post navigation properties)

### 3. ✅ Opdateret WebAPI:
- Program.cs (bruger nu EFC repositories + DbContext)
- WebAPI.csproj (reference til EfcRepositories)

### 4. ✅ Opdateret Solution:
- Aflevering01.sln (inkluderer EfcRepositories projekt)

### 5. ✅ Oprettet Migrations:
- 20241125000000_InitialCreate.cs
- 20241125000000_InitialCreate.Designer.cs
- AppContextModelSnapshot.cs

## 🎯 Hvad du skal gøre i Rider:

### Step 1: Genåbn Solution
1. Luk Rider helt ned
2. Åbn Rider igen
3. Åbn `Aflevering01.sln`
4. Rider skulle nu se det nye EfcRepositories projekt

### Step 2: Restore & Build
I Rider, højreklik på solution og vælg:
1. "Restore NuGet Packages" (vent til det er færdigt)
2. Build → "Rebuild Solution" (Ctrl+Shift+B)

**FORVENT NOGLE WARNINGS** - det er OK! Så længe der ikke er ERRORS er det fint.

### Step 3: Opret Database
Åbn Terminal i Rider (View → Tool Windows → Terminal) og kør:

```powershell
cd "Server\EfcRepositories"
dotnet ef database update
```

Dette opretter `app.db` filen.

### Step 4: Verificer Database fil
Tjek at filen `Server\EfcRepositories\app.db` nu eksisterer.

### Step 5: Kør WebAPI
1. I Rider, højreklik på **WebAPI** projekt
2. Vælg "Run 'WebAPI'"
3. Eller tryk F5

Swagger UI burde åbne automatisk i din browser.

### Step 6: Test API
I Swagger UI:

1. **Opret en user:**
   - POST /users
   - Body:
   ```json
   {
     "userName": "testuser",
     "password": "password123"
   }
   ```
   - Klik "Execute"
   - Du skulle få tilbage en user med Id = 1

2. **Hent users:**
   - GET /users
   - Klik "Execute"
   - Du skulle se din user

3. **Opret en post:**
   - POST /posts
   - Body:
   ```json
   {
     "title": "Min første post",
     "body": "Dette er en test post",
     "userId": 1
   }
   ```
   - Klik "Execute"

4. **Hent posts:**
   - GET /posts
   - Du skulle se din post

### Step 7: Verificer Database (OPTIONAL)
Hvis du vil se data i databasen:

I Rider:
1. View → Tool Windows → Database
2. Klik "+" → Data Source → SQLite
3. Browse til: `Server\EfcRepositories\app.db`
4. Klik OK
5. Udvid databasen → Udvid "main" → Se tabellerne
6. Dobbeltklik på "Users" tabel for at se data

## 🐛 Hvis du får fejl:

### Fejl: "Unable to create an object of type 'AppContext'"
**Løsning:** Installer dotnet-ef tools:
```powershell
dotnet tool install --global dotnet-ef
```

### Fejl: "A cycle was detected"
**Løsning:** Tilføj til `Program.cs` i WebAPI (EFTER `builder.Services.AddControllers()`):

```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
```

### Fejl: "SQLite Error: database is locked"
**Løsning:** Luk alle vinduer der har databasen åben (Database tool window i Rider, DataGrip, etc.)

### Fejl: Build fejler med "Navigation property must have a setter"
**Løsning:** Allerede fixet! Alle navigation properties har både get og set.

### Fejl: "Could not find a part of the path"
**Løsning:** Stien i AppContext.cs er måske forkert. Tjek at denne linje passer:
```csharp
optionsBuilder.UseSqlite(@"Data Source=C:\Users\Youna\OneDrive - ViaUC\3 SEM\DNP 1\Aflevering01\Server\EfcRepositories\app.db");
```

## ✅ Success Kriterier

Part 7 er færdig når:
- ✅ Solution builder uden ERRORS
- ✅ app.db fil eksisterer i Server\EfcRepositories\
- ✅ WebAPI starter uden fejl
- ✅ Du kan oprette en user via Swagger
- ✅ Du kan se useren ved at hente GET /users
- ✅ Du kan oprette en post via Swagger
- ✅ Data persisterer (stop WebAPI, start igen, data er stadig der)

## 🎊 Når alt virker:

### Næste steps:
1. ✅ Test alle endpoints (Users, Posts, Comments)
2. ✅ Opdater Blazor app til at virke med EFC API
3. ✅ Commit og push til GitHub
4. ✅ Fejr at Part 7 er færdig! 🎉

### Forbedringer du kan lave (OPTIONAL):
1. Opdater controllers til at bruge `.Include()` for at loade navigation properties
2. Tilføj seed data i AppContext
3. Tilføj indices til ofte søgte kolonner
4. Implementer soft delete i stedet for hard delete

## 📸 Screenshots til aflevering:

Tag screenshots af:
1. Solution Explorer der viser EfcRepositories projekt
2. Migrations mappen med filer
3. Database view der viser tabeller
4. Swagger UI med successful API calls
5. Data i databasen (Users, Posts, Comments tabeller)

Held og lykke! Du er næsten færdig! 🚀

---

**Sidst opdateret:** 25. November 2024
**Status:** Part 7 - Entity Framework Core - KOMPLET ✅
