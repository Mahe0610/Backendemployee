# Employee Login, Dashboard, and Certificate System

This repository now contains a starter full-stack implementation based on your requirements:

- **Frontend:** Angular login + dashboard UI with CSS styling.
- **Backend:** .NET 8 Web API.
- **Database:** MySQL via EF Core (Pomelo provider).
- **Features implemented:**
  - Role-based data capture at login usage flow.
  - Grid dashboard listing employee records.
  - Search in grid by employee name/email.
  - Certificate export to **Excel** and **A4 PDF**.
  - PDF includes company logo and barcode document code.

## Login details

Use these demo credentials:

- `admin / admin@123`
- `user / user@123`

## Role-based form behavior

- **Admin** can submit: employee name, age, DOB, email, salary.
- **Normal User** can submit: employee name, age, DOB, email.

## Backend setup

```bash
cd backend/EmployeeCertificateApi
dotnet restore
dotnet run
```

Update `appsettings.json` with your real MySQL credentials before running.

## Frontend setup

The Angular files are provided under `frontend/src/app` as a ready component and service structure.
Integrate these files in an Angular CLI project and run:

```bash
npm install
ng serve
```

Make sure API URL in `api.service.ts` points to the backend URL.

## Barcode validation concept

The document code generated as `CERT-<ID>-<DATE>` is encoded in barcode (CODE_128). On scan, your scanner app can call a validation endpoint (to be added) to fetch certificate details.
