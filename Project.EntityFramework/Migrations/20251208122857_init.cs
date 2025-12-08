using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    Nsn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumQuantity = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_BaseItems", x => x.Id);
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseTypes", x => x.Id);
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HazardDivisions", x => x.Id);
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                name: "Explosives",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
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
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepoId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecievedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
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
                    NumberOfOtherRank = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.InsertData(
                table: "BaseItems",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "IsDeleted", "ItemNo", "ItemType", "MinimumQuantity", "ModificationDate", "ModifiedBy", "Name", "Nsn", "PartNo", "Price" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-001", 1, 200L, null, null, "5.56x45mm NATO", "1305-01-000-0001", "PN-556-001", 0.65m },
                    { 2L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-002", 1, 150L, null, null, "7.62x51mm NATO", "1305-01-000-0002", "PN-762-001", 1.25m },
                    { 3L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-003", 1, 200L, null, null, "9x19mm Parabellum", "1305-01-000-0003", "PN-9MM-001", 0.70m },
                    { 4L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-004", 1, 50L, null, null, ".50 BMG", "1305-01-000-0004", "PN-50BMG-001", 3.50m },
                    { 5L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-005", 1, 100L, null, null, ".308 Winchester", "1305-01-000-0005", "PN-308-001", 1.50m },
                    { 6L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-006", 1, 150L, null, null, ".45 ACP", "1305-01-000-0006", "PN-45ACP-001", 0.75m },
                    { 7L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-007", 1, 50L, null, null, "12.7x108mm", "1305-01-000-0007", "PN-127-001", 2.50m },
                    { 8L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-008", 1, 200L, null, null, "5.45x39mm", "1305-01-000-0008", "PN-545-001", 0.60m },
                    { 9L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "AMM-009", 1, 150L, null, null, ".40 S&W", "1305-01-000-0009", "PN-40SW-001", 0.80m }
                });

            migrationBuilder.InsertData(
                table: "CaseTypes",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(3552), false, null, null, "نحاسي", "Brass" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(3574), false, null, null, "فولاذي", "Steel" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(3575), false, null, null, "ألومنيوم", "Aluminum" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(3577), false, null, null, "بلاستيك", "Plastic" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(3578), false, null, null, "مختلط", "Composite" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(6986), false, null, null, "نحاسي", "Brass" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(7006), false, null, null, "فولاذي", "Steel" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(7009), false, null, null, "ألومنيوم", "Aluminum" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(7010), false, null, null, "بلاستيك", "Plastic" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(7011), false, null, null, "مختلط", "Composite" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
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
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(6429), false, null, null, "المجموعة أ", "Group A" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(6445), false, null, null, "المجموعة ب", "Group B" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(6447), false, null, null, "المجموعة ج", "Group C" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(6448), false, null, null, "المجموعة د", "Group D" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(6449), false, null, null, "المجموعة هـ", "Group E" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(9866), false, null, null, "المجموعة أ", "Group A" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(9883), false, null, null, "المجموعة ب", "Group B" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(9885), false, null, null, "المجموعة ج", "Group C" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(9886), false, null, null, "المجموعة د", "Group D" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 76, DateTimeKind.Unspecified).AddTicks(9888), false, null, null, "المجموعة هـ", "Group E" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "IsDeleted", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, "SA", false, "المملكة العربية السعودية", "Saudi Arabia" },
                    { 2L, "US", false, "الولايات المتحدة الأمريكية", "United States" },
                    { 3L, "UK", false, "المملكة المتحدة", "United Kingdom" },
                    { 4L, "FR", false, "فرنسا", "France" },
                    { 5L, "DE", false, "ألمانيا", "Germany" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, "QELF", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9667), false, null, null, "القوات البرية الأميرية القطرية", "Qatar Emiri Land Forces" },
                    { 2L, "QEAF", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9682), false, null, null, "القوات الجوية الأميرية القطرية", "Qatar Emiri Air Force" },
                    { 3L, "QENF", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9684), false, null, null, "القوات البحرية الأميرية القطرية", "Qatar Emiri Navy" },
                    { 4L, "EGD", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9685), false, null, null, "الحرس الأميري", "Emiri Guard Directorate" },
                    { 5L, "JSFC", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9687), false, null, null, "قيادة القوات الخاصة المشتركة", "Joint Special Forces Command" },
                    { 6L, "NSAC", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9688), false, null, null, "أكاديمية الخدمة الوطنية", "National Service Academy" },
                    { 7L, "MID", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9689), false, null, null, "مديرية الاستخبارات العسكرية", "Military Intelligence Directorate" },
                    { 8L, "LOGC", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9691), false, null, null, "قيادة الإمداد والتموين", "Logistics and Supply Command" },
                    { 9L, "MP", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9692), false, null, null, "قيادة الشرطة العسكرية", "Military Police Command" },
                    { 10L, "MED", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9693), false, null, null, "الخدمات الطبية للقوات المسلحة", "Armed Forces Medical Services" },
                    { 11L, "TRAD", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9695), false, null, null, "قيادة التدريب والعقيدة", "Training and Doctrine Command" },
                    { 12L, "ADC", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9696), false, null, null, "قيادة الدفاع الجوي", "Air Defense Command" },
                    { 13L, "CYBC", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9697), false, null, null, "قيادة الاتصالات والدفاع السيبراني", "Cyber Defense & Communications Command" },
                    { 14L, "MT", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9698), false, null, null, "مديرية التدريب العسكري", "Military Training" },
                    { 15L, "DoA", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9699), false, null, null, "مديرية التسليح", "Directorate of Armament" },
                    { 16L, "MO", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9700), false, null, null, "هئية العمليات", "Military Operation" },
                    { 17L, "CoS", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9702), false, null, null, "مكتب رئيس الأركان", "Chief of Staff Office" },
                    { 18L, "Inventory", "SYSTEM", new DateTime(2025, 12, 8, 15, 6, 22, 520, DateTimeKind.Unspecified).AddTicks(9703), false, null, null, "مستودعات الأسلحة والذخيرة المركزيه", "Inventory" }
