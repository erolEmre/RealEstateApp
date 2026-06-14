![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-purple)
![EF Core](https://img.shields.io/badge/EF_Core-ORM-green)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-red)
![Docker](https://img.shields.io/badge/Docker-Containerization-blue)
![Auth0](https://img.shields.io/badge/Auth0-Authentication-orange)

# 🏠 Real Estate Management System

A modern real estate listing management application built with **ASP.NET Core MVC**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

---

## ✨ Features

- 🔐 Authentication and authorization with Auth0
- 🏡 Create, update, delete and manage property listings
- 👨‍💼 Employee-based property ownership
- 📍 Address management using Value Objects
- 🔄 Entity ↔ ViewModel mapping with AutoMapper
- 🗄️ MSSQL database integration
- 🎨 Responsive UI with Tailwind CSS
- 🐳 Docker & Docker Compose support

---

## 🛠️ Tech Stack

### Backend

- ASP.NET Core MVC
- C#
- Entity Framework Core
- Microsoft SQL Server
- Auth0 Authentication
- AutoMapper

### Architecture & Design Patterns

- Clean Architecture
- Domain-Driven Design (DDD)
- Repository Pattern
- Dependency Injection

### Frontend

- Tailwind CSS
- Razor Views
- JavaScript

### DevOps

- Docker
- Docker Compose

## 🏗️ Architecture

```text
RealEstate
│
├── RealEstate.Core
│   ├── Entities
│   ├── ValueObjects
│   └── Interfaces
│
├── RealEstate.Application
│   ├── DTOs
│   ├── Services
│   └── Mappings
│
├── RealEstate.Infrastructure
│   ├── Data
│   ├── Repositories
│   └── Persistence
│
└── RealEstate.WebUI
    ├── Controllers
    ├── ViewModels
    ├── Views
    └── wwwroot
```
## 🚀 Getting Started

### Prerequisites

Before running the project, make sure the following tools are installed:

* .NET 8 SDK
* Docker Desktop
* Git

---

### Clone the Repository

```bash
git clone https://github.com/erolEmre/RealEstateApp.git
cd RealEstateApp
```

---

### Run with Docker

The application and SQL Server database can be started using Docker Compose.

```bash
docker compose up -d
```

Verify that the containers are running:

```bash
docker ps
```

Stop the containers:

```bash
docker compose down
```

---

### Access the Application

After the containers are started, open your browser and navigate to:

```text
http://localhost:8080
```

or

```text
https://localhost:8081
```

(depending on your Docker configuration)

---

### Demo Account

For testing purposes, you can use the following account:

**Email**

```text
Email: available in Auth0 demo tenant
```

**Password**

```text
Password: available upon request
```

---

### Authentication

The project uses **Auth0** for authentication and authorization.

Authenticated users are mapped to local Employee records and can manage only the property listings they own.
