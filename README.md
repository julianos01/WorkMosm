# WorkMosm - User and CarSales Manager for demo

A professional-grade User and loans Management System built with **.NET 8** following **Hexagonal Architecture** (Ports & Adapters) and **Domain-Driven Design (DDD)** principles.

## 🚀 Architecture Overview

This project is structured using Hexagonal Architecture to ensure the core business logic remains independent of external technologies:

* **Domain (The Core):** Contains business entities (like User), value objects, and domain invariants. It is a pure C# library with no external dependencies.
* **Application:** Contains the application services and the definition of "Ports" (interfaces) that the infrastructure must implement.
* **Infrastructure (Adapters):** Implements the "Adapters" for the ports. This includes the PostgreSQL database integration via Entity Framework Core.
* **API (Web Adapter):** The entry point of the application, handling RESTful requests and responses.

## 🛠️ Tech Stack

* **Backend Framework:** .NET 8 (C#)
* **Database:** PostgreSQL (Native UUID support)
* **ORM:** Entity Framework Core
* **Architecture Pattern:** Hexagonal / Ports & Adapters
* **Version Control:** Git & GitHub

## 💎 Key Features

* **Domain Integrity:** Business rules (invariants) are enforced within the Domain entities.
* **UUID Primary Keys:** Strategic use of `Guid` in C# mapped to native `uuid` in PostgreSQL for better scalability and security.
* **Decoupled Persistence:** The database implementation is an adapter that can be replaced without touching the business logic.
* **Clean Migrations:** Database schema versioning managed entirely through EF Core Migrations.

## ⚙️ Setup & Installation

### Prerequisites
* .NET 8 SDK
* PostgreSQL (Running on port 5460 as per current config)
* EF Core CLI Tools (`dotnet tool install --global dotnet-ef`)

### Steps to Run
1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/julianos01/WorkMosM.git](https://github.com/julianos01/WorkMosM.git)
    cd WorkMosM
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Apply Database Migrations:**
    ```bash
    dotnet ef database update --project Infrastructure/Infrastructure.csproj --startup-project WorkMosMApi/WorkMosmApi.csproj
    ```

4.  **Launch the API:**
    ```bash
    dotnet run --project WorkMosMApi/WorkMosmApi.csproj
    ```

## 📝 License

This project is licensed under the **MIT License** - see the LICENSE file for details.

---
*Developed as part of a professional software engineering portfolio.*
