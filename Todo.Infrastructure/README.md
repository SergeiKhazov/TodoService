### Environments
```
set ASPNETCORE_ENVIRONMENT=Local
set ASPNETCORE_ENVIRONMENT=Development
```
###Setting environments in PowerShell
$Env:ASPNETCORE_ENVIRONMENT = "Development"

### Creating a migration

```
set ASPNETCORE_ENVIRONMENT=Local
dotnet ef migrations add <MIGRATION_NAME> -p .\Todo.Infrastructure\ -s .\Todo.Api\ --context TodoContext
```

### Removing a migration
```
dotnet ef migrations remove -p .\Todo.Infrastructure\ -s .\Todo.Api\ --context TodoContext
```

### Updating a migration
```
###If we going to add index to big sets of data, we should use extended period of Timeout: 
;CommandTimeout=600

set ASPNETCORE_ENVIRONMENT=Local
dotnet ef database update -p .\Todo.Infrastructure\ -s .\Todo.Api\ --context TodoContext
```
