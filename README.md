# Ambev Developer Evaluation API

> **Sales API** built with **DDD + Clean/Onion**, **MediatR**, **EF Core**, **FluentValidation**, **AutoMapper**, **JWT (Roles)** and **NSwag/Swagger**.

![Build](https://img.shields.io/badge/build-passing-brightgreen.svg) ![Tests](https://img.shields.io/badge/tests-coverage--pending-lightgray.svg) ![License](https://img.shields.io/badge/license-MIT-blue.svg)

Default dev server: **https://localhost:7181**  
Swagger UI: **`/swagger`**

---

## Table of Contents

- [Overview](#overview)
- [Business Rules](#business-rules)
- [Domain Model](#domain-model)
- [Authentication & Authorization](#authentication--authorization)
- [Endpoints](#endpoints)
- [How to Run (Docker)](#how-to-run-docker)
- [EF Core Migrations](#ef-core-migrations)
- [Request Examples](#request-examples)
- [Testing](#testing)
- [Troubleshooting](#troubleshooting)
- [License](#license)

---

## Overview

This repository implements a **Sales records API** with full CRUD and quantity-based discount business rules. The solution follows **Domain-Driven Design**, clearly splitting **Domain**, **Application**, **Infrastructure/ORM**, and **WebApi** layers.

**Key technologies**
- .NET (Web API) • MediatR (light CQRS) • EF Core (PostgreSQL)
- FluentValidation • AutoMapper (DTO↔Result; no Command → Entity mapping)
- JWT Bearer + Roles (Customer, Manager, Admin)
- NSwag/Swagger (OpenAPI documentation)

---

## Business Rules

Quantity-based **per product** (same `ProductId`) discount:

| Quantity Range | Discount |
|---|---|
| 1–3 units | **0%** |
| 4–9 units | **10%** |
| 10–20 units | **20%** |
| > 20 units | **Forbidden** |

Other rules:
- **Max 20 units** per product in a sale.
- **Item total** = `Quantity * UnitPrice * (1 - Discount)`, rounded to 2 decimals (AwayFromZero).
- **Sale total** = sum of **TotalPrice** of **non-cancelled** items.
- `IsCancelled` flag on both sale and item; cancelled items remain for audit/history.

Domain events (optional/logged): `SaleCreated`, `SaleModified`, `SaleCancelled`, `ItemCancelled`.

---

## Domain Model

**Sale**
- `Id` (Guid)
- `SaleNumber` (Guid)
- `CreatedAt` (**DateTime**) — sale timestamp (as per OpenAPI contract)
- `CustomerId` (Guid) • `BranchId` (Guid)
- `TotalAmount` (decimal, computed)
- `IsCancelled` (bool)
- `Items`: collection of **SaleItem**

**SaleItem**
- `Id` (Guid) 
- `ProductId` (Guid)
- `Quantity` (int)
- `UnitPrice` (decimal)
- `Discount` (decimal, **percentage**: 0, 10, 20)
- `TotalPrice` (decimal, computed per rule)
- `IsCancelled` (bool)

**Important:** handlers **do not** map Commands → Domain entities. Instead, they call **aggregate methods**:
- `AddOrIncrementItem(productId, quantity, unitPrice, policy)`
- `SetQuantity(...)` / `SetUnitPrice(...)`
- `ReplaceItems(...)`
- `CancelItem(...)` / `Cancel()`  
  This preserves invariants and discount calculation in every code path.

---

## Authentication & Authorization

- JWT Bearer with a **Role** claim (enum: `Customer`, `Manager`, `Admin`).
- NSwag configured for **Bearer**; in Swagger click **Authorize** and paste **only the token**.
- Role validation is enforced on endpoints (see table below).

**Token generation**
- Endpoint: `POST /api/Auth`
- Request: `{ "email": "...", "password": "..." }`
- Response: `{ "token": "...", "email": "...", "name": "...", "role": "Manager" }`

**Main token claims**
- `nameidentifier` (UserId), `name` (Username), `role` (Role)

---

## Endpoints

Base URL: `https://localhost:7181`

| Method | Route | Operation | Roles |
|---|---|---|---|
| POST | `/api/Users` | Create user | Admin |
| GET | `/api/Users/{id}` | Get user | Admin |
| DELETE | `/api/Users/{id}` | Delete user | Admin |
| POST | `/api/Sales` | Create sale | Customer, Manager, Admin |
| GET | `/api/Sales` | List sales (pagination `_page`,`_size`) | Customer, Manager, Admin |
| GET | `/api/Sales/{id}` | Get sale details | Customer, Manager, Admin |
| PUT | `/api/Sales/{id}` | Update sale | Customer, Manager, Admin |
| POST | `/api/Sales/{id}/cancel` | Cancel sale | Manager, Admin |
| POST | `/api/Sales/{id}/items` | Add item | Customer, Manager, Admin |
| PUT | `/api/Sales/{id}/items/{itemId}` | Update item | Customer, Manager, Admin |
| POST | `/api/Sales/{id}/items/{itemId}/cancel` | Cancel item | Manager, Admin |
| POST | `/api/Auth` | Authenticate (issue JWT) | Public |

**Security scheme (Swagger/NSwag):** HTTP **bearer** (JWT).

---

## How to Run (Docker)

### Prerequisites
- Docker installed
- Run:
```bash
docker-compose up -d
```

---

## EF Core Migrations

Add migration:
```bash
dotnet ef migrations add "Init"   --project src/Ambev.DeveloperEvaluation.ORM   --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

List migrations:
```bash
dotnet ef migrations list   --project src/Ambev.DeveloperEvaluation.ORM   --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

Apply:
```bash
dotnet ef database update   --project src/Ambev.DeveloperEvaluation.ORM   --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

> `MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")` is already configured in the `DbContext`.

---

## Request Examples

### Authenticate and get token
```bash
curl -X POST "https://localhost:7181/api/Auth"   -H "Content-Type: application/json"   -d '{"email":"admin@dev.com","password":"Pass@123"}'
```

### Create sale
```bash
curl -X POST "https://localhost:7181/api/Sales"   -H "Authorization: Bearer <TOKEN>"   -H "Content-Type: application/json"   -d '{
    "saleNumber": "11111111-1111-1111-1111-111111111111",
    "createdAt": "2025-09-03T12:00:00Z",
    "customerId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "branchId":   "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "items": [
      { "productId": "cccccccc-cccc-cccc-cccc-cccccccccccc", "quantity": 3,  "unitPrice": 10.00 },
      { "productId": "dddddddd-dddd-dddd-dddd-dddddddddddd", "quantity": 10, "unitPrice": 5.00  }
    ]
  }'
```

**Note:** the API will apply **0%** for the 3-unit line and **20%** for the 10-unit line, computing **totalPrice** per item and **totalAmount** for the sale.

### Update sale (header + items)
```bash
curl -X PUT "https://localhost:7181/api/Sales/SALE_ID_GUID"   -H "Authorization: Bearer <TOKEN>"   -H "Content-Type: application/json"   -d '{
    "saleNumber": "11111111-1111-1111-1111-111111111111",
    "createdAt": "2025-09-03T13:00:00Z",
    "customerId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "branchId":   "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "items": [
      { "productId": "cccccccc-cccc-cccc-cccc-cccccccccccc", "quantity": 4, "unitPrice": 10.00 },
      { "productId": "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee", "quantity": 2, "unitPrice": 8.00  }
    ]
  }'
```

### Cancel sale (Manager/Admin)
```bash
curl -X POST "https://localhost:7181/api/Sales/SALE_ID_GUID/cancel"   -H "Authorization: Bearer <TOKEN>"
```

### Add item
```bash
curl -X POST "https://localhost:7181/api/Sales/SALE_ID_GUID/items"   -H "Authorization: Bearer <TOKEN)"   -H "Content-Type: application/json"   -d '{ "productId":"cccccccc-cccc-cccc-cccc-cccccccccccc", "quantity":5, "unitPrice":10.00 }'
```

### Update item
```bash
curl -X PUT "https://localhost:7181/api/Sales/SALE_ID_GUID/items/ITEM_ID_GUID"   -H "Authorization: Bearer <TOKEN)"   -H "Content-Type: application/json"   -d '{ "quantity":12, "unitPrice":9.50 }'
```

### Cancel item (Manager/Admin)
```bash
curl -X POST "https://localhost:7181/api/Sales/SALE_ID_GUID/items/ITEM_ID_GUID/cancel"   -H "Authorization: Bearer <TOKEN)"
```

---

## Testing
- Run:
```bash
dotnet test
```

---

## Troubleshooting

- **401/403 in Swagger**  
  Check the token and the **role** required by the route. In the JWT, the claim should be `role: "Manager"` (or use `ClaimTypes.Role` + `RoleClaimType` in `JwtBearer`).

- **EF migrations:**
    - *“Your target project ... doesn’t match your migrations assembly ...”*  
      Always use `--project` pointing to the **ORM** project and `--startup-project` to the **WebApi**, or configure `MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")` (already set).

- **Discount/Total not updating**  
  Ensure handlers are using **domain methods** (no DTO → entity mapping). Correct logic is through `AddOrIncrementItem`, `SetQuantity`, `SetUnitPrice`, `ReplaceItems`.

---

## License

MIT — feel free to use and adapt.
