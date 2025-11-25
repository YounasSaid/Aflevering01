# QUICK START - Part 7

## 🚀 Start her:

### 1. Åbn projekt
```
Åbn Aflevering01.sln i Rider
```

### 2. Restore packages
```
Højreklik på solution → "Restore NuGet Packages"
```

### 3. Build
```
Build → Rebuild Solution (Ctrl+Shift+B)
```

### 4. Opret database
Åbn Terminal i Rider og kør:
```powershell
cd Server\EfcRepositories
dotnet ef database update
```

### 5. Kør WebAPI
```
Højreklik på WebAPI projekt → Run
Eller tryk F5
```

### 6. Test i Swagger
Swagger UI åbner automatisk. Test:
- POST /users (opret user)
- GET /users (se users)
- POST /posts (opret post)
- GET /posts (se posts)

## ✅ Færdig!

Database: `Server\EfcRepositories\app.db`
Guide: Se `PART7_KOMPLET.md` for detaljer

---

**Hvis noget fejler:** Se `PART7_KOMPLET.md` → "Hvis du får fejl" sektionen
