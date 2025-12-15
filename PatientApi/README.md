# PatientApi (ASP.NET Core 8)

Minimal ASP.NET Core 8 Web API implementing Patient and MedicalHistory domain models with EF Core.

Quick start (SQL Server)

1. Navigate to the project folder:

```powershell
cd "C:/Users/nada/OneDrive - Nile University/Desktop/back/ClinicWebsite_BackEnd/PatientApi"
```

2. Configure the SQL Server connection string in `appsettings.json` (example):

```json
"ConnectionStrings": {
	"DefaultConnection": "Server=YOUR_SERVER;Database=patient_domain;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

3. Install EF tooling (if not already installed), create migrations and apply them to your SQL Server:

```powershell
dotnet tool install --global dotnet-ef --version 8.0.0
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run the API (Development mode recommended while testing):

```powershell
set ASPNETCORE_ENVIRONMENT=Development
dotnet run --urls "http://127.0.0.1:5000;https://127.0.0.1:5001"
```

API endpoints (examples):
- `GET /api/patients` — list with search & filters
- `GET /api/patients/{id}` — get patient with medical histories
- `POST /api/patients` — create patient
- `PUT /api/patients/{id}` — update patient
- `DELETE /api/patients/{id}` — delete patient
- `GET /api/medicalhistories` — list histories
- `POST /api/medicalhistories` — create history (requires `PatientId`)

Notes:
- `Patient.UserId` is an optional string mapping to an external user system.
- Attachments API is implemented; uploaded files are stored under `wwwroot/uploads/patients/{id}`.
- This project uses SQL Server by default in `appsettings.json`. If you previously had a local SQLite file (e.g. `clinic.db`), it is not required.
