# JobBoard

Full-stack job board application built with ASP.NET Core MVC and SQL Server.

The project allows recruiters to create and manage job offers, while candidates can apply their applications through a role-based platform designed to simulate common recruiting workflows.

---

## Features

- User authentication and role-based authorization with ASP.NET Identity
- Recruiter and candidate profile management
- Job offer creation and application workflows
- PDF upload for CVs and recruiter verification documents
- Admin approval system for recruiter accounts
- Middleware-based profile completion enforcement
- Access control based on user roles and approval status

---

## Technical Highlights

- Role-based access control with ASP.NET Identity
- Custom middleware for profile completion and workflow restrictions
- File upload management with validation and cleanup
- Recruiter approval flow with document handling
- Structured MVC architecture with Entity Framework Core
- Separation between candidate, recruiter, and admin workflows

---

## Tech Stack

### Backend
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- ASP.NET Identity

### Frontend
- Razor Views
- Bootstrap 5

### Additional Features
- File upload handling
- Middleware-based request interception

---

## Running the Project

```bash
dotnet restore
dotnet ef database update
dotnet run
```

---

## Purpose of the Project

The goal of the project was to simulate a real-world recruiting platform with:
- authenticated users and role management
- recruiter approval workflows
- document upload and validation
- job application management
- controlled access to platform features

---

## Author

Developed by Ismaele Caporali