# CruiseApp-Fundamentals
#### 
#### [![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](LICENSE)
#### 
#### **Description:**  
#### CruiseApp-Fundamentals is the initial scaffold of a web application for a cruise company, developed with ASP.NET Core MVC (.NET 8).

#### The project demonstrates the basic application architecture, domain models, controllers, and Razor Views. It serves as a foundation to be expanded during the ASP.NET Fundamentals and ASP.NET Advanced courses.

### **Domain Overview**
#### The system contains multiple ships. Each ship contains multiple decks, and each deck contains cabins. Each ship has exactly one route. A route belongs to one ship and is defined for the entire season
#### Routes consist of RouteDays, each associated with a Point (destination) for a specific date. The system contains multiple cruises.
#### Each cruise:
#### 	Belongs to one ship
#### 	Has a start date and end date within the season
#### 	Uses the ship’s route to determine its destinations
#### 	Cannot start or end at the point "At Sea"
#### 	Has a maximum duration of 14 days
#### A ship can have multiple cruises
#### Different cruises of the same ship may share some RouteDays
#### CRUD operations are implemented only for Cruises

### **Database Setup**
#### 1. Configure environment variables
#### Open the file .env.example in the CruiseApp.Web folder
#### Replace YourStrongPassword with your SQL Server password
#### Change the database user if it is different from sa
#### Save the file and rename it to .env
#### The .env file must not be committed to source control.

#### 2. Apply database migrations
#### Open Package Manager Console
#### Set Default Project to CruiseApp.Data
#### Run the following commands:

```python
Add-Migration Initial
Update-Database
```


### **Seed Items**
#### Right click on project CruiseApp.Seed -> Debug -> Start New Istance
#### This will load:
#### 15 destinations, 3 ships, many decks for each ship, hundreds cabins for each ship, Route and RouteDays for each ship (in season 01/06/2026 - 30/09/2026), 6 cruises.


### **Admin Functionality**
#### An admin user can perform CRUD operations on Cruises. This is a fundamental version of the application.
#### There are no CRUD operations for:

#### Ships
#### Decks
#### Cabins
#### Routes
#### RouteDays
#### Points

### **Project Status**
#### This project focuses on:
#### Domain modeling
#### Entity relationships
#### EF Core configuration
#### MVC architecture

#### It is intentionally limited in functionality and will be extended in future iterations.