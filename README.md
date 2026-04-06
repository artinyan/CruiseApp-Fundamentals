# CruiseApp – ASP.NET Advanced

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](LICENSE)

## 📌 Description
**CruiseApp-Advanced** is an upgrade of the CruiseApp-Fundamentals web application for a cruise line, developed with **ASP.NET Core MVC (.NET 8)**.

The project demonstrates application architecture, domain modeling, controllers, services, and Razor Views.  

---

## 🌍 Domain Overview
- The system contains multiple **Ships**, each contains multiple **Decks** and **Cabins**
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
- Can select a cruise, choose a cabin type, deck, and a specific cabin (if it is vacant), and create a booking for number of passengers - not exceeding the cabin capacity. Pseudo payment is applied. Status: **Pending**.
- Can view his bookings in “My Bookings” list
- Can finish **Check-In** by filling passenger details for any passenger. Status: **Confirmed**.
- Can **Like / Unlike Cruises**
- Can view:
  - Number of likes per Cruise
  - “My Liked Cruises” list

---

## 🛠️ Admin User
- Pre-registered user
- Can perform **CRUD operations on Cruises**
- Can search bookings by reference number, view all details
- Can cancel any booking. Status: **Canceled**.
- Can add a new Ship using a pre-prepared ZIP package. The ZIP package must contain two JSON files that follow a specific structure, as well as PNG and JPG files. The ZIP file(s) must be placed at: CruiseApp.Web.Uploads.packages. For convenience, three new ZIP files for new **Ships** (Aphrodite, Nymph, Poseidon) have been placed in this folder. These ZIP files can serve as a template for preparing other ship packages.
- Cannot make bookings
- Cannot Like / Unlike Cruises or see Likes count

**Credentials:**
- Email: admin@cruise.com
- Password: Admin123!

- Credentials can be changed in: CruiseApp.Web.Infrastructure.IdentitySeeder.cs

---

## ✅ Validation Rules:
- Cruise dates must be within the Season
- Cruise cannot start or end at **"At Sea"**
- Last Day must be after First Day
- Cruise duration ≤ **14 nights**
- Cruises are **unique** per Ship, First Day, and Last Day

**Season duration:**  
📅 01.06.2026 - 30.09.2026

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
## 🌱 AutoSeed Data

#### The following related data will be seeded at first start:
- 15 destinations
- 3 Ships with multiple decks and cabins
- Routes and RouteDays (season **01.06.2026 – 30.09.2026**)
- 6 Cruises


## 🚧 Project Status
This project focuses on:
- Domain modeling
- Entity relationships
- EF Core configuration
- ASP.NET Core MVC architecture

## 🧱 Tech Stack
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server
- NUnit (Unit Testing)