# 🏦 HLChitFund — Chit Fund Management System

A full-stack **Chit Fund Management System** built to demonstrate MNC-grade, production-ready development practices across architecture, security, testing, and UX.

> A Chit Fund is a popular South Asian rotating savings scheme where a group of members contribute a fixed amount monthly, and one member "wins" the pooled amount each month through lottery or auction — until every member has won once.

---

## 🚀 Tech Stack

| Layer | Technology |
|---|---|
| **Backend** | .NET 8 (LTS), ASP.NET Core Web API, Entity Framework Core |
| **Frontend** | Angular 21 (Zoneless), Angular Material (Azure/Blue theme) |
| **Database** | Microsoft SQL Server |
| **Auth** | JWT Bearer Authentication with Role-Based Access Control |
| **Testing** | Jasmine + Karma (Frontend Unit Tests) |
| **Architecture** | Clean Architecture (Backend) · Standalone Components (Frontend) |
| **Forms** | Angular Reactive Forms with field-level validation |
| **Version Control** | Git Flow branching strategy with Conventional Commits |

---

## ✨ Features

- 🔐 **JWT Authentication** with role-based authorization (`Admin`, `Cashier`, `Customer`)
- 👥 **Customer Management** — Full CRUD with form validation
- 💰 **Chit Group Management** — Lottery / Auction / Fixed type groups with commission handling
- 📝 **Enrollment Management** — Slot allocation with max-member and duplicate-entry checks
- 💳 **Payment Collection** — Auto-generated receipt numbers (`RCP-yyyyMMdd-XXXXXX`)
- 🏆 **Winner Selection** — Automated commission deduction and net payout calculation
- 📊 **Reports Dashboard** — Monthly collections, chit group summary, commission summary, winner summary (with date-range and group filters)
- 🛡️ **Role-Based UI** — Sidebar and action buttons dynamically rendered per role
- 🔔 **Toast Notifications** & **Confirmation Dialogs** — Consistent UX across all CRUD operations
- ⏳ **Global Loading Indicator** — Signal-based HTTP loading interceptor
- ⚠️ **Global Error Handling** — Centralized interceptor for 401 / 403 / 404 / 500 / network errors
- 📱 **Responsive Design** — Mobile-friendly layout with collapsible sidebar
- ✅ **Unit Tested** — Services, Guards, and Components covered with Jasmine/Karma

---

## 🏗️ Architecture

### Backend — Clean Architecture

```
HLChitFund/
├── HLChitFund.Domain/         → Entities, Enums, Base classes
├── HLChitFund.Application/    → DTOs, Interfaces, Business contracts
├── HLChitFund.Infrastructure/ → Services, Repositories, EF Core DbContext
└── HLChitFund.API/            → Controllers, Program.cs, Middleware
```

**Key design decisions:**
- Generic Repository pattern with Unit of Work
- Eager loading via overloaded `GetAllAsync(params Expression<Func<T, object>>[] includes)` — avoids leaking `DbContext` into services
- Soft delete using `DeleteBehavior.Restrict` + `IsDeleted` flag
- Role-based endpoint authorization via `[Authorize(Roles = "...")]`

### Frontend — Standalone Component Architecture

```
frontend/src/app/
├── core/
│   ├── guards/          → authGuard (route protection)
│   ├── interceptors/    → auth, error, loading (single-responsibility)
│   └── services/        → AuthService, ToastService, LoadingService
├── shared/
│   ├── confirm-dialog/  → Reusable confirmation modal
│   ├── toast/           → Reusable toast notification
│   └── loading-spinner/ → Global top-bar loading indicator
├── layout/
│   ├── shell/           → Main layout wrapper
│   ├── navbar/          → Top navigation bar
│   └── sidebar/         → Role-based collapsible sidebar
└── features/
    ├── auth/login
    ├── dashboard
    ├── customer
    ├── chit-group
    ├── enrollment
    ├── payment
    ├── winner
    └── reports
```

**Key design decisions:**
- **Zoneless change detection** (`provideZonelessChangeDetection`) for improved performance
- **Reactive Forms** with `Validators` over template-driven forms for scalability and testability
- Single-responsibility HTTP interceptors (`authInterceptor`, `errorInterceptor`, `loadingInterceptor`)
- Signal-based services (`ToastService`, `LoadingService`) for reactive state

---

## 🔑 Role-Based Access Control

| Module | Admin | Cashier | Customer |
|---|:---:|:---:|:---:|
| Dashboard | ✅ | ✅ | ✅ |
| Customers | ✅ | ✅ | ❌ |
| Chit Groups (View) | ✅ | ✅ | ✅ |
| Chit Groups (Create/Edit/Delete) | ✅ | ❌ | ❌ |
| Payments | ✅ | ✅ | ❌ |
| Enrollments | ✅ | ❌ | ❌ |
| Winner Selection | ✅ | ❌ | ❌ |
| Reports | ✅ | ❌ | ❌ |

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18+) and npm
- [Angular CLI](https://angular.dev/tools/cli) `v21`
- SQL Server (LocalDB or full instance)

### Backend Setup

```bash
cd backend

# Update connection string in appsettings.Development.json
# "DefaultConnection": "Server=YOUR_SERVER;Database=HLChitFundDB;Trusted_Connection=True;TrustServerCertificate=True;"

dotnet restore
dotnet ef database update
dotnet run
```

API will be available at `https://localhost:7106`
Swagger UI: `https://localhost:7106/swagger`

### Frontend Setup

```bash
cd frontend

npm install

# Update src/environments/environment.ts if your API runs on a different port
# apiUrl: 'https://localhost:7106/api'

ng serve
```

App will be available at `http://localhost:4200`

### Running Tests

```bash
cd frontend
ng test
```

---

## 📡 Key API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Auth/login` | Authenticate user, returns JWT |
| `GET` | `/api/ChitGroup` | List all chit groups |
| `POST` | `/api/Enrollment` | Enroll a customer into a chit group |
| `POST` | `/api/Payment` | Record a payment (auto-generates receipt) |
| `POST` | `/api/Winner/select` | Select a winner (auto-calculates commission) |
| `GET` | `/api/Report/monthly-collection` | Monthly collection report (filterable) |
| `GET` | `/api/Report/chit-group-summary` | Per-group performance summary |

Full API reference available via Swagger once the backend is running.

## 🌳 Git Workflow

This project follows **Git Flow** with **Conventional Commits**:

```
main        → production-ready, tagged releases (v1.0.0, v2.0.0, ...)
develop     → integration branch
feature/*   → individual feature branches
```

Commit format: `type(scope): description`
Examples: `feat(auth): add JWT login`, `fix(report): fix change detection for filter`, `chore(release): v9.0.0`

---

## 🧪 Sprint History

| Sprint | Focus |
|---|---|
| 1–4 | Backend — Clean Architecture, Auth, ChitGroup, Enrollment, Payment, Winner |
| 5 | Frontend — Layout shell, Dashboard, full CRUD pages, Toast/Confirm dialogs |
| 6 | Reports — Backend filters + Frontend reports dashboard |
| 7 | Role-Based Access Control — Backend authorization + Frontend RBAC UI |
| 8 | UX Enhancements — Error handling, loading states, Reactive Forms validation, responsive design |
| 9 | Unit Testing — Jasmine/Karma test suite for services, guards, and components |

---
