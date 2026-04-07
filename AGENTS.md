# AGENTS Guide for BizyPopAPIsSln

## Big picture (what talks to what)
- Solution has 6 projects: `BizyPopAPIs` (main API), `ImageStorageAPI` (image microservice), `Application` (business services), `Infrastructure` (EF Core + DB helpers), `Domain` (entities), `Common` (DTOs/mappers/shared models) (`BizyPopAPIsSln.sln`).
- Typical flow is **Minimal API endpoint -> Application service -> `IDbContextFactory<BizyPopDbContext>` extension methods -> Postgres**.
- Endpoint aggregation pattern: feature group files map sub-endpoints (example `BizyPopAPIs/Features/Coupon/CouponEndpoints.cs`).
- Main app wiring lives in `BizyPopAPIs/Program.cs` (DI, auth, CORS, OpenAPI/Scalar, endpoint mapping).

## Service/data patterns to follow
- Prefer `contextFactory.QueryWithDbContextAsync(...)` for reads and `WriteWithDbContextAsync(...)` for writes (`Infrastructure/Extensions/DbContextFactoryExtensions.cs`).
- Use `AsNoTracking()` for read-only projections (seen in `Application/Services/IncentiveServices/Implementations/CouponService.cs`).
- Pagination should use `PaginateAsync(...)` from the same extension class, optionally with `urlMapperFunc` for URL enrichment.
- Caching is first-class via `ICacheService`; list/detail caches use group invalidation (example `CacheKeys.CouponsGroup(...)` in `CouponService`).

## EF Core conventions specific to this repo
- In entity configurations, all string columns should use `.HasColumnType("text")` (from `.github/copilot-instructions.md`; also visible in `Infrastructure/Configurations/BusinessConfiguration.cs`).
- Table names and column names are explicitly mapped and often follow legacy codes (examples: `B01_Business_Profile`, `10A_Coupon_BizDef`).
- Seed/reference data is commonly declared in configuration (`builder.HasData`) instead of migration payload (documented in `STATES_LOOKUP_SYSTEM.md`).

## Image handling boundary (critical)
- `BizyPopAPIs` should not store binary image data; it uploads via `IImageService` to `ImageStorageAPI` and stores returned relative paths (`Application/Services/Implementations/ImageService.cs`).
- Responses should prepend `ImageServer:BaseUrl` when returning URLs (use `GetPublicImageUrl(...)`).
- `ImageStorageAPI` owns file persistence under `wwwroot` and returns paths like `/<folder>/<file>` (`ImageStorageAPI/Services/ImageStorageService.cs`).

## API conventions
- Minimal API endpoints usually return `ApiResponse.SuccessResponse(...)` wrappers and declare `.Produces(...)` metadata (example `BizyPopAPIs/Features/Coupon/CreateCouponEndpoint.cs`).
- Form-data endpoints explicitly use `.DisableAntiforgery()` and `.Accepts<...>("multipart/form-data")` when files are involved.
- Authorization is mostly group-level or endpoint-level via `.RequireAuthorization()`; public lookups use `.AllowAnonymous()` (example `BizyPopAPIs/Features/State/LookupStatesEndpoint.cs`).

## Local workflows agents should use
- Build solution from root:
  - `dotnet build BizyPopAPIsSln.sln`
- Run main API:
  - `dotnet run --project BizyPopAPIs/BizyPopAPIs.csproj`
- Run image service:
  - `dotnet run --project ImageStorageAPI/ImageStorageAPI.csproj`
- Apply migrations:
  - `dotnet ef database update`
- List migrations:
  - `dotnet ef migrations list`
- No test project is currently present in the solution; use build + targeted endpoint/manual verification.

## Migration gotcha (do not miss)
- `Infrastructure/Infrastructure.csproj` has `<IncludeMigrationsInBuild>false</IncludeMigrationsInBuild>` and selectively includes migration files.
- If adding a migration, verify the new migration `.cs` and `.Designer.cs` are included in that project file, or build/runtime migration behavior can drift.

## Config/integration points
- DB + JWT + image server config is in `BizyPopAPIs/appsettings*.json`.
- Image upload target is built from `ImageServer:BaseUrl` + `ImageServer:UploadEndpoint` in `ImageService.BuildUploadUrl(...)`.
- Logging uses custom Postgres app logging setup via `AddPostgresAppLogging(...)` in `BizyPopAPIs/Program.cs`.