========
                    { 1L, "QELF", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3239), false, null, null, "القوات البرية الأميرية القطرية", "Qatar Emiri Land Forces" },
                    { 2L, "QEAF", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3256), false, null, null, "القوات الجوية الأميرية القطرية", "Qatar Emiri Air Force" },
                    { 3L, "QENF", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3258), false, null, null, "القوات البحرية الأميرية القطرية", "Qatar Emiri Navy" },
                    { 4L, "EGD", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3259), false, null, null, "الحرس الأميري", "Emiri Guard Directorate" },
                    { 5L, "JSFC", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3261), false, null, null, "قيادة القوات الخاصة المشتركة", "Joint Special Forces Command" },
                    { 6L, "NSAC", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3262), false, null, null, "أكاديمية الخدمة الوطنية", "National Service Academy" },
                    { 7L, "MID", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3263), false, null, null, "مديرية الاستخبارات العسكرية", "Military Intelligence Directorate" },
                    { 8L, "LOGC", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3264), false, null, null, "قيادة الإمداد والتموين", "Logistics and Supply Command" },
                    { 9L, "MP", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3266), false, null, null, "قيادة الشرطة العسكرية", "Military Police Command" },
                    { 10L, "MED", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3267), false, null, null, "الخدمات الطبية للقوات المسلحة", "Armed Forces Medical Services" },
                    { 11L, "TRAD", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3268), false, null, null, "قيادة التدريب والعقيدة", "Training and Doctrine Command" },
                    { 12L, "ADC", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3270), false, null, null, "قيادة الدفاع الجوي", "Air Defense Command" },
                    { 13L, "CYBC", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3271), false, null, null, "قيادة الاتصالات والدفاع السيبراني", "Cyber Defense & Communications Command" },
                    { 14L, "MT", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3272), false, null, null, "مديرية التدريب العسكري", "Military Training" },
                    { 15L, "DoA", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3273), false, null, null, "مديرية التسليح", "Directorate of Armament" },
                    { 16L, "MO", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3274), false, null, null, "هئية العمليات", "Military Operation" },
                    { 17L, "CoS", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3275), false, null, null, "مكتب رئيس الأركان", "Chief of Staff Office" },
                    { 18L, "Inventory", "SYSTEM", new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(3277), false, null, null, "مستودعات الأسلحة والذخيرة المركزيه", "Inventory" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Depots",
                columns: new[] { "Id", "Code", "CreatedBy", "CreationDate", "IsDeleted", "Latitude", "Location", "Longitude", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, "DEP-001", null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(1241), false, 25.2854m, "Doha", 51.5310m, null, null, "مستودع الدوحة المركزي", "Doha Central Depot" },
                    { 2L, "DEP-002", null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(1253), false, 25.2866m, "Al Rayyan", 51.4244m, null, null, "مستودع الريان الغربي", "Al Rayyan West Depot" },
                    { 3L, "DEP-003", null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(1256), false, 25.6800m, "Al Khor", 51.5059m, null, null, "مستودع الخور الشمالي", "Al Khor North Depot" },
                    { 4L, "DEP-004", null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(1258), false, 25.1657m, "Al Wakrah", 51.6034m, null, null, "مستودع الوكرة الجنوبي", "Al Wakrah South Depot" }
========
                    { 1L, "DEP-001", null, new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(4801), false, 25.2854m, "Doha", 51.5310m, null, null, "مستودع الدوحة المركزي", "Doha Central Depot" },
                    { 2L, "DEP-002", null, new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(4814), false, 25.2866m, "Al Rayyan", 51.4244m, null, null, "مستودع الريان الغربي", "Al Rayyan West Depot" },
                    { 3L, "DEP-003", null, new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(4842), false, 25.6800m, "Al Khor", 51.5059m, null, null, "مستودع الخور الشمالي", "Al Khor North Depot" },
                    { 4L, "DEP-004", null, new DateTime(2025, 12, 8, 15, 28, 57, 77, DateTimeKind.Unspecified).AddTicks(4845), false, 25.1657m, "Al Wakrah", 51.6034m, null, null, "مستودع الوكرة الجنوبي", "Al Wakrah South Depot" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "HazardDivisions",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(7419), false, null, null, "القسم 1.1 - مواد متفجرة", "Division 1.1 - Explosives" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(7437), false, null, null, "القسم 1.2 - مواد قابلة للانفجار", "Division 1.2 - Projection Hazard" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(7439), false, null, null, "القسم 1.3 - مواد قابلة للاشتعال", "Division 1.3 - Fire Hazard" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(7440), false, null, null, "القسم 1.4 - مواد منخفضة المخاطر", "Division 1.4 - Minor Hazard" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 521, DateTimeKind.Unspecified).AddTicks(7441), false, null, null, "القسم 1.5 - مواد غير حساسة", "Division 1.5 - Very Insensitive" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(653), false, null, null, "القسم 1.1 - مواد متفجرة", "Division 1.1 - Explosives" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(672), false, null, null, "القسم 1.2 - مواد قابلة للانفجار", "Division 1.2 - Projection Hazard" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(673), false, null, null, "القسم 1.3 - مواد قابلة للاشتعال", "Division 1.3 - Fire Hazard" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(675), false, null, null, "القسم 1.4 - مواد منخفضة المخاطر", "Division 1.4 - Minor Hazard" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(676), false, null, null, "القسم 1.5 - مواد غير حساسة", "Division 1.5 - Very Insensitive" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(5038), false, null, null, "مصنع الذخائر الملكي", "Royal Ordnance Factory" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(5057), false, null, null, "شركة رايثيون", "Raytheon Company" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(5059), false, null, null, "مؤسسة الصناعات العسكرية الوطنية", "National Military Industries" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(5060), false, null, null, "شركة لوكهيد مارتن", "Lockheed Martin" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(5061), false, null, null, "مجموعة بي إيه إي سيستمز", "BAE Systems" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(8292), false, null, null, "مصنع الذخائر الملكي", "Royal Ordnance Factory" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(8310), false, null, null, "شركة رايثيون", "Raytheon Company" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(8312), false, null, null, "مؤسسة الصناعات العسكرية الوطنية", "National Military Industries" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(8313), false, null, null, "شركة لوكهيد مارتن", "Lockheed Martin" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 78, DateTimeKind.Unspecified).AddTicks(8315), false, null, null, "مجموعة بي إيه إي سيستمز", "BAE Systems" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "NatureOptions",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6455), false, null, null, "ذخيرة حية", "Live Ammunition" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6464), false, null, null, "صوتي", "Sonic" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6513), false, null, null, "مشرح", "Fragmentation" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6517), false, null, null, "كاشف", "Detector" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6518), false, null, null, "مائت", "Inert" },
                    { 6L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6520), false, null, null, "متفجر", "Explosive" },
                    { 7L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6521), false, null, null, "خارق", "Armor-Piercing" },
                    { 8L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6522), false, null, null, "دخاني", "Smoke" },
                    { 9L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6523), false, null, null, "انارة", "Illuminating" },
                    { 10L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6524), false, null, null, "حارق", "Incendiary" },
                    { 11L, null, new DateTime(2025, 12, 8, 15, 6, 22, 522, DateTimeKind.Unspecified).AddTicks(6525), false, null, null, "خارق حارق", "Armor-Piercing Incendiary" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(38), false, null, null, "ذخيرة حية", "Live Ammunition" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(54), false, null, null, "صوتي", "Sonic" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(55), false, null, null, "مشرح", "Fragmentation" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(57), false, null, null, "كاشف", "Detector" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(58), false, null, null, "مائت", "Inert" },
                    { 6L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(59), false, null, null, "متفجر", "Explosive" },
                    { 7L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(60), false, null, null, "خارق", "Armor-Piercing" },
                    { 8L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(61), false, null, null, "دخاني", "Smoke" },
                    { 9L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(62), false, null, null, "انارة", "Illuminating" },
                    { 10L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(63), false, null, null, "حارق", "Incendiary" },
                    { 11L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(64), false, null, null, "خارق حارق", "Armor-Piercing Incendiary" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "PrimaryPurposes",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(3974), false, null, null, "قتالي", "Combat" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(3993), false, null, null, "تدريبي", "Training" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(3994), false, null, null, "دفاعي", "Defense" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(3996), false, null, null, "استطلاعي", "Reconnaissance" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(3997), false, null, null, "هجومي", "Offensive" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(7623), false, null, null, "قتالي", "Combat" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(7642), false, null, null, "تدريبي", "Training" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(7643), false, null, null, "دفاعي", "Defense" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(7645), false, null, null, "استطلاعي", "Reconnaissance" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(7646), false, null, null, "هجومي", "Offensive" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "ProjectailMaterials",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(5325), false, null, null, "فولاذ", "Steel" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(5335), false, null, null, "نحاس", "Brass" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(5336), false, null, null, "رصاص", "Lead" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(5337), false, null, null, "تنغستن", "Tungsten" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(5339), false, null, null, "يورانيوم منضب", "Depleted Uranium" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(8974), false, null, null, "فولاذ", "Steel" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(8985), false, null, null, "نحاس", "Brass" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(8987), false, null, null, "رصاص", "Lead" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(8988), false, null, null, "تنغستن", "Tungsten" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 79, DateTimeKind.Unspecified).AddTicks(8990), false, null, null, "يورانيوم منضب", "Depleted Uranium" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Propellants",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(6598), false, null, null, "بارود أحادي القاعدة", "Single-base Powder" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(6606), false, null, null, "بارود ثنائي القاعدة", "Double-base Powder" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(6607), false, null, null, "بارود ثلاثي القاعدة", "Triple-base Powder" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(6609), false, null, null, "نيتروسليلوز", "Nitrocellulose" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(6610), false, null, null, "كورديت", "Cordite" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(237), false, null, null, "بارود أحادي القاعدة", "Single-base Powder" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(246), false, null, null, "بارود ثنائي القاعدة", "Double-base Powder" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(247), false, null, null, "بارود ثلاثي القاعدة", "Triple-base Powder" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(249), false, null, null, "نيتروسليلوز", "Nitrocellulose" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(250), false, null, null, "كورديت", "Cordite" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7872), false, null, null, "عقيد", "Colonel" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7880), false, null, null, "مقدم", "Lieutenant Colonel" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7881), false, null, null, "رائد", "Major" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7883), false, null, null, "نقيب", "Captain" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7884), false, null, null, "ملازم أول", "First Lieutenant" },
                    { 6L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7885), false, null, null, "ملازم", "Second Lieutenant" },
                    { 7L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7886), false, null, null, "رقيب أول", "Master Sergeant" },
                    { 8L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7887), false, null, null, "رقيب", "Sergeant" },
                    { 9L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7888), false, null, null, "عريف", "Corporal" },
                    { 10L, null, new DateTime(2025, 12, 8, 15, 6, 22, 523, DateTimeKind.Unspecified).AddTicks(7889), false, null, null, "جندي", "Private" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1502), false, null, null, "عقيد", "Colonel" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1511), false, null, null, "مقدم", "Lieutenant Colonel" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1513), false, null, null, "رائد", "Major" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1515), false, null, null, "نقيب", "Captain" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1516), false, null, null, "ملازم أول", "First Lieutenant" },
                    { 6L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1517), false, null, null, "ملازم", "Second Lieutenant" },
                    { 7L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1518), false, null, null, "رقيب أول", "Master Sergeant" },
                    { 8L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1519), false, null, null, "رقيب", "Sergeant" },
                    { 9L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1520), false, null, null, "عريف", "Corporal" },
                    { 10L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(1521), false, null, null, "جندي", "Private" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "RequestPurposes",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn", "RequestType" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1523), null, null, false, null, null, "طلب عادي", "Normal Order", 1 },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1543), null, null, false, null, null, "طلب خدمة", "Duty Order", 1 },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1545), null, null, false, null, null, "طلب عملية", "Operation Order", 1 },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1546), null, null, false, null, null, "طلب تدريبي", "Training Order", 1 },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1549), null, null, false, null, null, "إرجاع عادي", "Normal Return", 2 },
                    { 6L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1550), null, null, false, null, null, "إرجاع بعد انتهاء الخدمة", "Return After Service", 2 },
                    { 7L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1551), null, null, false, null, null, "إرجاع بعد العملية", "Return After Operation", 2 },
                    { 8L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1552), null, null, false, null, null, "إرجاع بعد التدريب", "Return After Training", 2 },
                    { 9L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1553), null, null, false, null, null, "تسديد تالف", "Damaged Discard", 3 },
                    { 10L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1554), null, null, false, null, null, "تسديد منتهي الصلاحية", "Expired Discard", 3 },
                    { 11L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1555), null, null, false, null, null, "تسديد غير مستخدم", "Unused Discard", 3 },
                    { 12L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(1547), null, null, false, null, null, "تطهير الميدان", "Field Clearance", 1 }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4867), null, null, false, null, null, "طلب عادي", "Normal Order", 1 },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4884), null, null, false, null, null, "طلب خدمة", "Duty Order", 1 },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4885), null, null, false, null, null, "طلب عملية", "Operation Order", 1 },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4887), null, null, false, null, null, "طلب تدريبي", "Training Order", 1 },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4889), null, null, false, null, null, "إرجاع عادي", "Normal Return", 2 },
                    { 6L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4890), null, null, false, null, null, "إرجاع بعد انتهاء الخدمة", "Return After Service", 2 },
                    { 7L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4891), null, null, false, null, null, "إرجاع بعد العملية", "Return After Operation", 2 },
                    { 8L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4893), null, null, false, null, null, "إرجاع بعد التدريب", "Return After Training", 2 },
                    { 9L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4894), null, null, false, null, null, "تسديد تالف", "Damaged Discard", 3 },
                    { 10L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4895), null, null, false, null, null, "تسديد منتهي الصلاحية", "Expired Discard", 3 },
                    { 11L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4896), null, null, false, null, null, "تسديد غير مستخدم", "Unused Discard", 3 },
                    { 12L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(4888), null, null, false, null, null, "تطهير الميدان", "Field Clearance", 1 }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "Group", "Key", "ModificationDate", "ModifiedBy", "Value" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(3446), "LDAP", "LdapServer", null, null, "10.80.70.3" },
                    { 2, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(3459), "LDAP", "LdapDomain", null, null, "sddev.local" },
                    { 3, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(3461), "LDAP", "LdapEmpAttr", null, null, "sAMAccountName" },
                    { 4, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(3462), "LDAP", "LdapUsername", null, null, "1000" },
                    { 5, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(3463), "LDAP", "LdapPassword", null, null, "Qatar@2025" }
