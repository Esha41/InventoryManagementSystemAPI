# Ettad Inventory Services

## Overview
This module provides service layer implementation for inventory management, starting with Ammunition services.

## Features Implemented

### AmmunitionService
A complete CRUD service implementation for managing ammunition inventory with the following features:

#### Operations
- **GetByIdAsync**: Retrieve a single ammunition record by ID with all navigation properties
- **GetAllAsync**: Retrieve all ammunition records with navigation properties
- **CreateAsync**: Create a new ammunition record with validation
- **UpdateAsync**: Update an existing ammunition record with validation
- **DeleteAsync**: Soft delete an ammunition record

#### Key Features
1. **AutoMapper Integration**: Automatic mapping between DTOs and entities
2. **FluentValidation**: Comprehensive validation rules for create/update operations
3. **Soft Delete**: Implements soft delete pattern using IsDeleted flag
4. **Navigation Properties**: Automatically loads related entities (Hcc, Supplier, Country, Manufacturer, Units, etc.)
5. **Error Handling**: Comprehensive try-catch blocks with meaningful error messages
6. **Response Wrapping**: All responses wrapped in `APIOperationResponse` for consistency

## Project Structure

```
Ettad.Inventory.Services/
├── Ammunitions/
│   ├── Dtos/
│   │   ├── AmmunitionDto.cs                      # Full ammunition data transfer object
│   │   └── CreateUpdateAmmunitionDto.cs          # DTO for create/update operations
│   ├── Profiles/
│   │   └── AmmunitionMappingProfile.cs           # AutoMapper configuration
│   ├── Validators/
│   │   └── CreateUpdateAmmunitionDtoValidator.cs # FluentValidation rules
│   ├── AmmunitionService.cs                      # Service implementation
│   └── IAmmunitionService.cs                     # Service interface
├── ModuleServicesDependences.cs                  # Dependency injection setup
└── Ettad.Inventory.Services.csproj               # Project file
```

## Dependencies

### NuGet Packages
- **AutoMapper** (12.0.1): Object-to-object mapping
- **AutoMapper.Extensions.Microsoft.DependencyInjection** (12.0.1): DI integration
- **FluentValidation** (12.0.0): Input validation
- **FluentValidation.DependencyInjectionExtensions** (12.0.0): DI integration

### Project References
- **Ettad.Data**: Entity models and repository interfaces
- **Ettad.Repository**: Repository implementations
- **Ettad.CrossCutting.ResponseHandler**: Response models

## Validation Rules

The `CreateUpdateAmmunitionDtoValidator` enforces the following rules:

- **Name**: Required, max 200 characters
- **ItemNo**: Required, max 100 characters
- **Lot**: Must be greater than 0
- **BatchNo**: Required, max 100 characters
- **HccId**: Required, must be greater than 0
- **PartNo**: Max 100 characters
- **ExpiryDate**: Must be in the future if provided
- **BulletDiameter**: Must be greater than 0
- **BulletDiameterUnitId**: Required, must be greater than 0
- **CaseLength**: Must be greater than 0
- **CaseLengthUnitId**: Required, must be greater than 0
- **Primer**: Required, max 100 characters
- **TotalWeight**: Must be greater than 0
- **Nsn**: Required, max 255 characters (must be unique)
- **CaseTypeId**: Required, must be greater than 0
- **PropellantId**: Required, must be greater than 0
- **CompatibilityId**: Required, must be greater than 0
- **HazardDivisionId**: Required, must be greater than 0
- Optional fields (SupplierId, CountryId, ManufacturerId, etc.): Validated only when provided

## Usage

### Registration in Startup/Program.cs
```csharp
using Ettad.Inventory.Services;

// In ConfigureServices or builder.Services
services.AddInventoryServices();
```

### Injecting the Service
```csharp
public class AmmunitionController : ControllerBase
{
    private readonly IAmmunitionService _ammunitionService;

    public AmmunitionController(IAmmunitionService ammunitionService)
    {
        _ammunitionService = ammunitionService;
    }

    // Use the service methods...
}
```

### Example Service Calls
```csharp
// Get all ammunition
var result = await _ammunitionService.GetAllAsync();

// Get by ID
var ammunition = await _ammunitionService.GetByIdAsync(1);

// Create new ammunition
var createDto = new CreateUpdateAmmunitionDto 
{ 
    Name = "7.62mm NATO",
    ItemNo = "AMM-001",
    // ... other properties
};
var created = await _ammunitionService.CreateAsync(createDto);

// Update ammunition
var updateDto = new CreateUpdateAmmunitionDto { /* ... */ };
var updated = await _ammunitionService.UpdateAsync(1, updateDto);

// Delete ammunition (soft delete)
var deleted = await _ammunitionService.DeleteAsync(1);
```

## Changes Made to Other Projects

### Project.Data/Entities/BaseItem.cs
Added `IsDeleted` property to support soft delete pattern:
```csharp
public bool IsDeleted { get; set; } = false;
```

This property is now available to all items that inherit from `BaseItem` (Ammunition, Explosive, Weapon, Accessory).

## Notes

- All operations return `APIOperationResponse<T>` with proper success/failure status
- Soft delete is implemented - deleted records are marked as `IsDeleted = true` instead of being physically removed
- Navigation properties are eagerly loaded for better performance and data completeness
- Validation occurs before any database operations to ensure data integrity
- Entity timestamps (CreationDate, ModificationDate) are automatically managed

## Future Enhancements

Potential improvements for future iterations:
- Add filtering and pagination support for GetAllAsync
- Implement search functionality
- Add audit logging
- Support for bulk operations
- Add caching layer for frequently accessed data

