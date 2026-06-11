# Quiz App API

A RESTful API for managing quizzes and their nested questions. Built with ASP.NET Core, Entity Framework (EF) Core, and MS SQL Server.

## Tech Stack

* **Framework:** .NET / ASP.NET Core Web API
* **Language:** C#
* **ORM:** Entity Framework (EF) Core
* **Database:** MS SQL Server
* **Documentation:** Swagger (OpenAPI)

---

## Features

* **Relational Data Management:** Seamlessly handles the one-to-many relationship between Quizzes and Questions.
* **Full CRUD Operations:** Create, Read, Update, and Delete endpoints for all resources.
* **Nested Routing:** Clean, REST-compliant URL structures (e.g., accessing questions through their parent quiz).
* **Interactive API Docs:** Built-in Swagger UI for easy testing and exploration.

---

## API Endpoints

### Quizzes
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **GET** | `/quizzes` | Retrieves a list of all quizzes. |
| **POST** | `/quizzes` | Creates a new quiz. |
| **GET** | `/quizzes/{id}` | Retrieves a specific quiz by its ID. |
| **PUT** | `/quizzes/{id}` | Updates an existing quiz. |
| **DELETE**| `/quizzes/{id}` | Deletes a quiz and its associated questions. |

### Questions
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **GET** | `/quizzes/{quizId}/questions` | Retrieves all questions for a specific quiz. |
| **POST** | `/quizzes/{quizId}/questions` | Adds a new question to a specific quiz. |
| **GET** | `/quizzes/{quizId}/questions/{id}` | Retrieves a specific question by its ID. |
| **PUT** | `/quizzes/{quizId}/questions/{id}` | Updates a specific question. |
| **DELETE**| `/quizzes/{quizId}/questions/{id}` | Deletes a specific question. |

---

## Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (or your current version)
* SQL Server (e.g., SQL Server Express or LocalDB)
* EF Core CLI Tools (`dotnet tool install --global dotnet-ef`)

