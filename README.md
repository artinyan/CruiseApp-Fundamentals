# CruiseApp – ASP.NET Fundamentals

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](LICENSE)

## 📌 Description
**CruiseApp-Fundamentals** is the initial scaffold of a web application for a cruise company, developed with **ASP.NET Core MVC (.NET 8)**.

The project demonstrates basic application architecture, domain modeling, controllers, services, and Razor Views.  
It serves as a foundation to be extended during the **ASP.NET Fundamentals** and **ASP.NET Advanced** courses.

---

## 🌍 Domain Overview
- The system contains multiple **Ships**
- Each Ship contains multiple **Decks**, and each Deck contains **Cabins**
- Each Ship has exactly **one Route** defined for the entire season
- Routes consist of **RouteDays**, each associated with a **Point (destination)** for a specific date
- The system contains multiple **Cruises**

Each Cruise:
- Belongs to one Ship
- Has a start and end date within the season
- Uses the Ship’s Route to determine destinations
- Cannot start or end at the point **"At Sea"**
- Has a maximum duration of **14 nights**

Additional rules:
- A Ship can have multiple Cruises
- Different Cruises of the same Ship may share RouteDays
- CRUD operations are implemented **only for Cruises**

---

## 👥 Roles
- **Anonymous User**
- **Logged User**
- **Admin User**

---

## 👤 Anonymous User
- No registration or login required
- Can browse all Cruises and Cruise Details
- Can search Cruises by:
  - Ship
  - Start Point
  - Start Date

---

## 🔐 Logged User
- Registration and login required
- Same permissions as Anonymous User
- Can **Like / Unlike Cruises**
- Can view:
  - Number of likes per Cruise
  - “My Liked Cruises” list

---

## 🛠️ Admin User
- Pre-registered user
- Can perform **CRUD operations on Cruises**
- Cannot Like / Unlike Cruises
- Cannot see likes count

**Credentials:**
- Email: admin@cruise.com
- Password: Admin123!

- Credentials can be changed in: CruiseApp.Web.Infrastructure.IdentitySeeder.cs


---

## ✅ Validation Rules
Validation is implemented at:
- Client-side
- Server-side
- Domain level

Rules:
- Cruise dates must be within the Season
- Cruise cannot start or end at **"At Sea"**
- Last Day must be after First Day
- Cruise duration ≤ **14 nights**
- Cruises are **unique** per Ship, First Day, and Last Day

**Season duration:**  
📅 01-06-2026 → 30-09-2026

---

## 🗄️ Database Setup

### 1️⃣ Configure environment variables
1. Open `.env.example` in **CruiseApp.Web**
2. Replace `YourStrongPassword` with your SQL Server password
3. Change database user if needed (`sa` by default)
4. Rename the file to `.env`

⚠️ The `.env` file must **NOT** be committed to source control.

---

### 2️⃣ Apply database migrations
Open **Package Manager Console**  
Set **Default Project** to `CruiseApp.Data`

```powershell
Update-Database
```
#### ℹ️ Migrations are already included in the repository.
---
## 🌱 Seed Data

1. Right-click CruiseApp.Seed
2. Debug → Start New Instance
- This will seed:
- 15 destinations
- 3 ships
- Multiple decks per ship
- Hundreds of cabins
- Routes and RouteDays (season 01/06/2026 – 30/09/2026)
- 6 Cruises

## 🚧 Project Status
- This project focuses on:
- Domain modeling
- Entity relationships
- EF Core configuration
- ASP.NET Core MVC architecture
#### 🚀 The application is intentionally limited and will be expanded in future iterations.