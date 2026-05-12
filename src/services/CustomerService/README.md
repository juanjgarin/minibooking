# CustomerService

REST API for **customers** (`FullName`, `Email`, `Phone`). Same layered layout as BookingService: **Api**, **Application**, **Domain**, **Infrastructure**; **.NET 10** (`net10.0`).

## Endpoints

| Method | Route | Description |
|--------|--------|-------------|
| GET | `/api/customers` | List all customers |
| GET | `/api/customers/{id}` | Get one customer (**404** if missing) |
| POST | `/api/customers/upsert` | Create or update |
| DELETE | `/api/customers/{id}` | Delete (**404** if missing) |

- **FluentValidation** on `SaveCustomerRequest` (email format, lengths, required fields).
- **Health**: `GET /health` (includes SQL Server check via EF).
- **Swagger**: `/swagger` in Development.

## Database

Connection string key: `ConnectionStrings:DefaultConnection` (see `appsettings.json`). Default database name: `MiniBooking_CustomerDb`.

Apply migrations from `CustomerService.Api`:

```bash
dotnet ef database update --project ../CustomerService.Infrastructure --startup-project .
```

## Local ports

Default launch profile uses **http://localhost:5222** (and **https://localhost:7138** for the `https` profile). **BookingService** should point `CustomerService:BaseUrl` at the same URL you run this API on.

## Docker (stack completo)

Desde la raíz del repo `MiniBooking`:

```bash
docker compose -f deploy/docker-compose.yml up --build
```

CustomerService queda en **http://localhost:5222**. El compose levanta también SQL Server y BookingService; las migraciones se aplican en arranque cuando `APPLY_MIGRATIONS_ON_STARTUP=true`.
