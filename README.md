# MiniBooking

Practice project focused on modern architecture using microservices with .NET, React, and Docker.

## Stack

### Backend

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* FluentValidation
* AutoMapper

### Frontend

* React
* TypeScript
* Vite
* PrimeReact

### Infrastructure

* Docker
* Docker Compose

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
