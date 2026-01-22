# DevExpress Reporting Integration Setup Guide

This guide explains how to set up and use the DevExpress XtraReports integration for the report builder feature.

## Prerequisites

1. **DevExpress License**: Ensure you have a valid DevExpress license
2. **NuGet Packages**: The following package is already included:
   - `DevExpress.AspNetCore.Reporting` (Version 25.2.3)

## Backend Setup

### 1. Database Migration

After adding the `ReportEntity`, create and apply a migration:

```bash
dotnet ef migrations add AddReportsTable --project Project.EntityFramework --startup-project Project.Api
dotnet ef database update --project Project.EntityFramework --startup-project Project.Api
```

### 2. Permissions Setup

Add the following permissions to your role/permission system:
- `ReportDesigner.Create` - Allows users to create new reports
- `ReportDesigner.Edit` - Allows users to edit existing reports
- `ReportDesigner.View` - Allows users to view reports (read-only)

### 3. Configuration

The DevExpress Reporting is already configured in `Program.cs`:
- Report storage extension is registered
- Middleware is configured
- Endpoints are mapped

## Frontend Setup

### 1. Install DevExpress Reporting for Angular

You need to include DevExpress Reporting scripts in your Angular application. Add the following to your `index.html`:

```html
<!-- DevExpress Reporting Scripts -->
<script src="https://cdn.jsdelivr.net/npm/devextreme@latest/dist/js/dx.all.js"></script>
<script src="https://cdn.jsdelivr.net/npm/devexpress-reporting@latest/dist/js/dx-reporting.js"></script>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/devextreme@latest/dist/css/dx.common.css" />
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/devexpress-reporting@latest/dist/css/dx-reporting.css" />
```

**OR** install via npm:

```bash
npm install devexpress-reporting devextreme
```

Then import in your `angular.json`:

```json
{
  "scripts": [
    "node_modules/devextreme/dist/js/dx.all.js",
    "node_modules/devexpress-reporting/dist/js/dx-reporting.js"
  ],
  "styles": [
    "node_modules/devextreme/dist/css/dx.common.css",
    "node_modules/devexpress-reporting/dist/css/dx-reporting.css"
  ]
}
```

### 2. Backend URL Configuration

Ensure your Angular app knows the backend URL. Update the `getBackendUrl()` method in `devexpress-designer.component.ts` to use your environment configuration.

## Usage

### Creating a Report

1. Navigate to the Report Designer page
2. Click "Create Report" button
3. The DevExpress Web Report Designer will open in a new view
4. Design your report using drag-and-drop interface
5. Save the report (it will be stored in the database)

### Report Storage

Reports are stored in the `Reports` table with:
- **Url**: Unique identifier for the report
- **Name**: Display name
- **LayoutData**: Serialized report layout (XML)
- **Status**: Draft, Published, or Archived
- **IsPublic**: Whether the report is publicly accessible

### Base Template

All reports start from `BaseReportTemplate` which includes:
- Report header with title and date
- Detail band for data
- Footer with page numbers
- Consistent styling

## API Endpoints

- `GET /api/reportdesigner/can-design` - Check if user can design reports
- `GET /api/reportdesigner/reports` - Get list of reports
- DevExpress endpoints (handled automatically):
  - `/DXXRD` - Report storage
  - `/DXXRDV` - Data source
  - `/DXXRDB` - Bindings

## Authorization

The report designer checks for:
- User authentication (JWT token)
- Permission claims: `ReportDesigner.Create` or `ReportDesigner.Edit`
- Administrator role (full access)

## Troubleshooting

### Designer Not Loading

1. Check browser console for errors
2. Verify DevExpress scripts are loaded
3. Check backend URL configuration
4. Verify JWT token is being sent in headers

### Reports Not Saving

1. Check database connection
2. Verify user has `ReportDesigner.Create` or `ReportDesigner.Edit` permission
3. Check backend logs for errors

### CORS Issues

Ensure your backend CORS policy allows requests from your Angular app origin.

## Next Steps

1. Create data sources for your reports
2. Configure report parameters
3. Set up report export functionality (PDF, Excel, etc.)
4. Implement report viewer component for non-designers
