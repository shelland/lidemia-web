# AGENTS.md

## Overview
.NET 10 / ASP.NET Core MVC solution (modular monolith) orchestrated by .NET Aspire. Use the `.slnx` solution file — there is no `.sln`.

## Commands
- Build: `dotnet build Lidemia.slnx`
- Run everything web-facing via Aspire: `dotnet run --project Lidemia.AppHost` (starts `Lidemia` web, `Lidemia.Admin`, `Lidemia.Storage`; dashboard on http://localhost:15137)
- Run a single app: `dotnet run --project Lidemia` (env `Local`, http://localhost:51000), `dotnet run --project Lidemia.Scheduler`, `dotnet run --project Lidemia.Processor`
- There are no test projects, no CI, and no lint/format scripts. `dotnet build` is the only verification step.

## Architecture
- `Lidemia/Program.cs` is the main MVC web app; it wires every module via `RegisterModules` in `Lidemia/Logic/Extensions/ModulesExtensions.cs`.
- Modules: `Lidemia.Core` + `Lidemia.Resources` (base), `Lidemia.DataAccess`, `Lidemia.Common`, `Lidemia.Common.BusinessLogic`, `Lidemia.Web.BusinessLogic`, `Lidemia.Search`, `Lidemia.EmailNotifications`. Dependencies flow inward toward `Core`/`DataAccess`.
- `Lidemia.AppHost/AppHost.cs` is Aspire orchestration (not a `Program.cs`); it references only Web/Admin/Storage.
- `Lidemia.Scheduler` runs `ProcessPendingDiscountsJob`; `Lidemia.Processor` hosts the MassTransit bus (logging currently disabled).

## DI convention (important)
Register services with Scrutor's attribute plus marker-interface scan — do not hand-write `AddScoped`:
`[ServiceDescriptor<IFoo>(ServiceLifetime.Scoped)]` on `Foo : IFoo` (needs `using Scrutor;`). Each module has an `AddXModule()` extension that scans its own assembly through a marker interface (`ICommonModule`, `IWebModule`, `IWebBusinessLogicModule`, ...). Add the attribute and the service is auto-registered; only edit a module file when introducing a new assembly.

## Data access
- EF Core + PostgreSQL (Npgsql): snake_case naming, `NoTracking` by default, NetTopologySuite geography, dynamic JSON.
- Migrations live in `Lidemia.DataAccess/Migrations` and auto-apply on web startup (`RunMigrations` -> `IAppDbMigrator`). Add one with `dotnet ef migrations add <Name> --project Lidemia.DataAccess` (the design-time factory `LidemiaDbContextFactory` supplies a hardcoded localhost connection string). `dotnet-ef` is not pinned in a tool manifest.
- `Scripts/Initial.sql` and `Scripts/Seed.sql` are embedded and run through `MigrationBuilder.PreInit`/`PostInit`.

## Local prerequisites / config
`appsettings.Local.json` (checked in, used when the web app runs with `ASPNETCORE_ENVIRONMENT=Local`) requires: PostgreSQL `127.0.0.1:5432` db `lidemia-dev`, RabbitMQ `127.0.0.1:5672` vhost `lidemia-dev`, Seq `http://127.0.0.1:5341`, OpenSearch (`LocalServices:OpenSearch`), and MinIO for object storage. `RegisterBus` reads Rabbit config unconditionally, so missing Rabbit settings fail app startup.

## Conventions / quirks
- `.editorconfig`: 4-space indent, CRLF, no final newline, block-scoped namespaces, braces required, explicit types (`var` is off by convention).
- Styles are authored as `.less` in `Lidemia/wwwroot/Pages/Styles` and compiled to `Main.css`/`Main.min.css` by Visual Studio Web Compiler (`compilerconfig.json`); the generated CSS is committed, so update it when changing LESS.
- Client libraries are managed with `Lidemia/libman.json`. Localization supports `ru`/`en` (default `en`); resources are in `Lidemia.Resources/App/AppResources.resx`.
- `Lidemia/appsettings.json` contains a checked-in reCAPTCHA key pair (dev, not a production secret).
