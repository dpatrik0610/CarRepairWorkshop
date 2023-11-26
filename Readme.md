# Car Repair Workshop

Car Repair Workshop is a web application designed for managing jobs and customers in a car repair shop.

## Prerequisites

Before you begin, ensure you have the following installed:

- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Entity Framework Core Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- [SQLite](https://www.sqlite.org/index.html) (for local development)

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/dpatrik0610/CarRepairWorkshop.git
   cd CarRepairWorkshop
   ```

2. Update the `appsettings.json` file with your database connection string.
Ex:
```json
  "ConnectionStrings": {
    "SQLite": "Data Source=Workshop.db"
  }
```
3. Open the solution in Visual Studio.

4. Run the following commands to apply migrations and update the database:

   ```bash
   dotnet ef migrations add Initial
   dotnet ef database update
   ```

5. Build and run the project:

   - Visual Studio: Press F5.
   - Visual Studio Code: Run `dotnet run` in the terminal.

6. Open your web browser and navigate to [https://localhost:5000](https://localhost:5000).

## Project Structure

- **CarRepairWorkshop.API:** ASP.NET Core Web API project.
- **CarRepairWorkshop.Shared:** Class library for shared models.
- **CarRepairWorkshop.UI:** Blazor WebAssembly project.

## Technologies Used

- ASP.NET Core
- Blazor WebAssembly
- Entity Framework Core
- SQLite (for local development)
