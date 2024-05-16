# Initializing project
```bash
dotnet new webapi
```

## Packages Used
### Add Entity Framework package
```bash
dotnet add package Microsoft.EntityFrameworkCore.Tools
```
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```
```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

### Add Identity and Authentication package
```bash
dotnet add package Microsoft.Extensions.Identity.Core
```
```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

```bash
dotnet add package Microsoft.AspNetCore.OpenApi
```

### Install dotnet entity framework cli tool
```bash
dotnet tool install --global dotnet-ef
```


<!-- For a clean build delete "Migrations" directory and Database.db file
    and run the below command in the source directory -->
## For Clean Build
### Database setup
<!-- Initial database migrations -->

```bash
dotnet ef migrations add Init
```
```bash
dotnet ef database update
```

### Build step
```bash
dotnet build
```


# Caution (Important during deployment and run)
In appsettings.json these fields are Important

```json
"JWT": {
    "Issuer": "string",
    "Audience": "string",
    "SigningKey": "string"  //  must be at least 512 bytes (SigningKey must be kept secret)
}
```

