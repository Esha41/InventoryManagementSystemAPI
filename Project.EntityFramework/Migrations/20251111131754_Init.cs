using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                name: "Hcc",
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
                    table.PrimaryKey("PK_Hcc", x => x.Id);
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationEntityId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleApplicationEntities", x => x.Id);
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
                name: "BaseItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Nsn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HccId = table.Column<long>(type: "bigint", nullable: false),
                    PartNo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReadyForIssue = table.Column<bool>(type: "bit", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_BaseItems_Hcc_HccId",
                        column: x => x.HccId,
                        principalTable: "Hcc",
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
                    ApplicationEntityId = table.Column<int>(type: "int", nullable: false),
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
                    BulletDiameter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BulletDiameterUnitId = table.Column<long>(type: "bigint", nullable: false),
                    CaseLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CaseLengthUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IsLinked = table.Column<bool>(type: "bit", nullable: false),
                    Primer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NatureOptionId = table.Column<long>(type: "bigint", nullable: true),
                    PrimaryPurposId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectileColorId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectailMaterialId = table.Column<long>(type: "bigint", nullable: true),
                    CaseTypeId = table.Column<long>(type: "bigint", nullable: false),
                    PropellantId = table.Column<long>(type: "bigint", nullable: false),
                    CompatibilityId = table.Column<long>(type: "bigint", nullable: false),
                    HazardDivisionId = table.Column<long>(type: "bigint", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Ammunitions_Units_CaseLengthUnitId",
                        column: x => x.CaseLengthUnitId,
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
                    ItemQuantity = table.Column<long>(type: "bigint", nullable: false)
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
                    RecieverId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DepotId = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_BaseRequests_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_BaseRequests_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
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
                    ApproverEmployeeId = table.Column<int>(type: "int", nullable: true),
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
                    OldRequestStatus = table.Column<int>(type: "int", nullable: false),
                    NewRequestStatus = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    WorkflowStepId = table.Column<int>(type: "int", nullable: true),
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
                    UsageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    UsagePurpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnualDiscard = table.Column<int>(type: "int", nullable: true),
                    UsageLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.InsertData(
                table: "ApplicationEntity",
                columns: new[] { "Id", "Code", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, "ORE", null, new DateTime(2025, 11, 11, 16, 17, 53, 842, DateTimeKind.Unspecified).AddTicks(8359), false, null, null, "الجهة الطالبة للطلب", "Order Requesting Entity" },
                    { 2L, "MT", null, new DateTime(2025, 11, 11, 16, 17, 53, 842, DateTimeKind.Unspecified).AddTicks(8376), false, null, null, "التدريب العسكري", "Military Training" },
                    { 3L, "DoA", null, new DateTime(2025, 11, 11, 16, 17, 53, 842, DateTimeKind.Unspecified).AddTicks(8378), false, null, null, "مدير التسليح", "Director of Armament" },
                    { 4L, "MO", null, new DateTime(2025, 11, 11, 16, 17, 53, 842, DateTimeKind.Unspecified).AddTicks(8379), false, null, null, "العمليات العسكرية", "Military Operations" },
                    { 5L, "DCoS", null, new DateTime(2025, 11, 11, 16, 17, 53, 842, DateTimeKind.Unspecified).AddTicks(8381), false, null, null, "نائب رئيس الأركان", "Deputy Chief of Staff" },
                    { 6L, "Function", null, new DateTime(2025, 11, 11, 16, 17, 53, 842, DateTimeKind.Unspecified).AddTicks(8382), false, null, null, "وظيفة", "Function" },
                    { 7L, "CoS", null, new DateTime(2025, 11, 11, 16, 17, 53, 842, DateTimeKind.Unspecified).AddTicks(8383), false, null, null, "رئيس الأركان", "Chief of Staff" },
                    { 8L, "Inventory", null, new DateTime(2025, 11, 11, 16, 17, 53, 842, DateTimeKind.Unspecified).AddTicks(8385), false, null, null, "المخزون", "Inventory" }
                });

            migrationBuilder.InsertData(
                table: "CaseTypes",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 843, DateTimeKind.Unspecified).AddTicks(8798), false, null, null, "نحاسي", "Brass" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 843, DateTimeKind.Unspecified).AddTicks(8821), false, null, null, "فولاذي", "Steel" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 843, DateTimeKind.Unspecified).AddTicks(8822), false, null, null, "ألومنيوم", "Aluminum" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 843, DateTimeKind.Unspecified).AddTicks(8823), false, null, null, "بلاستيك", "Plastic" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 843, DateTimeKind.Unspecified).AddTicks(8825), false, null, null, "مختلط", "Composite" }
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
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(1743), false, null, null, "المجموعة أ", "Group A" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(1760), false, null, null, "المجموعة ب", "Group B" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(1762), false, null, null, "المجموعة ج", "Group C" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(1763), false, null, null, "المجموعة د", "Group D" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(1765), false, null, null, "المجموعة هـ", "Group E" }
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
                    { 1L, "LOG", null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(5203), false, null, null, "قسم اللوجستيات", "Logistics Department" },
                    { 2L, "OPS", null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(5221), false, null, null, "قسم العمليات", "Operations Department" },
                    { 3L, "INV", null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(5223), false, null, null, "قسم المخزون", "Inventory Department" },
                    { 4L, "ARM", null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(5224), false, null, null, "قسم التسليح", "Armament Department" },
                    { 5L, "MNT", null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(5226), false, null, null, "قسم الصيانة", "Maintenance Department" }
                });

            migrationBuilder.InsertData(
                table: "Depots",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "Latitude", "Location", "Longitude", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(6672), false, 25.2854m, "Doha", 51.5310m, null, null, "مستودع الدوحة المركزي", "Doha Central Depot" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(6685), false, 25.2866m, "Al Rayyan", 51.4244m, null, null, "مستودع الريان الغربي", "Al Rayyan West Depot" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(6687), false, 25.6800m, "Al Khor", 51.5059m, null, null, "مستودع الخور الشمالي", "Al Khor North Depot" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(6689), false, 25.1657m, "Al Wakrah", 51.6034m, null, null, "مستودع الوكرة الجنوبي", "Al Wakrah South Depot" }
                });

            migrationBuilder.InsertData(
                table: "HazardDivisions",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(9074), false, null, null, "القسم 1.1 - مواد متفجرة", "Division 1.1 - Explosives" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(9088), false, null, null, "القسم 1.2 - مواد قابلة للانفجار", "Division 1.2 - Projection Hazard" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(9090), false, null, null, "القسم 1.3 - مواد قابلة للاشتعال", "Division 1.3 - Fire Hazard" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(9092), false, null, null, "القسم 1.4 - مواد منخفضة المخاطر", "Division 1.4 - Minor Hazard" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 844, DateTimeKind.Unspecified).AddTicks(9093), false, null, null, "القسم 1.5 - مواد غير حساسة", "Division 1.5 - Very Insensitive" }
                });

            migrationBuilder.InsertData(
                table: "Hcc",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(415), false, null, null, "HCC-A1", "HCC-A1" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(426), false, null, null, "HCC-B2", "HCC-B2" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(427), false, null, null, "HCC-C3", "HCC-C3" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(428), false, null, null, "HCC-D4", "HCC-D4" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(430), false, null, null, "HCC-E5", "HCC-E5" }
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(7464), false, null, null, "مصنع الذخائر الملكي", "Royal Ordnance Factory" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(7485), false, null, null, "شركة رايثيون", "Raytheon Company" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(7487), false, null, null, "مؤسسة الصناعات العسكرية الوطنية", "National Military Industries" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(7488), false, null, null, "شركة لوكهيد مارتن", "Lockheed Martin" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(7489), false, null, null, "مجموعة بي إيه إي سيستمز", "BAE Systems" }
                });

            migrationBuilder.InsertData(
                table: "NatureOptions",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(9033), false, null, null, "قتالية", "Combat" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(9044), false, null, null, "تدريبية", "Training" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(9046), false, null, null, "تعليمية", "Educational" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(9048), false, null, null, "وهمية", "Dummy" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 845, DateTimeKind.Unspecified).AddTicks(9049), false, null, null, "عرض", "Display" }
                });

            migrationBuilder.InsertData(
                table: "PrimaryPurposes",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(7155), false, null, null, "قتالي", "Combat" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(7180), false, null, null, "تدريبي", "Training" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(7182), false, null, null, "دفاعي", "Defense" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(7183), false, null, null, "استطلاعي", "Reconnaissance" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(7184), false, null, null, "هجومي", "Offensive" }
                });

            migrationBuilder.InsertData(
                table: "ProjectailMaterials",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(8637), false, null, null, "فولاذ", "Steel" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(8646), false, null, null, "نحاس", "Brass" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(8648), false, null, null, "رصاص", "Lead" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(8650), false, null, null, "تنغستن", "Tungsten" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(8651), false, null, null, "يورانيوم منضب", "Depleted Uranium" }
                });

            migrationBuilder.InsertData(
                table: "Propellants",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(9967), false, null, null, "بارود أحادي القاعدة", "Single-base Powder" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(9976), false, null, null, "بارود ثنائي القاعدة", "Double-base Powder" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(9978), false, null, null, "بارود ثلاثي القاعدة", "Triple-base Powder" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(9979), false, null, null, "نيتروسليلوز", "Nitrocellulose" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 846, DateTimeKind.Unspecified).AddTicks(9981), false, null, null, "كورديت", "Cordite" }
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1398), false, null, null, "عقيد", "Colonel" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1408), false, null, null, "مقدم", "Lieutenant Colonel" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1410), false, null, null, "رائد", "Major" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1411), false, null, null, "نقيب", "Captain" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1412), false, null, null, "ملازم أول", "First Lieutenant" },
                    { 6L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1414), false, null, null, "ملازم", "Second Lieutenant" },
                    { 7L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1415), false, null, null, "رقيب أول", "Master Sergeant" },
                    { 8L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1416), false, null, null, "رقيب", "Sergeant" },
                    { 9L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1417), false, null, null, "عريف", "Corporal" },
                    { 10L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(1418), false, null, null, "جندي", "Private" }
                });

            migrationBuilder.InsertData(
                table: "RequestPurposes",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn", "RequestType" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5101), null, null, false, null, null, "طلب عادي", "Normal Order", 1 },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5122), null, null, false, null, null, "طلب خدمة", "Duty Order", 1 },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5124), null, null, false, null, null, "طلب عملية", "Operation Order", 1 },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5125), null, null, false, null, null, "طلب تدريبي", "Training Order", 1 },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5176), null, null, false, null, null, "إرجاع عادي", "Normal Return", 2 },
                    { 6L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5178), null, null, false, null, null, "إرجاع بعد انتهاء الخدمة", "Return After Service", 2 },
                    { 7L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5179), null, null, false, null, null, "إرجاع بعد العملية", "Return After Operation", 2 },
                    { 8L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5180), null, null, false, null, null, "إرجاع بعد التدريب", "Return After Training", 2 },
                    { 9L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5182), null, null, false, null, null, "تسديد تالف", "Damaged Discard", 3 },
                    { 10L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5183), null, null, false, null, null, "تسديد منتهي الصلاحية", "Expired Discard", 3 },
                    { 11L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(5184), null, null, false, null, null, "تسديد غير مستخدم", "Unused Discard", 3 }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(7611), false, null, null, "شركة الإمدادات العسكرية المتقدمة", "Advanced Military Supplies Co." },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(7625), false, null, null, "المؤسسة العامة للتسليح", "General Armament Corporation" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(7627), false, null, null, "شركة الصناعات الدفاعية", "Defense Industries Company" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(7658), false, null, null, "مجموعة التجهيزات العسكرية", "Military Equipment Group" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(7660), false, null, null, "شركة التوريدات الاستراتيجية", "Strategic Supplies Corporation" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(8947), false, null, null, "قطعة", "Piece" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(8956), false, null, null, "صندوق", "Box" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(8958), false, null, null, "طن", "Ton" },
                    { 4L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(8959), false, null, null, "كيلوغرام", "Kilogram" },
                    { 5L, null, new DateTime(2025, 11, 11, 16, 17, 53, 847, DateTimeKind.Unspecified).AddTicks(8960), false, null, null, "حاوية", "Container" }
                });

            migrationBuilder.InsertData(
                table: "WorkFlowType",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 11, 11, 16, 17, 53, 849, DateTimeKind.Unspecified).AddTicks(1270), false, null, null, "Request", "Request" },
                    { 2L, null, new DateTime(2025, 11, 11, 16, 17, 53, 849, DateTimeKind.Unspecified).AddTicks(1291), false, null, null, "Discard", "Discard" },
                    { 3L, null, new DateTime(2025, 11, 11, 16, 17, 53, 849, DateTimeKind.Unspecified).AddTicks(1293), false, null, null, "Return", "Return" }
                });

            migrationBuilder.InsertData(
                table: "BaseItems",
                columns: new[] { "Id", "BatchNo", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "ExpiryDate", "HccId", "IsDeleted", "ItemNo", "ItemType", "ModificationDate", "ModifiedBy", "Name", "Nsn", "PartNo", "ReadyForIssue" },
                values: new object[,]
                {
                    { 1L, "BATCH-2024-001", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1L, false, "AMM-001", 1, null, null, "5.56x45mm NATO", "1305-01-000-0001", "PN-556-001", true },
                    { 2L, "BATCH-2024-002", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L, false, "AMM-002", 1, null, null, "7.62x51mm NATO", "1305-01-000-0002", "PN-762-001", true },
                    { 3L, "BATCH-2024-003", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L, false, "AMM-003", 1, null, null, "9x19mm Parabellum", "1305-01-000-0003", "PN-9MM-001", true },
                    { 4L, "BATCH-2024-004", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2030, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1L, false, "AMM-004", 1, null, null, ".50 BMG", "1305-01-000-0004", "PN-50BMG-001", true },
                    { 5L, "BATCH-2024-005", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L, false, "AMM-005", 1, null, null, ".308 Winchester", "1305-01-000-0005", "PN-308-001", true },
                    { 6L, "BATCH-2024-006", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L, false, "AMM-006", 1, null, null, ".45 ACP", "1305-01-000-0006", "PN-45ACP-001", true },
                    { 7L, "BATCH-2024-007", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2030, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1L, false, "AMM-007", 1, null, null, "12.7x108mm", "1305-01-000-0007", "PN-127-001", true },
                    { 8L, "BATCH-2024-008", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L, false, "AMM-008", 1, null, null, "5.45x39mm", "1305-01-000-0008", "PN-545-001", true },
                    { 9L, "BATCH-2024-009", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L, false, "AMM-009", 1, null, null, ".40 S&W", "1305-01-000-0009", "PN-40SW-001", true }
                });

            migrationBuilder.InsertData(
                table: "Ammunitions",
                columns: new[] { "Id", "BulletDiameter", "BulletDiameterUnitId", "CaseLength", "CaseLengthUnitId", "CaseTypeId", "CompatibilityId", "HazardDivisionId", "IsLinked", "NatureOptionId", "PrimaryPurposId", "Primer", "ProjectailMaterialId", "ProjectileColorId", "PropellantId", "TotalWeight" },
                values: new object[,]
                {
                    { 1L, 5.56m, 1L, 45.0m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Boxer", 1L, 1L, 1L, 12.0m },
                    { 2L, 7.62m, 2L, 51.0m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Berdan", 2L, 2L, 2L, 24.0m },
                    { 3L, 9.0m, 3L, 19.0m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 7.5m },
                    { 4L, 12.7m, 1L, 99.0m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Berdan", 1L, 1L, 1L, 115.0m },
                    { 5L, 7.62m, 2L, 51.0m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Boxer", 2L, 2L, 2L, 23.0m },
                    { 6L, 11.43m, 3L, 23.0m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 15.0m },
                    { 7L, 12.7m, 1L, 108.0m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Berdan", 1L, 1L, 1L, 130.0m },
                    { 8L, 5.45m, 2L, 39.0m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Berdan", 2L, 2L, 2L, 10.5m },
                    { 9L, 10.16m, 3L, 21.6m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 11.0m }
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
                name: "IX_Ammunitions_CaseLengthUnitId",
                table: "Ammunitions",
                column: "CaseLengthUnitId");

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
                name: "IX_BaseItems_HccId",
                table: "BaseItems",
                column: "HccId");

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
                name: "IX_BaseRequests_DepotId",
                table: "BaseRequests",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseRequests_RecieverId",
                table: "BaseRequests",
                column: "RecieverId");

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
                name: "IX_Hcc_NameAr",
                table: "Hcc",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Hcc_NameEn",
                table: "Hcc",
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
                name: "InventoryDetails");

            migrationBuilder.DropTable(
                name: "NotificationReceivers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "RequestItems");

            migrationBuilder.DropTable(
                name: "Returns");

            migrationBuilder.DropTable(
                name: "RoleApplicationEntities");

            migrationBuilder.DropTable(
                name: "WorkflowApprovalSteps");

            migrationBuilder.DropTable(
                name: "WorkflowStepApprovalLog");

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
                name: "BaseItems");

            migrationBuilder.DropTable(
                name: "BaseRequests");

            migrationBuilder.DropTable(
                name: "WorkflowSteps");

            migrationBuilder.DropTable(
                name: "Hcc");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "RequestPurposes");

            migrationBuilder.DropTable(
                name: "ApplicationEntity");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Workflows");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Ranks");
        }
    }
}