========
                    { 1, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(6747), "LDAP", "LdapServer", null, null, "10.80.70.3" },
                    { 2, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(6762), "LDAP", "LdapDomain", null, null, "sddev.local" },
                    { 3, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(6764), "LDAP", "LdapEmpAttr", null, null, "sAMAccountName" },
                    { 4, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(6765), "LDAP", "LdapUsername", null, null, "1000" },
                    { 5, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(6767), "LDAP", "LdapPassword", null, null, "Qatar@2025" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(4717), false, null, null, "شركة الإمدادات العسكرية المتقدمة", "Advanced Military Supplies Co." },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(4726), false, null, null, "المؤسسة العامة للتسليح", "General Armament Corporation" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(4728), false, null, null, "شركة الصناعات الدفاعية", "Defense Industries Company" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(4729), false, null, null, "مجموعة التجهيزات العسكرية", "Military Equipment Group" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 6, 22, 524, DateTimeKind.Unspecified).AddTicks(4730), false, null, null, "شركة التوريدات الاستراتيجية", "Strategic Supplies Corporation" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(8011), false, null, null, "شركة الإمدادات العسكرية المتقدمة", "Advanced Military Supplies Co." },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(8020), false, null, null, "المؤسسة العامة للتسليح", "General Armament Corporation" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(8022), false, null, null, "شركة الصناعات الدفاعية", "Defense Industries Company" },
                    { 4L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(8023), false, null, null, "مجموعة التجهيزات العسكرية", "Military Equipment Group" },
                    { 5L, null, new DateTime(2025, 12, 8, 15, 28, 57, 80, DateTimeKind.Unspecified).AddTicks(8024), false, null, null, "شركة التوريدات الاستراتيجية", "Strategic Supplies Corporation" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 525, DateTimeKind.Unspecified).AddTicks(528), false, null, null, "غرام", "Gram" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 525, DateTimeKind.Unspecified).AddTicks(545), false, null, null, "مليمتر", "Millimeter" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 525, DateTimeKind.Unspecified).AddTicks(547), false, null, null, "قطعة", "Piece" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 81, DateTimeKind.Unspecified).AddTicks(3640), false, null, null, "غرام", "Gram" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 81, DateTimeKind.Unspecified).AddTicks(3658), false, null, null, "مليمتر", "Millimeter" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 81, DateTimeKind.Unspecified).AddTicks(3660), false, null, null, "قطعة", "Piece" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "WorkFlowType",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
