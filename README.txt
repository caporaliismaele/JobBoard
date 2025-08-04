# 🧑‍💼 JobBoard - ASP.NET Core MVC Job Portal

A full-stack web application developed with **ASP.NET Core MVC** that allows **Recruiters** to post and manage job offers, and **Candidates** to search, apply, and manage their applications. The project follows a clean architecture, role-based access, and simulates a real-world job board.

## 🚀 Features

### 🔒 Authentication & Authorization
- User registration and login
- Role-based access for `RECRUITER`, `CANDIDATE`, and `ADMIN`
- Middleware that restricts access based on profile completion and recruiter approval

---

### 👤 Candidate Features
- Complete profile with personal details and upload a **PDF CV**
- Consent to privacy policy (mandatory)
- Browse and search job offers by **position** and **location**
- View job details in a side panel layout
- Apply to job offers (only once per offer)
- View personal application history (`My Applications`)
- Update personal profile and replace CV (old CV automatically deleted)

---

### 🧑‍💼 Recruiter Features
- Complete profile with business and identity documents (PDF)
- Consent to privacy policy (mandatory)
- Recruiter profile must be **approved by admin** before accessing platform features
- Manage only their own job offers
- Create, edit, and delete job offers
- Responsive table view with action buttons
- View applicants for each job offer, including access to candidate details and CV

---

### 🛡️ Admin Features
- Role assigned via **code seeding**
- Dashboard for **approving or rejecting recruiter applications**
- Ability to view submitted documents
- On **rejection**, recruiter account and uploaded files are permanently deleted

---

## 🛠️ Technologies Used

- ASP.NET Core MVC
- Entity Framework Core + Code First Migrations
- Identity for authentication & roles
- Bootstrap 5 (for responsive UI)
- SQL Server LocalDB
- LINQ, Data Annotations
- File Upload with validation and cleanup

---

## 🧪 How to Run Locally

1. Clone the repo:
   git clone https://github.com/caporaliismaele/JobBoard.git
   cd jobboard

2. Run migrations and start the project:
   Update-Database -Context JobBoardContext
   dotnet run

---

## 🙋‍♂️ Author
Developed by Ismaele Caporali  
Feel free to reach out on LinkedIn or by email (ismaelecaporali@gmail.com)!
