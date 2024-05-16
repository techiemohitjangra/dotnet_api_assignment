### Initializing project
dotnet new webapi

## Packages Used
### Add Entity Framework package
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

### Add Identity and Authentication package
dotnet add package Microsoft.Extensions.Identity.Core
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer


dotnet add package Microsoft.AspNetCore.OpenApi

### Install dotnet entity framework cli tool
dotnet tool install --global dotnet-ef


<!-- For a clean build delete "Migrations" directory and Database.db file
    and run the below command in the source directory -->
## For Clean Build
### Database setup
<!-- Initial database migrations -->
dotnet ef migrations add Init
dotnet ef database update

### Build step
dotnet build


# Caution (Important during deployment and run)
In appsettings.json these fields are Important

```json
"JWT": {
    "Issuer": "string",
    "Audience": "string",
    "SigningKey": "string"  //  must be at least 512 bytes (SigningKey must be kept secret)
}
```

