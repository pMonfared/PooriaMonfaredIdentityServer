# PooriaMonfaredIdentityServer (Beta) in progress...

##Customize Identity Server 4 + asp.net Core App

###Create database:

#####setup your custom connectionString in appsetting.json in PooriaMonfaredIdentityServer.Web
#####run CMD from location of PooriaMonfaredIdentityServer.DataLayer
######and Enter this commands(one by one):
```
dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb
dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb
dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ migrations add InitialIdentityServerMigration -c ApplicationDbContext

dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ database update -c ApplicationDbContext

dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ database update -c PersistedGrantDbContext
dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ database update -c ConfigurationDbContext
```


##Exmaple OpenIdConnect as a web app dotnetcore from here:
https://github.com/pMonfared/PooriaMonfaredIdentityClientSample

####Demo user info:
username: pooria
password: newPass@1234

#####have fun
