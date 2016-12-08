# PooriaMonfaredIdentityServer

Custom Identity Server 4 + asp.net Core App

for Create Database:

setup your custom connectionString in appsetting.json in PooriaMonfaredIdentityServer.Web
run CMD from location of PooriaMonfaredIdentityServer.DataLayer
and Enter this commands:

dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb
dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb
dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ migrations add InitialIdentityServerMigration -c ApplicationDbContext

dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ database update -c ApplicationDbContext

dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ database update -c PersistedGrantDbContext
dotnet ef --startup-project ../src/PooriaMonfaredIdentityServer.Web/ database update -c ConfigurationDbContext

