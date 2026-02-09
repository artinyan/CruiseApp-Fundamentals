# CruiseApp-Fundamentals

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](LICENSE)

**Description:**  
CruiseApp-Fundamentals is the initial scaffold of a web application for a cruise company, developed with ASP.NET Core MVC (.NET 8). This project demonstrates the basic architecture, models, controllers, and Razor Views, providing a foundation to expand during the ASP.NET Fundamentals and Advanced courses.

System contains some ships. Any ship contains some decks with cabins. Ani ship has one route. Any route is for one ship. Routes are for whole season. Any route contains RouteDays with point for any day of the season. There are some cruises. Any cruise is for one ship, and contains first and last day into the season. Ship route determins points (destinations) of the cruise. First and last day of the cruise can't be point "At Sea". Any ship can contains many cruises. Two different crushes of one ship can have some same RouteDays.
Cruises length are restricted up to 14 days. CRUD is applied only to cruises.

Instructions:

Database Setup
1. Configure environment variables

Open the file .env.example in the CruiseApp.Web folder

Replace YourStrongPassword with your own SQL Server password

Change the user if it is different from sa

Save the file and rename it from .env.example to .env




2. Apply database migrations

Open Package Manager Console

Set Default Project to CruiseApp.Data

Run:

Add-Migration Initial
Update-Database



Admin functionality

An admin user can perform CRUD operations on Cruises.

This is a fundamental version of the application.
There are no CRUD operations for:

Ships

Decks

Cabins

Routes

RouteDays

Points