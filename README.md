# eGift

An e-commerce solution built on **ASP.NET Core (.NET 10)**, made up of three applications: a REST API, an admin back-office portal, and a customer storefront.

| Project | Type | Purpose | Default URL |
|---|---|---|---|
| `eGift.WebAPI` | ASP.NET Core Minimal API | Business logic, data access (EF Core + SQL Server), file uploads | `http://localhost:5270` |
| `eGift.Admin` | ASP.NET Core MVC | Back-office portal for employees | `http://localhost:5073` |
| `eGift.Store` | ASP.NET Core MVC | Customer-facing storefront | `http://localhost:5108` |

Both MVC applications talk to the API through a typed `HttpClient` wrapper (`WebClientHelper`) and never access the database directly.

<!-- Add screenshots here, e.g. ![Storefront](docs/storefront.png) -->

---

## Features

### Admin portal (`eGift.Admin`)
- Employee login (session-based)
- CRUD for Employees, Customers, Countries, States, Cities, Addresses, Categories, Sub-Categories, Genders, Roles and Products
- Product management with a main image and up to four additional images
- Cascading dropdowns (Country → State → City, Category → Sub-Category)
- Order list and order details, with order status updates via AJAX
- SweetAlert2 delete confirmations and Toastr notifications

### Storefront (`eGift.Store`)
- Customer registration and login (including a popup login modal)
- Product catalog and product details page (image gallery, zoom, discount display, quantity selector)
- Shopping cart: add, update quantity, remove, clear
- Place orders from the cart, view "My Orders", cancel orders that are still in the `New` status
- Profile view and edit (personal details, address, profile image)

### API (`eGift.WebAPI`)
- 15 resource groups under `/api/*`
- Soft delete and audit fields (`IsDeleted`, `CreatedBy`, `CreatedDate`, `UpdatedBy`, `UpdatedDate`) on all entities
- Order status workflow: `New → Dispatched → Shipped → Delivered / Cancelled`
- Image upload (multipart) and image retrieval endpoints for products, customers and employees
- API-key protection on every request
- Password hashing with ASP.NET Core Identity's `PasswordHasher`
- Per-endpoint error handling and logging

---

## Tech Stack

- **Languages:** C#, JavaScript, HTML/Razor, CSS
- **Backend:** ASP.NET Core Minimal APIs (`net10.0`), custom middleware, DTO/entity mapping extension methods
- **Frontend:** ASP.NET Core MVC, Razor views and Tag Helpers, Bootstrap, Bootstrap Icons, jQuery, jQuery Validation (Unobtrusive), Toastr, SweetAlert2
- **Database:** SQL Server, Entity Framework Core 10 (code-first migrations), LINQ
- **Security:** API key middleware, session-based authentication, anti-forgery tokens, Data Annotations validation

---

## Architecture

```
┌──────────────┐        ┌──────────────┐
│  eGift.Admin │        │  eGift.Store │
│  (MVC)       │        │  (MVC)       │
└──────┬───────┘        └──────┬───────┘
       │  HttpClient + X-API-Key header
       └───────────┬───────────┘
            ┌──────▼───────┐
            │ eGift.WebAPI │
            │ (Minimal API)│
            └──────┬───────┘
                   │ EF Core
            ┌──────▼───────┐
            │  SQL Server  │
            └──────────────┘
```

### Folder structure

```
eGift.WebAPI/
  Common/        Enums (RefType, Size, OrderStatus)
  Data/          AppDBContext, Migrations
  Dtos/          Create and Edit DTOs
  Endpoints/     One static class per resource (MapGroup)
  Helpers/       PasswordHelper
  HttpFiles/     .http request files for each resource
  Mappings/      Entity <-> DTO extension methods
  Middlewares/   ApiKeyMiddleware, DefaultEmployeeMiddleware
  Models/        EF Core entities (BaseModel + per-entity models)

eGift.Admin/ and eGift.Store/
  Controllers/   MVC controllers
  CustomFilter/  SessionAuthorizeAttribute
  Helpers/       WebClientHelper, DateTimeHelper
  Models/        ViewModels, ResponseViewModels, ListViewModels
  Views/         Razor views
  wwwroot/       CSS and JavaScript
```

---

## API Overview

All routes require the `X-API-Key` header.

| Resource | Base route | Routes |
|---|---|---|
| Address, Category, City, Country, Gender, Role, State, SubCategory, OrderDetails | `/api/{resource}` | `GET /`, `GET /{id}`, `POST /`, `PUT /{id}`, `DELETE /{id}` |
| Customer, Employee, Product | `/api/{resource}` | CRUD + `GET /image/{fileName}` |
| Login | `/api/login` | CRUD + `GET /employee`, `GET /customer` |
| Order | `/api/order` | CRUD + `PUT /{id}/status` |
| MyCart | `/api/mycart` | CRUD + `GET /customer/{cid}` |

Ready-to-run sample requests are in `eGift.WebAPI/HttpFiles/*.http`.

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or SQL Server Express / LocalDB)
- Optional: EF Core CLI (`dotnet tool install --global dotnet-ef`)

### 1. Clone

```bash
git clone <your-repository-url>
cd <repository-folder>
```

### 2. Configure

**`eGift.WebAPI/appsettings.json`**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=eGiftDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "X-Api-Key": "<your-own-api-key>"
}
```

**`eGift.Admin/appsettings.json`** and **`eGift.Store/appsettings.json`**

```json
{
  "WebAPIUrl": "http://localhost:5270",
  "X-API-Key": "<the same API key as the API>"
}
```

> Use your own API key and keep real keys and connection strings out of source control (for example with .NET User Secrets or environment variables).

### 3. Create the database

```bash
cd eGift.WebAPI
dotnet ef database update
```

### 4. Run (API first)

```bash
# Terminal 1
dotnet run --project eGift.WebAPI

# Terminal 2
dotnet run --project eGift.Admin

# Terminal 3
dotnet run --project eGift.Store
```

### First-time data notes
- On the first API request, `DefaultEmployeeMiddleware` creates a default admin employee and login if none exists. Review the seed values in `eGift.WebAPI/Middlewares/DefaultEmployeeMiddleware.cs` and change them before any real deployment.
- The seeded employee references Gender, Role and Address with Id `1`, and customer registration assigns Role Id `3`. Create these master records (Gender, Role, Country/State/City/Address) from the Admin portal or the `.http` files so lists and joins return data.
- Uploaded images are stored under `eGift.WebAPI/uploads/`, which is created automatically.

---

## Known Limitations

- No automated tests
- No role-based authorization (the role id is stored in session but not enforced)
- No payment integration
- Database relationships are resolved with LINQ joins; foreign-key constraints are not defined in the model

---

## License

<!-- Add your license, e.g. MIT -->

## Author

<!-- Your name, GitHub profile and LinkedIn -->
