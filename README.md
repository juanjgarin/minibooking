# MiniBooking

Practice project focused on modern architecture using microservices with .NET, React, and Docker.

## Stack

### Backend

* .NET 10 (`net10.0`)
* ASP.NET Core Web API — `Microsoft.AspNetCore.OpenApi` 10.0.6, `Swashbuckle.AspNetCore` 10.1.7
* Entity Framework Core 10.0.7 (`Microsoft.EntityFrameworkCore.SqlServer`)
* SQL Server 2022 (`mcr.microsoft.com/mssql/server:2022-latest` in Compose)
* FluentValidation 11.11.0 (`FluentValidation.AspNetCore` 11.3.1 in API projects)
* AutoMapper 12.0.1

### Frontend

(`web/package.json`)

* React 18.3.x
* TypeScript ~5.6
* Vite 5.4.x
* PrimeReact 10.8.x, PrimeIcons 7.x, PrimeFlex 3.3.x
* React Router 6.28.x
* Axios 1.7.x

### Infrastructure

* Docker / Docker Compose (`deploy/docker-compose.yml`)
* API images: `mcr.microsoft.com/dotnet/sdk:10.0` (build), `mcr.microsoft.com/dotnet/aspnet:10.0` (runtime)
* Web image: Node 22 Alpine (build), nginx 1.27 Alpine (static hosting)

---

## Architecture

```text
React Frontend
       ↓
BookingService API
       ↓ HTTP
CustomerService API
       ↓
SQL Server
```

Each service follows a layered architecture:

```text
Api
Application
Domain
Infrastructure
```

---

## Services

### BookingService

* Create bookings
* Update bookings
* Get bookings
* Change booking status
* Validate customers via HTTP

### CustomerService

* Create customers
* Update customers
* Get customers

---

## Features

* Microservices-based architecture
* HTTP communication between services
* Validation with FluentValidation
* AutoMapper integration
* EF Core + SQL Server
* Dockerized environment
* Cloud-ready structure

---

## Structure

```text
MiniBooking
├── deploy/
├── src/services/
│   ├── BookingService/
│   └── CustomerService/
├── web/
└── MiniBooking.slnx
```

---

## Run locally

```bash
docker compose -f deploy/docker-compose.yml up --build
```

---

## Local URLs

### Frontend

```text
http://localhost:8080
```

### Booking API

```text
http://localhost:5221/swagger
```

### Customer API

```text
http://localhost:5222/swagger
```

---

## Concepts Used

* Docker
* Docker Compose
* Microservices
* Layered Architecture
* Repository Pattern
* Dependency Injection
* DTOs
* Environment Variables
* Cloud-ready containers

---

## Possible Improvements

* JWT Authentication
* API Gateway
* Redis
* Message Broker
* CI/CD
* Automated Tests
* Kubernetes
