# PatientApi (ASP.NET Core 8)

Minimal ASP.NET Core 8 Web API implementing Patient and MedicalHistory domain models with EF Core (SQLite recommended for local development).

Quick start

1. Navigate to the project folder:

```powershell
cd "c:/Users/nada/OneDrive - Nile University/Desktop/back/ClinicWebsite_BackEnd/PatientApi"
```

2. Restore and run:

```powershell
dotnet restore
dotnet run
```

3. Generate EF migrations and apply (requires `dotnet-ef` tool):

```powershell
dotnet tool install --global dotnet-ef --version 8.0.0
dotnet ef migrations add InitialCreate -p .
dotnet ef database update -p .
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
- Attachments API is not included (optional next step).
