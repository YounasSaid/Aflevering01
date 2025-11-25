# Part 7 - Næste Steps Guide

## ✅ Hvad jeg har gjort for dig:

1. ✅ Oprettet `Server\EfcRepositories` mappen
2. ✅ Oprettet `EfcRepositories.csproj` med alle NuGet packages
3. ✅ Oprettet `AppContext.cs` (DbContext)
4. ✅ Oprettet `EfcUserRepository.cs`
5. ✅ Oprettet `EfcPostRepository.cs`
6. ✅ Oprettet `EfcCommentRepository.cs`
7. ✅ Opdateret `User.cs` med navigation properties
8. ✅ Opdateret `Post.cs` med navigation properties
9. ✅ Opdateret `Comment.cs` med navigation properties
10. ✅ Opdateret `Aflevering01.sln` med EfcRepositories projekt
11. ✅ Opdateret `WebAPI\Program.cs` til at bruge EFC repositories
12. ✅ Opdateret `WebAPI\WebAPI.csproj` med reference til EfcRepositories

## 🎯 Hvad du skal gøre nu:

### Step 1: Genåbn Solution i Rider

1. Luk Rider helt ned hvis den er åben
2. Åbn Rider igen
3. Åbn `Aflevering01.sln`
4. Rider skulle nu genkende det nye EfcRepositories projekt

### Step 2: Restore NuGet Packages

I Rider:
- Højreklik på solution
- Vælg "Restore NuGet Packages"

Eller åbn Terminal i Rider (View → Tool Windows → Terminal) og kør:
```
dotnet restore
```

### Step 3: Build Solution

I Rider:
- Tryk `Ctrl+Shift+B`
- Eller vælg Build → Build Solution

Eller i Terminal:
```
dotnet build
```

**Forvent muligvis nogle fejl her** - det er normalt! Fortsæt til næste step.

### Step 4: Installer dotnet-ef tools (kun første gang)

Åbn Terminal i Rider og kør:
```
dotnet tool install --global dotnet-ef
```

Hvis det allerede er installeret:
```
dotnet tool update --global dotnet-ef
```

### Step 5: Naviger til EfcRepositories mappen

I Terminal:
```
cd "Server\EfcRepositories"
```

### Step 6: Opret Migration

Kør:
```
dotnet ef migrations add InitialCreate
```

Dette skulle oprette en `Migrations` mappe med migrationsfiler.

### Step 7: Opret Database

Kør:
```
dotnet ef database update
```

Dette opretter `app.db` filen i EfcRepositories mappen.

### Step 8: Naviger tilbage til projekt roden

```
cd ..\..
```

### Step 9: Build igen

```
dotnet build
```

Nu skulle alt compile uden fejl!

### Step 10: Kør WebAPI

I Rider:
- Højreklik på WebAPI projekt
- Vælg "Run"
- Eller tryk F5

Swagger UI skulle åbne automatisk.

### Step 11: Test API

1. I Swagger UI, prøv at oprette en user med POST /users
2. Prøv at hente users med GET /users
3. Opret en post med POST /posts
4. Tjek at data gemmes i databasen

### Step 12: Verificer Database

I Rider:
1. Gå til View → Tool Windows → Database
2. Klik på "+" → Data Source → SQLite
3. Browse til: `Server\EfcRepositories\app.db`
4. Klik OK
5. Åbn tabellerne og se at data er der!

## 🔧 Hvis du får fejl:

### Fejl: "Navigation property must have a setter"
Løsning: Allerede fixet - alle navigation properties har `set;`

### Fejl: "Cycles were detected"
Dette sker når du returnerer entities direkte fra controllers. Tilføj til `Program.cs`:

```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
```

### Fejl: "Unable to create migration"
Sørg for du er i `Server\EfcRepositories` mappen når du kører migration commands.

### Fejl: "SQLite Error: database is locked"
Luk alle programmer der har databasen åben (Rider Database view, DataGrip, etc.)

## 🎉 Success Kriterier

Du ved det virker når:
- ✅ Solution builder uden fejl
- ✅ Migrations mappe eksisterer
- ✅ app.db fil eksisterer
- ✅ WebAPI starter uden fejl
- ✅ Du kan oprette en user i Swagger
- ✅ Du kan se useren i databasen
- ✅ Data persisterer efter restart af WebAPI

## 📝 Næste opgaver efter Part 7 virker:

1. Opdater controllers til at bruge `.Include()` for at loade navigation properties
2. Test alle endpoints grundigt
3. Opdater Blazor app til at virke med den nye API
4. Commit og push til GitHub

Held og lykke! 🚀
