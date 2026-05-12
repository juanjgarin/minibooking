# MiniBooking Web

React + TypeScript + Vite + PrimeReact UI for the MiniBooking APIs.

## Setup

```bash
npm install
```

Copy `.env.example` to `.env` and set:

- `VITE_BOOKING_API_URL` — BookingService base URL (default `http://localhost:5221`)
- `VITE_CUSTOMER_API_URL` — CustomerService base URL (default `http://localhost:5222`)

APIs must allow the dev origin (`http://localhost:5173`) and the Dockerized UI (`http://localhost:8080`) via `AllowedOrigins` in their `appsettings.json`.

## Docker (Nginx, full stack)

From repo root `MiniBooking`:

```bash
docker compose -f deploy/docker-compose.yml up --build
```

Open **http://localhost:8080**. The SPA is built with `VITE_*` pointing at `http://localhost:5221` and `http://localhost:5222` (host ports published for the APIs). If you change those ports in `deploy/docker-compose.yml`, update the `web.build.args` and rebuild the `web` service.

## Scripts

```bash
npm run dev      # Vite dev server (port 5173)
npm run build    # Typecheck + production bundle
npm run preview  # Preview production build
```

## Routes

- `/dashboard` — summary cards
- `/customers` — CRUD table + modal
- `/bookings` — table, status tags, modal, quick status actions on selection
