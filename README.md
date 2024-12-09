# Pokedex
This repository contains a **fun version of the Pokédex**, where under certain conditions the description of the Pokemon is translated using **Yoda's or Shakespeare's language**.

## Solution Structure
The solution has been implemented using a framework of mine: it is a fusion of DDD, CQRS, and Clean Architecture—It leverages their individual strengths to **create scalable, maintainable, and valuable systems.** I call it Jewel Architecture, because it organizes your system into multifaceted, interchangeable components, creating a highly valuable and mantainable structure—like a finely cut gem. It uses an aggregate-based folder structure and follows a clean architecture, with the following key layers:

**- Application Layer:** Contains use cases, services and business logic.

**- Domain Layer:** Defines entities, aggregates, value objects, invariants and domain logic.

**- Infrastructure Layer:** Optional infrastructure components.

**- Interface Layer:** ASP.NET Core Web API exposing application endpoints.

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed on your machine
  
It is an **ASP.NET Core Web API** project built with .NET 8. The API is **configured with Swagger** for API documentation.

## Getting Started

### 1. Clone the repository

Clone this repository to your local machine:

```bash
git clone https://github.com/emanuelebuldrini/pokedex.git
cd pokedex
cd Pokedex.WebApi
```
### 2. Build the Solution
Use the .NET CLI to restore and build the solution:
```bash
dotnet build
```
### 3. Run the Application
Run the application using the .NET CLI.
```bash
dotnet run --project "./Pokedex.Interface/Pokedex.Interface.csproj"
```
### 4. Access the Application
Once running, you can access the Swagger API documentation at:

HTTP: http://localhost:5046/swagger

### 5. Test the Application
To run tests for the application, use:
```bash
dotnet test
```
