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
