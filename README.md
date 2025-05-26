# Event Booking App

An event booking API built with **ASP.NET Core 8** and **PostgreSQL**. This application supports user-based event creation, bookings, and wallet features, providing a robust backend for your event management needs.

---

## Features

* **User Authentication & Authorization**: Secure access to API endpoints using JWT Bearer tokens.
* **Event Management**: Create, view, update, and delete events.
* **Booking System**: Allow users to book available events.
* **Wallet System**: Manage user balances for seamless payments and transactions.
* **PostgreSQL Integration**: Reliable data persistence using a powerful open-source relational database.
* **Clean Architecture**: Structured for maintainability, testability, and separation of concerns.
* **Dockerized Development**: Easy setup and consistent environment with Docker and Docker Compose.
* **API Documentation**: Swagger/OpenAPI for interactive API exploration.

---

## Tech Stack

* [.NET 8](https://dotnet.microsoft.com/)
* [Entity Framework Core](https://learn.microsoft.com/en-us/ef/) for ORM
* [PostgreSQL](https://www.postgresql.org/) as the database
* [Docker](https://docs.docker.com/) & [Docker Compose](https://docs.docker.com/compose/) for containerization
* [JWT Bearer Authentication](https://jwt.io/) for API security
* [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL) for PostgreSQL EF Core provider
* [Swagger/OpenAPI](https://swagger.io/docs/specification/about/) for API documentation

---

## Getting Started (Dockerized Setup) Option 1

This project is set up to be launched effortlessly using Docker Compose, providing a consistent development environment.

###  1. Clone the Project

First, get a copy of the repository:

```bash
git clone [https://github.com/your-username/EventBookingApp.git](https://github.com/your-username/EventBookingApp.git)
cd EventBookingApp
---
```
###  2. Launch with Docker Compose

From the root of the `EventBookingApp` directory, run the following command:

```bash
docker-compose up --build
```

This command will:

   - Build the .NET 8 API application's Docker image.
   - Start a PostgreSQL database container.
   - Run Entity Framework Core migrations automatically, creating or updating your database schema.
   - Launch the ASP.NET Core API server.

    The API server should be accessible at:
    http://localhost:5128

---

## Getting Started (Local Setup) Option 2

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- A tool like [pgAdmin](https://www.pgadmin.org/) (optional, for DB visualization)

---
###  1. Clone the Project

First, get a copy of the repository:

```bash
git clone [https://github.com/AyemDan/VendorCredit_EventManagementAPP.git]
cd EventBookingApp
---
```
###  2. Download Dependencies and build 

```bash
dotnet build
dotnet ef migrations add InitialMigration
dotnet ef database update \
  --project EventBookingApp.Infrastructure \
  --startup-project EventBookingApp.API

cd EventBookingApp.API
dotnet run --Project EventBookingApp.API.csproj


```

---

####  Configuration

The application uses `appsettings.development.json` or  `appsettings.json` for configuration. For Dockerized setup, sensitive information like database credentials should ideally be managed via **environment variables** or Docker secrets, but for local development, `appsettings.json` is sufficient.

---

#### Database Connection

Ensure your `appsettings.development.json` in `EventBookingApp.API` has the correct PostgreSQL connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=your_postgres_db;Username=your_username;Password=your_password"
  },
  // ... other settings
}
```
Replace your_postgres_db, your_username, and your_password with your actual database credentials.

---

#### API Usage
The application uses JWT Bearer Authentication for most of its endpoints.

- Register/Login: Start by registering a new user or logging in to obtain a JWT token. This token will be required for accessing protected resources.
- Include Token: Include the obtained JWT token in the Authorization header of your subsequent API requests, prefixed with Bearer (e.g., Authorization: Bearer <your_jwt_token>).
API Documentation (Swagger)
- Once the application is running, you can explore the API endpoints and test them interactively using Swagger UI:

http://localhost:5128/swagger

---

#### 🔄 EF Core Migrations (Manual)
While docker-compose up typically handles migrations, if you need to manage them manually (e.g., adding new migrations, removing, or updating to specific versions), you can do so from within the running api container.

Navigate to your solution root directory in your terminal before running these commands.

Add a New Migration
```bash

docker-compose exec api dotnet ef migrations add YourMigrationName \
  --project EventBookingApp.Infrastructure \


docker-compose exec api dotnet ef database update \
  --project EventBookingApp.Infrastructure \

```
---

## Testing with Postman

You can easily test all endpoints using our pre-configured Postman collection.

###  Postman Collection

 [Click here to open the Postman Collection](https://www.postman.com/speeding-astronaut-625283/workspace/eventapptest/collection/25140662-46ddf63a-5a77-4ef5-aa0e-a46aeaea9ab8?action=share&creator=25140662)


### 🛡 Authentication

1. Register or login via the `/api/auth/register` or `/api/auth/login` endpoint.
2. Copy the JWT token from the response.
3. In Postman, click **Authorization** tab → Select **Bearer Token** → Paste your token.
4. You're now authenticated and can access all protected routes!

>Note : A Script hass already been made to copy the token into the "authToken" variable
just make sure to put {{authToken}} in the token area of the Authorization section. So you can Skip number 2 & 3

---
> ⚠️ **Important Note:**  
> No matter which setup method you choose (Docker or Local), **you must run Entity Framework Core migrations** to ensure the database schema is correctly created or updated. If you dont it wont work properly.
>
---
