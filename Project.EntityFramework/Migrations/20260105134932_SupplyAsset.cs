using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class SupplyAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Departments_DepartmentId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Employees_CustodianId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_LoginAttempts_AspNetUsers_UserId",
                table: "LoginAttempts");

            migrationBuilder.DropIndex(
                name: "IX_Assets_CustodianId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CustodianId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Assets",
                newName: "CurrentAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Assets_DepartmentId",
                table: "Assets",
                newName: "IX_Assets_CurrentAssignmentId");

            migrationBuilder.AddColumn<DateTime>(
                name: "SupplyDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LoginType",
                table: "LoginAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigned",
                table: "Assets",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                    CustodianId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
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
                name: "AssetAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    AssetSupplyId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    CustodianId = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_AssetAssignments_AssetSupplies_AssetSupplyId",
                        column: x => x.AssetSupplyId,
                        principalTable: "AssetSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Employees_CustodianId",
                        column: x => x.CustodianId,
                        principalTable: "Employees",
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
                    PreviousCustodianId = table.Column<long>(type: "bigint", nullable: true),
                    NewCustodianId = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_AssetHistory_Employees_NewCustodianId",
                        column: x => x.NewCustodianId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_Employees_PreviousCustodianId",
                        column: x => x.PreviousCustodianId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetHistory_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5835));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5885));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5887));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5888));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1329));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1373));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1375));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1378));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1379));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5637));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5662));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5664));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5666));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5668));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5669));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5671));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5723));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5725));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5727));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5748));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5803));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5808));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5810));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5813));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5815));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(7798));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(7816));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(7825));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(7827));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8869));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8896));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8898));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2865));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2892));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2895));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2896));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2898));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4615));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4627));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4629));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4632));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4634));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4635));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4637));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4638));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4640));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4457));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4488));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4490));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4492));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4494));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6222));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6236));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6239));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6241));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6242));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8019));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8035));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8039));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8041));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9615));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9640));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3960));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3985));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3986));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3988));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3991));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3993));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3994));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3996));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3997));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3999));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(4000));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3990));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6401));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6423));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6425));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6426));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8208));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8225));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8227));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8228));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8230));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 595, DateTimeKind.Unspecified).AddTicks(5221));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 595, DateTimeKind.Unspecified).AddTicks(5246));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 595, DateTimeKind.Unspecified).AddTicks(5248));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 597, DateTimeKind.Unspecified).AddTicks(3930));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 597, DateTimeKind.Unspecified).AddTicks(3960));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 597, DateTimeKind.Unspecified).AddTicks(3962));

            migrationBuilder.CreateIndex(
                name: "IX_Assets_IsAssigned",
                table: "Assets",
                column: "IsAssigned");

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
                name: "IX_AssetSupplyDetails_ItemId",
                table: "AssetSupplyDetails",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AssetAssignments_CurrentAssignmentId",
                table: "Assets",
                column: "CurrentAssignmentId",
                principalTable: "AssetAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoginAttempts_AspNetUsers_UserId",
                table: "LoginAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AssetAssignments_CurrentAssignmentId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_LoginAttempts_AspNetUsers_UserId",
                table: "LoginAttempts");

            migrationBuilder.DropTable(
                name: "AssetHistory");

            migrationBuilder.DropTable(
                name: "AssetSupplyDetails");

            migrationBuilder.DropTable(
                name: "AssetAssignments");

            migrationBuilder.DropTable(
                name: "AssetSupplies");

            migrationBuilder.DropIndex(
                name: "IX_Assets_IsAssigned",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "SupplyDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsAssigned",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "CurrentAssignmentId",
                table: "Assets",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Assets_CurrentAssignmentId",
                table: "Assets",
                newName: "IX_Assets_DepartmentId");

            migrationBuilder.AlterColumn<string>(
                name: "LoginType",
                table: "LoginAttempts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "CustodianId",
                table: "Assets",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Assets",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3016));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3040));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3041));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3043));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7211));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7228));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7230));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7231));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7232));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(420));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(504));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(506));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(508));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(509));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(510));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(512));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(513));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(514));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(516));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(517));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(518));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(519));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(521));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(522));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(523));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(525));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(526));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(2182));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(2193));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(2195));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(2197));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(877));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(899));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(902));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(903));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2326));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2351));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3770));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3779));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3780));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3782));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3783));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3784));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3785));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3788));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3789));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3790));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2584));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2609));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2611));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2612));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2614));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4530));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4541));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4542));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4544));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4545));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6038));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6047));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6049));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6050));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6051));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7438));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7450));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7453));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7454));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7455));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7456));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7457));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1211));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1231));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1232));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1234));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1236));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1239));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1240));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1241));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1242));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1243));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1235));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3329));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3347));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3350));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3351));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5073));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5086));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5088));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5089));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5090));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 978, DateTimeKind.Unspecified).AddTicks(1574));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 978, DateTimeKind.Unspecified).AddTicks(1593));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 978, DateTimeKind.Unspecified).AddTicks(1595));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 979, DateTimeKind.Unspecified).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 979, DateTimeKind.Unspecified).AddTicks(6008));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 979, DateTimeKind.Unspecified).AddTicks(6010));

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CustodianId",
                table: "Assets",
                column: "CustodianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Departments_DepartmentId",
                table: "Assets",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Employees_CustodianId",
                table: "Assets",
                column: "CustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoginAttempts_AspNetUsers_UserId",
                table: "LoginAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
