# BookingService

REST API for managing **bookings** (customer, space, date range, status). Part of MiniBooking; targets **.NET 10** (`net10.0`).

## Structure

| Project | Responsibility |
|---------|----------------|
| **BookingService.Api** | ASP.NET Core host, controllers, Swagger, health checks, FluentValidation auto-validation |
| **BookingService.Application** | Use cases, DTOs, AutoMapper profiles, FluentValidation validators |
| **BookingService.Domain** | `Booking` entity and `BookingStatus` enum |
| **BookingService.Infrastructure** | EF Core, SQL Server, `BookingDbContext`, repositories, typed **HttpClient** to CustomerService |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote) reachable from the connection string in `BookingService.Api/appsettings.json`
- **CustomerService** running (or correct `CustomerService:BaseUrl`) before creating new bookings

## Configuration

Set `ConnectionStrings:DefaultConnection` in `BookingService.Api/appsettings.json` or user secrets / environment variables.

**CustomerService** must be reachable for **new** bookings (HTTP `GET` to verify the customer exists). Configure:

```json
"CustomerService": {
  "BaseUrl": "http://localhost:5222",
  "TimeoutSeconds": 30
}
```

`BaseUrl` must match where CustomerService is running (see that service’s `launchSettings.json`). If `BaseUrl` is missing, the API fails at startup when registering the HTTP client.

Example (development):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=MiniBooking_BookingDb;User Id=sa;Password=***;TrustServerCertificate=True"
}
```

## Run

From `BookingService.Api`:

```bash
dotnet ef database update --project ../BookingService.Infrastructure --startup-project .
```

```bash
dotnet run
```

## Docker (stack con CustomerService)

Desde la **raíz del repo** `MiniBooking` (donde están `src/` y `deploy/`):

```bash
docker compose -f deploy/docker-compose.yml up --build
```

- **SQL Server**: `localhost:1433` (misma contraseña `SA_PASSWORD` que en el compose).
- **CustomerService**: `http://localhost:5222` (mapeado al puerto **8080** del contenedor).
- **BookingService**: `http://localhost:5221` — usa `CustomerService__BaseUrl=http://customer-service:8080` dentro de la red Docker.

Las migraciones EF se aplican al arrancar si `APPLY_MIGRATIONS_ON_STARTUP=true` (ya definido en el compose). En local con `dotnet run` puedes omitir esa variable y seguir usando `dotnet ef database update`.

- Swagger UI: `https://localhost:<port>/swagger`
- Health (includes DB check): `GET /health`

## API overview

| Method | Route | Description |
|--------|--------|-------------|
| GET | `/api/bookings` | List all bookings |
| GET | `/api/bookings/{id}` | Get one booking |
| POST | `/api/bookings/upsert` | Create or update a booking (on **create**, customer must exist in CustomerService) |
| PUT | `/api/bookings/changeStatus` | Change `BookingStatus` |

### Validation

- **FluentValidation**: requests are validated automatically; invalid bodies return **400** with error details.
- **Date rule**: `EndDate` must be **strictly after** `StartDate`.
- **Status**: must be a defined `BookingStatus` value (`Pending`, `Confirmed`, `Cancelled` — same integer values as in the domain enum).

### Logging

Application service logs informational messages for list/get/create/update/status changes, and warnings when a booking is not found for status updates or when a create is rejected because the customer does not exist in CustomerService.

## Migrations

EF Core migrations live under `BookingService.Infrastructure/Data/Migrations`. Apply as shown in **Run** above.
