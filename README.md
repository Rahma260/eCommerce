# 🛒 E-Commerce Backend API

A **production-ready E-Commerce Backend API** built with **ASP.NET Core** following **Clean Architecture principles**.  
The system handles secure authentication, cart management, order processing, and online payments with a strong focus on **scalability, consistency, and maintainability**.

🔗 **Repository:** https://github.com/Rahma260/eCommerce

---

## 🚀 Features

### 🔐 Authentication & Authorization
- User registration and login using **ASP.NET Identity**
- **JWT-based authentication**
- **Google OAuth** for social login
- **Email-based OTP verification** for secure account actions
- Role-based authorization

### 🛍️ Cart & Checkout
- **Redis-based shopping cart** for high-performance caching
- Cart persistence and synchronization per user
- Seamless transition from cart to order

### 💳 Payments
- **Stripe payment integration**
- Secure payment intent creation
- Automatic order status updates after successful payment

### 📦 Orders
- Full **order lifecycle management**
  - `Pending`
  - `Paid`
  - `Cancelled`
- Reliable order creation with transactional consistency

### 🖼️ Media Management
- Image upload and management using **Cloudinary**

### 📨 Background Processing
- **Hangfire** for background jobs:
  - Sending OTP and notification emails
  - Async tasks without blocking requests

### 📊 Observability & Validation
- **Serilog** for structured logging and diagnostics
- **FluentValidation** for clean and reusable request validation

---

## 🧠 System Design & Logic

- **Clean Architecture**
  - Clear separation of concerns (Domain, Application, Infrastructure, API)
  - Business logic isolated from frameworks and external services
- **Checkout Workflow**
  - Redis is used for fast cart access
  - SQL Server stores finalized orders
  - Payment confirmation guarantees **consistency between payment state and order status**
- **Scalable & Maintainable**
  - AutoMapper for DTO mapping
  - Unit of Work & Repository patterns
  - Easily extensible for new features (discounts, shipping, admin dashboards)

---

## 🛠️ Tech Stack

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Redis

### Authentication & Security
- ASP.NET Identity
- JWT
- Google OAuth

### Payments & Storage
- Stripe
- Cloudinary

### Background & Infrastructure
- Hangfire
- SMTP Email Service

### Utilities
- AutoMapper
- FluentValidation
- Serilog

---

## 📂 Project Structure (Clean Architecture)

```txt
src/
│
├── Domain
│   └── Entities, Enums, Interfaces
│
├── Application
│   └── DTOs, Services, Business Logic
│
├── Infrastructure
│   └── EF Core, Redis, Identity, Stripe, Cloudinary
│
└── API
    └── Controllers, Middleware, Authentication
