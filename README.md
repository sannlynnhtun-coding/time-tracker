# TimeTracker

TimeTracker is a **Time Tracking System** built using **ASP.NET Core Web API** and **Entity Framework Core**. It allows users to manage clients, projects, tasks, and log time entries.

## 🚀 Features
- **Client Management**: Create and retrieve client records.
- **Project Tracking**: Associate projects with clients.
- **Task Management**: Manage tasks assigned to projects.
- **Time Logging**: Track user work hours on tasks.
- **User Management**: Maintain user details.
- **Swagger API Documentation** for easy testing.

## 🏗 Tech Stack
- **Backend:** ASP.NET Core Web API (.NET 8)
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Tools:** Swagger, Postman, Curl

## 📂 Project Structure
```
TimeTracker/
│── TimeTracker.Database/      # Database project (EF Core DbContext, Models)
│── TimeTracker.Api/           # Web API (Controllers, Program.cs, Startup)
│── TimeTracker.ConsoleApp/    # Console-based application (Optional)
│── README.md                  # Project documentation
│── TimeTracker.sln            # Solution file
```

## ⚡ Installation
1. **Clone the repository**:
   ```sh
   git clone https://github.com/your-username/TimeTracker.git
   cd TimeTracker
   ```
2. **Set up the database**:
   - Update `appsettings.json` with your **SQL Server** connection string.
   - Run migrations (if needed) using:
     ```sh
     dotnet ef database update --project TimeTracker.Database
     ```
3. **Run the API**:
   ```sh
   dotnet run --project TimeTracker.Api
   ```

## 🔗 API Endpoints
| Method | Endpoint               | Description                |
|--------|------------------------|----------------------------|
| GET    | `/api/clients`         | Get all clients            |
| POST   | `/api/clients`         | Create a new client        |
| GET    | `/api/projects`        | Get all projects           |
| POST   | `/api/projects`        | Create a new project       |
| GET    | `/api/tasks`           | Get all tasks              |
| POST   | `/api/tasks`           | Create a new task          |
| GET    | `/api/users`           | Get all users              |
| POST   | `/api/users`           | Create a new user          |
| GET    | `/api/timeentries`     | Get all time entries       |
| POST   | `/api/timeentries`     | Log a time entry           |

## 📖 Usage
### **1️⃣ Add a Client**
```sh
curl -X POST "https://localhost:5001/api/clients" -H "Content-Type: application/json" -d '{"name": "Acme Corp"}'
```
### **2️⃣ Create a Project for a Client**
```sh
curl -X POST "https://localhost:5001/api/projects" -H "Content-Type: application/json" -d '{"name": "Website Redesign", "clientId": 1}'
```
### **3️⃣ Assign a Task to a Project**
```sh
curl -X POST "https://localhost:5001/api/tasks" -H "Content-Type: application/json" -d '{"name": "Design Homepage", "projectId": 1}'
```
### **4️⃣ Log Time Entry for a User**
```sh
curl -X POST "https://localhost:5001/api/timeentries" -H "Content-Type: application/json" -d '{"userId": 1, "taskId": 1, "hoursWorked": 3}'
```

## 📌 Future Enhancements
- [ ] Implement Authentication & Authorization (JWT)
- [ ] Add User Role Management (Admin, Manager, Employee)
- [ ] Develop a Blazor UI for User-Friendly Experience
- [ ] Export Reports in CSV/PDF Format