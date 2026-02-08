using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefaultRole = table.Column<bool>(type: "bit", nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlacklistedTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    BlacklistedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compatibilities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compatibilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Depots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileUplodMaster",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUplodMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HazardDivisions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HazardDivisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NatureOptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NatureOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryPurposes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectailMaterials",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectailMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Propellants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propellants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestPurposes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleApplicationEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationEntityId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleApplicationEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkflowType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSpecialOrReserved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MilitaryId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepoId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecievedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Depots_DepoId",
                        column: x => x.DepoId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileUplodDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileUplodMasterId = table.Column<long>(type: "bigint", nullable: false),
                    Entity = table.Column<int>(type: "int", nullable: false),
                    entityId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUplodDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUplodDetails_FileUplodMaster_FileUplodMasterId",
                        column: x => x.FileUplodMasterId,
                        principalTable: "FileUplodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ItemNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    Nsn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumQuantity = table.Column<long>(type: "bigint", nullable: true),
                    Distribution = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReferenceNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UNNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    ClassificationId = table.Column<long>(type: "bigint", nullable: true),
                    TypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseItems_Classifications_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "Classifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseItems_ItemTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLdapUser = table.Column<bool>(type: "bit", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraEmployeesView = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    FullNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankId = table.Column<long>(type: "bigint", nullable: true),
                    MilitoryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LdapUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Ranks_RankId",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    ApplicationRoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationEntityId = table.Column<long>(type: "bigint", nullable: false),
                    MustApprove = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RequireHigherApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HigherApprovalRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HigherApplicationEntityId = table.Column<long>(type: "bigint", nullable: true),
                    ReserveQty = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CanSkip = table.Column<bool>(type: "bit", nullable: false),
                    CanReturn = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_ApplicationEntity_HigherApplicationEntityId",
                        column: x => x.HigherApplicationEntityId,
                        principalTable: "ApplicationEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_AspNetRoles_HigherApprovalRoleId",
                        column: x => x.HigherApprovalRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllowanceItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowanceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllowanceItems_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AllowanceItems_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ammunitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    AmmunitionType = table.Column<int>(type: "int", nullable: false),
                    BulletDiameter = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BulletDiameterUnitId = table.Column<long>(type: "bigint", nullable: true),
                    ArmNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsLinked = table.Column<bool>(type: "bit", nullable: false),
                    Primer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NatureOptionId = table.Column<long>(type: "bigint", nullable: true),
                    PrimaryPurposId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectileColorId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectailMaterialId = table.Column<long>(type: "bigint", nullable: true),
                    CaseTypeId = table.Column<long>(type: "bigint", nullable: true),
                    PropellantId = table.Column<long>(type: "bigint", nullable: true),
                    CompatibilityId = table.Column<long>(type: "bigint", nullable: true),
                    HazardDivisionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ammunitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ammunitions_BaseItems_Id",
                        column: x => x.Id,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ammunitions_CaseTypes_CaseTypeId",
                        column: x => x.CaseTypeId,
                        principalTable: "CaseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ammunitions_Colors_ProjectileColorId",
                        column: x => x.ProjectileColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ammunitions_Compatibilities_CompatibilityId",
                        column: x => x.CompatibilityId,
                        principalTable: "Compatibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ammunitions_HazardDivisions_HazardDivisionId",
                        column: x => x.HazardDivisionId,
                        principalTable: "HazardDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ammunitions_NatureOptions_NatureOptionId",
                        column: x => x.NatureOptionId,
                        principalTable: "NatureOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ammunitions_PrimaryPurposes_PrimaryPurposId",
                        column: x => x.PrimaryPurposId,
                        principalTable: "PrimaryPurposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ammunitions_ProjectailMaterials_ProjectailMaterialId",
                        column: x => x.ProjectailMaterialId,
                        principalTable: "ProjectailMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ammunitions_Propellants_PropellantId",
                        column: x => x.PropellantId,
                        principalTable: "Propellants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ammunitions_Units_BulletDiameterUnitId",
                        column: x => x.BulletDiameterUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Explosives",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    HazardDivisionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Explosives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Explosives_BaseItems_Id",
                        column: x => x.Id,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Explosives_HazardDivisions_HazardDivisionId",
                        column: x => x.HazardDivisionId,
                        principalTable: "HazardDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Explosives_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Lot = table.Column<int>(type: "int", nullable: false),
                    InventoryId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    ItemQuantity = table.Column<long>(type: "bigint", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReadyForIssue = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    YearOfManufacture = table.Column<int>(type: "int", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLotEmpty = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Caliber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaliberUnitId = table.Column<long>(type: "bigint", nullable: true),
                    YearOfManufacture = table.Column<int>(type: "int", nullable: true),
                    CountryOfManufactureId = table.Column<long>(type: "bigint", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_BaseItems_Id",
                        column: x => x.Id,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Weapons_Countries_CountryOfManufactureId",
                        column: x => x.CountryOfManufactureId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Weapons_Units_CaliberUnitId",
                        column: x => x.CaliberUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    RequesterId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RequestPurposeId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseRequests_AspNetUsers_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseRequests_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseRequests_RequestPurposes_RequestPurposeId",
                        column: x => x.RequestPurposeId,
                        principalTable: "RequestPurposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AttemptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoginType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginAttempts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntityId = table.Column<long>(type: "bigint", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDelegations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DelegatorUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DelegateeUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DelegationStatus = table.Column<int>(type: "int", nullable: false),
                    DelegationScopes = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDelegations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDelegations_AspNetUsers_DelegateeUserId",
                        column: x => x.DelegateeUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDelegations_AspNetUsers_DelegatorUserId",
                        column: x => x.DelegatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowApprovalSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowStepId = table.Column<int>(type: "int", nullable: false),
                    TargetRequestId = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    ApproverUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelegation = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReturnToStepId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowApprovalSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowApprovalSteps_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStepApprovalLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowApprovalStepId = table.Column<int>(type: "int", nullable: false),
                    WorkflowStepId = table.Column<int>(type: "int", nullable: true),
                    OldRequestStatus = table.Column<int>(type: "int", nullable: false),
                    NewRequestStatus = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepApprovalLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStepApprovalLog_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStepNotifiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowStepId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepNotifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStepNotifiers_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowStepNotifiers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowStepNotifiers_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStepTransitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceWorkflowStepId = table.Column<int>(type: "int", nullable: false),
                    TargetWorkflowStepId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStepTransitions_WorkflowSteps_SourceWorkflowStepId",
                        column: x => x.SourceWorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowStepTransitions_WorkflowSteps_TargetWorkflowStepId",
                        column: x => x.TargetWorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Discards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discards_BaseRequests_Id",
                        column: x => x.Id,
                        principalTable: "BaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    IsFromAllowance = table.Column<bool>(type: "bit", nullable: false),
                    UsageDateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageTimeFrom = table.Column<TimeOnly>(type: "time", nullable: false),
                    UsageDateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageTimeTo = table.Column<TimeOnly>(type: "time", nullable: false),
                    UsagePurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnualDiscard = table.Column<int>(type: "int", nullable: true),
                    UsageLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfOfficer = table.Column<int>(type: "int", nullable: true),
                    NumberOfOtherRank = table.Column<int>(type: "int", nullable: true),
                    SupplyDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_BaseRequests_Id",
                        column: x => x.Id,
                        principalTable: "BaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    RequestId = table.Column<long>(type: "bigint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestItems_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestItems_BaseRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "BaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returns_BaseRequests_Id",
                        column: x => x.Id,
                        principalTable: "BaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationReceivers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationReceivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationReceivers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationReceivers_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetSupplies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    SupplyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmissionStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    FulfillmentStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    CustodianId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReceiverMilitaryId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceiverRankId = table.Column<long>(type: "bigint", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpectedReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetSupplies_AspNetUsers_CustodianId",
                        column: x => x.CustodianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetSupplies_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetSupplies_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetSupplies_Ranks_ReceiverRankId",
                        column: x => x.ReceiverRankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    SupplyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecieverName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReceiverRankId = table.Column<long>(type: "bigint", nullable: true),
                    RecieverMilitaryId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SubmissionStatus = table.Column<int>(type: "int", nullable: false),
                    FulfillmentStatus = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Supplies_Ranks_ReceiverRankId",
                        column: x => x.ReceiverRankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplyDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Lot = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyDetails_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplyDetails_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    AssetSupplyId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    CustodianId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Purpose = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ConditionOnAssign = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConditionOnReturn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReceiverMilitaryId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceiverRankId = table.Column<long>(type: "bigint", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_AspNetUsers_CustodianId",
                        column: x => x.CustodianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_AssetSupplies_AssetSupplyId",
                        column: x => x.AssetSupplyId,
                        principalTable: "AssetSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Ranks_ReceiverRankId",
                        column: x => x.ReceiverRankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RFID = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DepotId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    AssetTag = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarrantyExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CurrentAssignmentId = table.Column<long>(type: "bigint", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AssetAssignments_CurrentAssignmentId",
                        column: x => x.CurrentAssignmentId,
                        principalTable: "AssetAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: true),
                    NewStatus = table.Column<int>(type: "int", nullable: true),
                    PreviousDepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    NewDepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    PreviousCustodianId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    NewCustodianId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    PreviousLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NewLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    AssetSupplyId = table.Column<long>(type: "bigint", nullable: true),
                    AssetAssignmentId = table.Column<long>(type: "bigint", nullable: true),
                    PerformedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    PerformedByUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetHistory_AspNetUsers_NewCustodianId",
                        column: x => x.NewCustodianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_AspNetUsers_PreviousCustodianId",
                        column: x => x.PreviousCustodianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_AssetAssignments_AssetAssignmentId",
                        column: x => x.AssetAssignmentId,
                        principalTable: "AssetAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_AssetSupplies_AssetSupplyId",
                        column: x => x.AssetSupplyId,
                        principalTable: "AssetSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_Departments_NewDepartmentId",
                        column: x => x.NewDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_Departments_PreviousDepartmentId",
                        column: x => x.PreviousDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetSupplyDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetSupplyId = table.Column<long>(type: "bigint", nullable: false),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNo = table.Column<int>(type: "int", nullable: false),
                    ConditionOnSupply = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustodianId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetSupplyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetSupplyDetails_AspNetUsers_CustodianId",
                        column: x => x.CustodianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetSupplyDetails_AssetSupplies_AssetSupplyId",
                        column: x => x.AssetSupplyId,
                        principalTable: "AssetSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetSupplyDetails_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetSupplyDetails_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    RequestItemId = table.Column<long>(type: "bigint", nullable: true),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    PreviousQuantity = table.Column<long>(type: "bigint", nullable: true),
                    NewQuantity = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedQuantity = table.Column<long>(type: "bigint", nullable: true),
                    SuppliedQuantity = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ModifiedByUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WorkflowApprovalStepId = table.Column<int>(type: "int", nullable: true),
                    WorkflowStepId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SupplyId = table.Column<long>(type: "bigint", nullable: true),
                    AssetSupplyId = table.Column<long>(type: "bigint", nullable: true),
                    SupplyDetailId = table.Column<long>(type: "bigint", nullable: true),
                    AssetSupplyDetailId = table.Column<long>(type: "bigint", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_AspNetUsers_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_AssetSupplies_AssetSupplyId",
                        column: x => x.AssetSupplyId,
                        principalTable: "AssetSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_AssetSupplyDetails_AssetSupplyDetailId",
                        column: x => x.AssetSupplyDetailId,
                        principalTable: "AssetSupplyDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_RequestItems_RequestItemId",
                        column: x => x.RequestItemId,
                        principalTable: "RequestItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_SupplyDetails_SupplyDetailId",
                        column: x => x.SupplyDetailId,
                        principalTable: "SupplyDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_WorkflowApprovalSteps_WorkflowApprovalStepId",
                        column: x => x.WorkflowApprovalStepId,
                        principalTable: "WorkflowApprovalSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemHistory_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CaseTypes",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "نحاسي", "Brass" },
                    { 2L, null, false, null, null, "فولاذي", "Steel" },
                    { 3L, null, false, null, null, "ألومنيوم", "Aluminum" },
                    { 4L, null, false, null, null, "بلاستيك", "Plastic" },
                    { 5L, null, false, null, null, "مختلط", "Composite" }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "IsDeleted", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, false, "أخضر", "Green" },
                    { 2L, false, "أسود", "Black" },
                    { 3L, false, "أصفر", "Yellow" },
                    { 4L, false, "أحمر", "Red" },
                    { 5L, false, "رمادي", "Gray" }
                });

            migrationBuilder.InsertData(
                table: "Compatibilities",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "المجموعة أ", "Group A" },
                    { 2L, null, false, null, null, "المجموعة ب", "Group B" },
                    { 3L, null, false, null, null, "المجموعة ج", "Group C" },
                    { 4L, null, false, null, null, "المجموعة د", "Group D" },
                    { 5L, null, false, null, null, "المجموعة هـ", "Group E" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, "QELF", "SYSTEM", false, null, null, "القوات البرية الأميرية القطرية", "Qatar Emiri Land Forces" },
                    { 2L, "QEAF", "SYSTEM", false, null, null, "القوات الجوية الأميرية القطرية", "Qatar Emiri Air Force" },
                    { 3L, "QENF", "SYSTEM", false, null, null, "القوات البحرية الأميرية القطرية", "Qatar Emiri Navy" },
                    { 4L, "EGD", "SYSTEM", false, null, null, "الحرس الأميري", "Emiri Guard Directorate" },
                    { 5L, "JSFC", "SYSTEM", false, null, null, "قيادة القوات الخاصة المشتركة", "Joint Special Forces Command" },
                    { 6L, "NSAC", "SYSTEM", false, null, null, "أكاديمية الخدمة الوطنية", "National Service Academy" },
                    { 7L, "MID", "SYSTEM", false, null, null, "مديرية الاستخبارات العسكرية", "Military Intelligence Directorate" },
                    { 8L, "LOGC", "SYSTEM", false, null, null, "قيادة الإمداد والتموين", "Logistics and Supply Command" },
                    { 9L, "MP", "SYSTEM", false, null, null, "قيادة الشرطة العسكرية", "Military Police Command" },
                    { 10L, "MED", "SYSTEM", false, null, null, "الخدمات الطبية للقوات المسلحة", "Armed Forces Medical Services" },
                    { 11L, "TRAD", "SYSTEM", false, null, null, "قيادة التدريب والعقيدة", "Training and Doctrine Command" },
                    { 12L, "ADC", "SYSTEM", false, null, null, "قيادة الدفاع الجوي", "Air Defense Command" },
                    { 13L, "CYBC", "SYSTEM", false, null, null, "قيادة الاتصالات والدفاع السيبراني", "Cyber Defense & Communications Command" },
                    { 14L, "MT", "SYSTEM", false, null, null, "مديرية التدريب العسكري", "Military Training" },
                    { 15L, "DoA", "SYSTEM", false, null, null, "مديرية التسليح", "Directorate of Armament" },
                    { 16L, "MO", "SYSTEM", false, null, null, "هئية العمليات", "Military Operation" },
                    { 17L, "CoS", "SYSTEM", false, null, null, "مكتب رئيس الأركان", "Chief of Staff Office" },
                    { 18L, "Inventory", "SYSTEM", false, null, null, "مستودعات الأسلحة والذخيرة المركزيه", "Inventory" }
                });

            migrationBuilder.InsertData(
                table: "Depots",
                columns: new[] { "Id", "Code", "CreatedBy", "IsDeleted", "Latitude", "Location", "Longitude", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, "DEP-001", null, false, 25.2854m, "Doha", 51.5310m, null, null, "مستودع الدوحة المركزي", "Doha Central Depot" },
                    { 2L, "DEP-002", null, false, 25.2866m, "Al Rayyan", 51.4244m, null, null, "مستودع الريان الغربي", "Al Rayyan West Depot" },
                    { 3L, "DEP-003", null, false, 25.6800m, "Al Khor", 51.5059m, null, null, "مستودع الخور الشمالي", "Al Khor North Depot" },
                    { 4L, "DEP-004", null, false, 25.1657m, "Al Wakrah", 51.6034m, null, null, "مستودع الوكرة الجنوبي", "Al Wakrah South Depot" }
                });

            migrationBuilder.InsertData(
                table: "HazardDivisions",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "القسم 1.1 - مواد متفجرة", "Division 1.1 - Explosives" },
                    { 2L, null, false, null, null, "القسم 1.2 - مواد قابلة للانفجار", "Division 1.2 - Projection Hazard" },
                    { 3L, null, false, null, null, "القسم 1.3 - مواد قابلة للاشتعال", "Division 1.3 - Fire Hazard" },
                    { 4L, null, false, null, null, "القسم 1.4 - مواد منخفضة المخاطر", "Division 1.4 - Minor Hazard" },
                    { 5L, null, false, null, null, "القسم 1.5 - مواد غير حساسة", "Division 1.5 - Very Insensitive" }
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "مصنع الذخائر الملكي", "Royal Ordnance Factory" },
                    { 2L, null, false, null, null, "شركة رايثيون", "Raytheon Company" },
                    { 3L, null, false, null, null, "مؤسسة الصناعات العسكرية الوطنية", "National Military Industries" },
                    { 4L, null, false, null, null, "شركة لوكهيد مارتن", "Lockheed Martin" },
                    { 5L, null, false, null, null, "مجموعة بي إيه إي سيستمز", "BAE Systems" }
                });

            migrationBuilder.InsertData(
                table: "NatureOptions",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "ذخيرة حية", "Live Ammunition" },
                    { 2L, null, false, null, null, "صوتي", "Sonic" },
                    { 3L, null, false, null, null, "مشرح", "Fragmentation" },
                    { 4L, null, false, null, null, "كاشف", "Detector" },
                    { 5L, null, false, null, null, "مائت", "Inert" },
                    { 6L, null, false, null, null, "متفجر", "Explosive" },
                    { 7L, null, false, null, null, "خارق", "Armor-Piercing" },
                    { 8L, null, false, null, null, "دخاني", "Smoke" },
                    { 9L, null, false, null, null, "انارة", "Illuminating" },
                    { 10L, null, false, null, null, "حارق", "Incendiary" },
                    { 11L, null, false, null, null, "خارق حارق", "Armor-Piercing Incendiary" }
                });

            migrationBuilder.InsertData(
                table: "PrimaryPurposes",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "قتالي", "Combat" },
                    { 2L, null, false, null, null, "تدريبي", "Training" },
                    { 3L, null, false, null, null, "دفاعي", "Defense" },
                    { 4L, null, false, null, null, "استطلاعي", "Reconnaissance" },
                    { 5L, null, false, null, null, "هجومي", "Offensive" }
                });

            migrationBuilder.InsertData(
                table: "ProjectailMaterials",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "فولاذ", "Steel" },
                    { 2L, null, false, null, null, "نحاس", "Brass" },
                    { 3L, null, false, null, null, "رصاص", "Lead" },
                    { 4L, null, false, null, null, "تنغستن", "Tungsten" },
                    { 5L, null, false, null, null, "يورانيوم منضب", "Depleted Uranium" }
                });

            migrationBuilder.InsertData(
                table: "Propellants",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "بارود أحادي القاعدة", "Single-base Powder" },
                    { 2L, null, false, null, null, "بارود ثنائي القاعدة", "Double-base Powder" },
                    { 3L, null, false, null, null, "بارود ثلاثي القاعدة", "Triple-base Powder" },
                    { 4L, null, false, null, null, "نيتروسليلوز", "Nitrocellulose" },
                    { 5L, null, false, null, null, "كورديت", "Cordite" }
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "عقيد", "Colonel" },
                    { 2L, null, false, null, null, "مقدم", "Lieutenant Colonel" },
                    { 3L, null, false, null, null, "رائد", "Major" },
                    { 4L, null, false, null, null, "نقيب", "Captain" },
                    { 5L, null, false, null, null, "ملازم أول", "First Lieutenant" },
                    { 6L, null, false, null, null, "ملازم", "Second Lieutenant" },
                    { 7L, null, false, null, null, "رقيب أول", "Master Sergeant" },
                    { 8L, null, false, null, null, "رقيب", "Sergeant" },
                    { 9L, null, false, null, null, "عريف", "Corporal" },
                    { 10L, null, false, null, null, "جندي", "Private" }
                });

            migrationBuilder.InsertData(
                table: "RequestPurposes",
                columns: new[] { "Id", "CreatedBy", "DeletedBy", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn", "RequestType" },
                values: new object[,]
                {
                    { 1L, null, null, null, false, null, null, "طلب عادي", "Normal Order", 1 },
                    { 2L, null, null, null, false, null, null, "طلب خدمة", "Duty Order", 1 },
                    { 3L, null, null, null, false, null, null, "طلب عملية", "Operation Order", 1 },
                    { 4L, null, null, null, false, null, null, "طلب تدريبي", "Training Order", 1 },
                    { 5L, null, null, null, false, null, null, "إرجاع عادي", "Normal Return", 2 },
                    { 6L, null, null, null, false, null, null, "إرجاع بعد انتهاء الخدمة", "Return After Service", 2 },
                    { 7L, null, null, null, false, null, null, "إرجاع بعد العملية", "Return After Operation", 2 },
                    { 8L, null, null, null, false, null, null, "إرجاع بعد التدريب", "Return After Training", 2 },
                    { 9L, null, null, null, false, null, null, "تسديد تالف", "Damaged Discard", 3 },
                    { 10L, null, null, null, false, null, null, "تسديد منتهي الصلاحية", "Expired Discard", 3 },
                    { 11L, null, null, null, false, null, null, "تسديد غير مستخدم", "Unused Discard", 3 },
                    { 12L, null, null, null, false, null, null, "تطهير الميدان", "Field Clearance", 1 }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "CreatedBy", "Group", "Key", "ModificationDate", "ModifiedBy", "Value" },
                values: new object[,]
                {
                    { 1, null, "LDAP", "LdapServer", null, null, "10.80.70.3" },
                    { 2, null, "LDAP", "LdapDomain", null, null, "sddev.local" },
                    { 3, null, "LDAP", "LdapEmpAttr", null, null, "sAMAccountName" },
                    { 4, null, "LDAP", "LdapUsername", null, null, "1000" },
                    { 5, null, "LDAP", "LdapPassword", null, null, "Qatar@2025" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "شركة الإمدادات العسكرية المتقدمة", "Advanced Military Supplies Co." },
                    { 2L, null, false, null, null, "المؤسسة العامة للتسليح", "General Armament Corporation" },
                    { 3L, null, false, null, null, "شركة الصناعات الدفاعية", "Defense Industries Company" },
                    { 4L, null, false, null, null, "مجموعة التجهيزات العسكرية", "Military Equipment Group" },
                    { 5L, null, false, null, null, "شركة التوريدات الاستراتيجية", "Strategic Supplies Corporation" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ItemType", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, 1, null, null, "غرام", "Gram" },
                    { 2L, null, false, 1, null, null, "مليمتر", "Millimeter" },
                    { 3L, null, false, 1, null, null, "قطعة", "Piece" },
                    { 4L, null, false, 2, null, null, "غرام", "Gram" },
                    { 5L, null, false, 2, null, null, "مليمتر", "Millimeter" },
                    { 6L, null, false, 2, null, null, "قطعة", "Piece" },
                    { 7L, null, false, 3, null, null, "غرام", "Gram" },
                    { 8L, null, false, 3, null, null, "متر", "Meter" }
                });

            migrationBuilder.InsertData(
                table: "WorkFlowType",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "Request", "Request" },
                    { 2L, null, false, null, null, "Discard", "Discard" },
                    { 3L, null, false, null, null, "Return", "Return" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceItems_DepartmentId",
                table: "AllowanceItems",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceItems_ItemId_DepartmentId_Year",
                table: "AllowanceItems",
                columns: new[] { "ItemId", "DepartmentId", "Year" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_BulletDiameterUnitId",
                table: "Ammunitions",
                column: "BulletDiameterUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_CaseTypeId",
                table: "Ammunitions",
                column: "CaseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_CompatibilityId",
                table: "Ammunitions",
                column: "CompatibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_HazardDivisionId",
                table: "Ammunitions",
                column: "HazardDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_NatureOptionId",
                table: "Ammunitions",
                column: "NatureOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_PrimaryPurposId",
                table: "Ammunitions",
                column: "PrimaryPurposId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_ProjectailMaterialId",
                table: "Ammunitions",
                column: "ProjectailMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_ProjectileColorId",
                table: "Ammunitions",
                column: "ProjectileColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_PropellantId",
                table: "Ammunitions",
                column: "PropellantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEntity_Code",
                table: "ApplicationEntity",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEntity_NameAr",
                table: "ApplicationEntity",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEntity_NameEn",
                table: "ApplicationEntity",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RankId",
                table: "AspNetUsers",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_AssetId_Status",
                table: "AssetAssignments",
                columns: new[] { "AssetId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_AssetSupplyId",
                table: "AssetAssignments",
                column: "AssetSupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_CustodianId",
                table: "AssetAssignments",
                column: "CustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_DepartmentId",
                table: "AssetAssignments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_OrderId",
                table: "AssetAssignments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_ReceiverRankId",
                table: "AssetAssignments",
                column: "ReceiverRankId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_ActionDate",
                table: "AssetHistory",
                column: "ActionDate");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_ActionType",
                table: "AssetHistory",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_AssetAssignmentId",
                table: "AssetHistory",
                column: "AssetAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_AssetId",
                table: "AssetHistory",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_AssetSupplyId",
                table: "AssetHistory",
                column: "AssetSupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_NewCustodianId",
                table: "AssetHistory",
                column: "NewCustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_NewDepartmentId",
                table: "AssetHistory",
                column: "NewDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_OrderId",
                table: "AssetHistory",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_PreviousCustodianId",
                table: "AssetHistory",
                column: "PreviousCustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_PreviousDepartmentId",
                table: "AssetHistory",
                column: "PreviousDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CurrentAssignmentId",
                table: "Assets",
                column: "CurrentAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_DepotId",
                table: "Assets",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_IsAssigned",
                table: "Assets",
                column: "IsAssigned");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ItemId",
                table: "Assets",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_RFID",
                table: "Assets",
                column: "RFID",
                filter: "[RFID] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_SerialNumber",
                table: "Assets",
                column: "SerialNumber",
                unique: true,
                filter: "[SerialNumber] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplies_CustodianId",
                table: "AssetSupplies",
                column: "CustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplies_DepartmentId",
                table: "AssetSupplies",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplies_OrderId",
                table: "AssetSupplies",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplies_ReceiverRankId",
                table: "AssetSupplies",
                column: "ReceiverRankId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplies_SubmissionStatus",
                table: "AssetSupplies",
                column: "SubmissionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplyDetails_AssetId",
                table: "AssetSupplyDetails",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplyDetails_AssetSupplyId",
                table: "AssetSupplyDetails",
                column: "AssetSupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplyDetails_CustodianId",
                table: "AssetSupplyDetails",
                column: "CustodianId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplyDetails_ItemId",
                table: "AssetSupplyDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_ClassificationId",
                table: "BaseItems",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_ClassificationId_IsDeleted",
                table: "BaseItems",
                columns: new[] { "ClassificationId", "IsDeleted" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_IsDeleted",
                table: "BaseItems",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_ItemNo",
                table: "BaseItems",
                column: "ItemNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_Name",
                table: "BaseItems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_TypeId",
                table: "BaseItems",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_TypeId_IsDeleted",
                table: "BaseItems",
                columns: new[] { "TypeId", "IsDeleted" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BaseRequests_DepartmentId",
                table: "BaseRequests",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseRequests_RequesterId",
                table: "BaseRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseRequests_RequestNo",
                table: "BaseRequests",
                column: "RequestNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseRequests_RequestPurposeId",
                table: "BaseRequests",
                column: "RequestPurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_ExpiresAt",
                table: "BlacklistedTokens",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_TokenId",
                table: "BlacklistedTokens",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_UserId",
                table: "BlacklistedTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseTypes_NameAr",
                table: "CaseTypes",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CaseTypes_NameEn",
                table: "CaseTypes",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Classifications_NameAr",
                table: "Classifications",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Classifications_NameEn",
                table: "Classifications",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameAr",
                table: "Colors",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameEn",
                table: "Colors",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Compatibilities_NameAr",
                table: "Compatibilities",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Compatibilities_NameEn",
                table: "Compatibilities",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameAr",
                table: "Countries",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameEn",
                table: "Countries",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_NameAr",
                table: "Departments",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_NameEn",
                table: "Departments",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Depots_Code",
                table: "Depots",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Depots_NameAr",
                table: "Depots",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Depots_NameEn",
                table: "Depots",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Explosives_HazardDivisionId",
                table: "Explosives",
                column: "HazardDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Explosives_UnitId",
                table: "Explosives",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUplodDetails_FileUplodMasterId",
                table: "FileUplodDetails",
                column: "FileUplodMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_HazardDivisions_NameAr",
                table: "HazardDivisions",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_HazardDivisions_NameEn",
                table: "HazardDivisions",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_DepoId",
                table: "Inventories",
                column: "DepoId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_CountryId",
                table: "InventoryDetails",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_InventoryId",
                table: "InventoryDetails",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_ItemId",
                table: "InventoryDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_ManufacturerId",
                table: "InventoryDetails",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_SupplierId",
                table: "InventoryDetails",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_NameAr_ItemType",
                table: "ItemTypes",
                columns: new[] { "NameAr", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_NameEn_ItemType",
                table: "ItemTypes",
                columns: new[] { "NameEn", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_AttemptDate",
                table: "LoginAttempts",
                column: "AttemptDate");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_UserId",
                table: "LoginAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_Username",
                table: "LoginAttempts",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_Username_AttemptDate",
                table: "LoginAttempts",
                columns: new[] { "Username", "AttemptDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_NameAr",
                table: "Manufacturers",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_NameEn",
                table: "Manufacturers",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_NatureOptions_NameAr",
                table: "NatureOptions",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_NatureOptions_NameEn",
                table: "NatureOptions",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationReceivers_IsRead",
                table: "NotificationReceivers",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationReceivers_NotificationId",
                table: "NotificationReceivers",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationReceivers_UserId",
                table: "NotificationReceivers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreationDate",
                table: "Notifications",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EntityId",
                table: "Notifications",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EntityType",
                table: "Notifications",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_ActionDate",
                table: "OrderItemHistory",
                column: "ActionDate");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_ActionType",
                table: "OrderItemHistory",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_AssetSupplyDetailId",
                table: "OrderItemHistory",
                column: "AssetSupplyDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_AssetSupplyId",
                table: "OrderItemHistory",
                column: "AssetSupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_DepartmentId",
                table: "OrderItemHistory",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_ItemId",
                table: "OrderItemHistory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_ModifiedByUserId",
                table: "OrderItemHistory",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_OrderId",
                table: "OrderItemHistory",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_OrderId_ItemId",
                table: "OrderItemHistory",
                columns: new[] { "OrderId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_RequestItemId",
                table: "OrderItemHistory",
                column: "RequestItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_SupplyDetailId",
                table: "OrderItemHistory",
                column: "SupplyDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_SupplyId",
                table: "OrderItemHistory",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_WorkflowApprovalStepId",
                table: "OrderItemHistory",
                column: "WorkflowApprovalStepId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemHistory_WorkflowStepId",
                table: "OrderItemHistory",
                column: "WorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryPurposes_NameAr",
                table: "PrimaryPurposes",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryPurposes_NameEn",
                table: "PrimaryPurposes",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectailMaterials_NameAr",
                table: "ProjectailMaterials",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectailMaterials_NameEn",
                table: "ProjectailMaterials",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Propellants_NameAr",
                table: "Propellants",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Propellants_NameEn",
                table: "Propellants",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_NameAr",
                table: "Ranks",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_NameEn",
                table: "Ranks",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_ItemId",
                table: "RequestItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_RequestId",
                table: "RequestItems",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPurposes_NameAr",
                table: "RequestPurposes",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestPurposes_NameEn",
                table: "RequestPurposes",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_NameAr",
                table: "Suppliers",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_NameEn",
                table: "Suppliers",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_OrderId",
                table: "Supplies",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_ReceiverRankId",
                table: "Supplies",
                column: "ReceiverRankId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyDetails_ItemId",
                table: "SupplyDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyDetails_SupplyId",
                table: "SupplyDetails",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameAr_ItemType",
                table: "Units",
                columns: new[] { "NameAr", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameEn_ItemType",
                table: "Units",
                columns: new[] { "NameEn", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserDelegations_DelegateeUserId",
                table: "UserDelegations",
                column: "DelegateeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDelegations_DelegatorUserId",
                table: "UserDelegations",
                column: "DelegatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CaliberUnitId",
                table: "Weapons",
                column: "CaliberUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CountryOfManufactureId",
                table: "Weapons",
                column: "CountryOfManufactureId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowApprovalSteps_TargetRequestId_IsCurrent",
                table: "WorkflowApprovalSteps",
                columns: new[] { "TargetRequestId", "IsCurrent" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowApprovalSteps_WorkflowStepId",
                table: "WorkflowApprovalSteps",
                column: "WorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepApprovalLog_WorkflowStepId",
                table: "WorkflowStepApprovalLog",
                column: "WorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepNotifiers_RoleId",
                table: "WorkflowStepNotifiers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepNotifiers_UserId",
                table: "WorkflowStepNotifiers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepNotifiers_WorkflowStepId",
                table: "WorkflowStepNotifiers",
                column: "WorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_ApplicationRoleId",
                table: "WorkflowSteps",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_HigherApplicationEntityId",
                table: "WorkflowSteps",
                column: "HigherApplicationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_HigherApprovalRoleId",
                table: "WorkflowSteps",
                column: "HigherApprovalRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_WorkflowId",
                table: "WorkflowSteps",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepTransitions_SourceWorkflowStepId",
                table: "WorkflowStepTransitions",
                column: "SourceWorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepTransitions_TargetWorkflowStepId",
                table: "WorkflowStepTransitions",
                column: "TargetWorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowType_NameAr",
                table: "WorkFlowType",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowType_NameEn",
                table: "WorkFlowType",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_Assets_AssetId",
                table: "AssetAssignments",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_BaseItems_ItemId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Departments_DepartmentId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplies_Departments_DepartmentId",
                table: "AssetSupplies");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseRequests_Departments_DepartmentId",
                table: "BaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_AspNetUsers_CustodianId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplies_AspNetUsers_CustodianId",
                table: "AssetSupplies");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseRequests_AspNetUsers_RequesterId",
                table: "BaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Ranks_ReceiverRankId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplies_Ranks_ReceiverRankId",
                table: "AssetSupplies");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_AssetSupplies_AssetSupplyId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Assets_AssetId",
                table: "AssetAssignments");

            migrationBuilder.DropTable(
                name: "AllowanceItems");

            migrationBuilder.DropTable(
                name: "Ammunitions");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AssetHistory");

            migrationBuilder.DropTable(
                name: "BlacklistedTokens");

            migrationBuilder.DropTable(
                name: "Discards");

            migrationBuilder.DropTable(
                name: "EmailConfigurations");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Explosives");

            migrationBuilder.DropTable(
                name: "FileUplodDetails");

            migrationBuilder.DropTable(
                name: "InventoryDetails");

            migrationBuilder.DropTable(
                name: "LoginAttempts");

            migrationBuilder.DropTable(
                name: "NotificationReceivers");

            migrationBuilder.DropTable(
                name: "OrderItemHistory");

            migrationBuilder.DropTable(
                name: "Returns");

            migrationBuilder.DropTable(
                name: "RoleApplicationEntities");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "UserDelegations");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "WorkflowStepApprovalLog");

            migrationBuilder.DropTable(
                name: "WorkflowStepNotifiers");

            migrationBuilder.DropTable(
                name: "WorkflowStepTransitions");

            migrationBuilder.DropTable(
                name: "WorkFlowType");

            migrationBuilder.DropTable(
                name: "CaseTypes");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Compatibilities");

            migrationBuilder.DropTable(
                name: "NatureOptions");

            migrationBuilder.DropTable(
                name: "PrimaryPurposes");

            migrationBuilder.DropTable(
                name: "ProjectailMaterials");

            migrationBuilder.DropTable(
                name: "Propellants");

            migrationBuilder.DropTable(
                name: "HazardDivisions");

            migrationBuilder.DropTable(
                name: "FileUplodMaster");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "AssetSupplyDetails");

            migrationBuilder.DropTable(
                name: "RequestItems");

            migrationBuilder.DropTable(
                name: "SupplyDetails");

            migrationBuilder.DropTable(
                name: "WorkflowApprovalSteps");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "WorkflowSteps");

            migrationBuilder.DropTable(
                name: "ApplicationEntity");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Workflows");

            migrationBuilder.DropTable(
                name: "BaseItems");

            migrationBuilder.DropTable(
                name: "Classifications");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropTable(
                name: "AssetSupplies");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "AssetAssignments");

            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "BaseRequests");

            migrationBuilder.DropTable(
                name: "RequestPurposes");
        }
    }
}