<<<<<<<< HEAD:Project.EntityFramework/Migrations/20251208120623_Initial.cs
                    { 1L, null, new DateTime(2025, 12, 8, 15, 6, 22, 526, DateTimeKind.Unspecified).AddTicks(7920), false, null, null, "Request", "Request" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 6, 22, 526, DateTimeKind.Unspecified).AddTicks(7944), false, null, null, "Discard", "Discard" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 6, 22, 526, DateTimeKind.Unspecified).AddTicks(7946), false, null, null, "Return", "Return" }
========
                    { 1L, null, new DateTime(2025, 12, 8, 15, 28, 57, 82, DateTimeKind.Unspecified).AddTicks(7301), false, null, null, "Request", "Request" },
                    { 2L, null, new DateTime(2025, 12, 8, 15, 28, 57, 82, DateTimeKind.Unspecified).AddTicks(7321), false, null, null, "Discard", "Discard" },
                    { 3L, null, new DateTime(2025, 12, 8, 15, 28, 57, 82, DateTimeKind.Unspecified).AddTicks(7374), false, null, null, "Return", "Return" }
>>>>>>>> main:Project.EntityFramework/Migrations/20251208122857_init.cs
                });

            migrationBuilder.InsertData(
                table: "Ammunitions",
                columns: new[] { "Id", "AmmunitionType", "ArmNumber", "BulletDiameter", "BulletDiameterUnitId", "CaseTypeId", "CompatibilityId", "HazardDivisionId", "IsLinked", "NatureOptionId", "PrimaryPurposId", "Primer", "ProjectailMaterialId", "ProjectileColorId", "PropellantId", "TotalWeight" },
                values: new object[,]
                {
                    { 1L, 1, null, 5.56m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Boxer", 1L, 1L, 1L, 12.0m },
                    { 2L, 1, null, 7.62m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Berdan", 2L, 2L, 2L, 24.0m },
                    { 3L, 1, null, 9.0m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 7.5m },
                    { 4L, 1, null, 12.7m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Berdan", 1L, 1L, 1L, 115.0m },
                    { 5L, 1, null, 7.62m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Boxer", 2L, 2L, 2L, 23.0m },
                    { 6L, 1, null, 11.43m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 15.0m },
                    { 7L, 1, null, 12.7m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Berdan", 1L, 1L, 1L, 130.0m },
                    { 8L, 1, null, 5.45m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Berdan", 2L, 2L, 2L, 10.5m },
                    { 9L, 1, null, 10.16m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 11.0m }
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
                name: "IX_BaseItems_ItemNo",
                table: "BaseItems",
                column: "ItemNo",
                unique: true);

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
                name: "IX_Units_NameAr",
                table: "Units",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameEn",
                table: "Units",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Discards");

            migrationBuilder.DropTable(
                name: "EmailConfigurations");

            migrationBuilder.DropTable(
                name: "Explosives");

            migrationBuilder.DropTable(
                name: "FileUplodDetails");

            migrationBuilder.DropTable(
                name: "InventoryDetails");

            migrationBuilder.DropTable(
                name: "NotificationReceivers");

            migrationBuilder.DropTable(
                name: "RequestItems");

            migrationBuilder.DropTable(
                name: "Returns");

            migrationBuilder.DropTable(
                name: "RoleApplicationEntities");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SupplyDetails");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "WorkflowApprovalSteps");

            migrationBuilder.DropTable(
                name: "WorkflowStepApprovalLog");

            migrationBuilder.DropTable(
                name: "WorkflowStepNotifiers");

            migrationBuilder.DropTable(
                name: "WorkFlowType");

            migrationBuilder.DropTable(
                name: "CaseTypes");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Compatibilities");

            migrationBuilder.DropTable(
                name: "HazardDivisions");

            migrationBuilder.DropTable(
                name: "NatureOptions");

            migrationBuilder.DropTable(
                name: "PrimaryPurposes");

            migrationBuilder.DropTable(
                name: "ProjectailMaterials");

            migrationBuilder.DropTable(
                name: "Propellants");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "FileUplodMaster");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "BaseItems");

            migrationBuilder.DropTable(
                name: "WorkflowSteps");

            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ApplicationEntity");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Workflows");

            migrationBuilder.DropTable(
                name: "BaseRequests");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RequestPurposes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Ranks");
        }
    }
}
