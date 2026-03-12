using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDepotsItemAssignmentBatchWeaponAndAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_AspNetUsers_CustodianId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_AspNetUsers_NewCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_AspNetUsers_PreviousCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplyDetails_AspNetUsers_CustodianId",
                table: "AssetSupplyDetails");

            migrationBuilder.AddColumn<long>(
                name: "RankId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Employees",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustodianId",
                table: "AssetSupplyDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<long>(
                name: "BatchId",
                table: "Assets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "PreviousCustodianId",
                table: "AssetHistory",
                type: "bigint",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "NewCustodianId",
                table: "AssetHistory",
                type: "bigint",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustodianId",
                table: "AssetAssignments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentTokenId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryType",
                table: "Announcements",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DepotId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Batches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batches_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemDepartmentAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_ItemDepartmentAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDepartmentAssignments_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemDepartmentAssignments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDepots",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepotId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDepots", x => new { x.UserId, x.DepotId });
                    table.ForeignKey(
                        name: "FK_UserDepots_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDepots_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponSupplySelections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    DepotId = table.Column<long>(type: "bigint", nullable: false),
                    BatchId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponSupplySelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponSupplySelections_BaseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeaponSupplySelections_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeaponSupplySelections_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeaponSupplySelections_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RankId",
                table: "Employees",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_BatchId",
                table: "Assets",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_BatchNumber",
                table: "Batches",
                column: "BatchNumber",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_DepotId",
                table: "Batches",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDepartmentAssignments_DepartmentId",
                table: "ItemDepartmentAssignments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDepartmentAssignments_ItemId",
                table: "ItemDepartmentAssignments",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDepartmentAssignments_ItemId_DepartmentId",
                table: "ItemDepartmentAssignments",
                columns: new[] { "ItemId", "DepartmentId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepots_DepotId",
                table: "UserDepots",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepots_UserId",
                table: "UserDepots",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_BatchId",
                table: "WeaponSupplySelections",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_DepotId",
                table: "WeaponSupplySelections",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_ItemId",
                table: "WeaponSupplySelections",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_OrderId",
                table: "WeaponSupplySelections",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_OrderId_DepotId_BatchId_ItemId",
                table: "WeaponSupplySelections",
                columns: new[] { "OrderId", "DepotId", "BatchId", "ItemId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_Employees_CustodianId",
                table: "AssetAssignments",
                column: "CustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_Employees_NewCustodianId",
                table: "AssetHistory",
                column: "NewCustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_Employees_PreviousCustodianId",
                table: "AssetHistory",
                column: "PreviousCustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Batches_BatchId",
                table: "Assets",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetSupplyDetails_Employees_CustodianId",
                table: "AssetSupplyDetails",
                column: "CustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Ranks_RankId",
                table: "Employees",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Employees_CustodianId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_Employees_NewCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_Employees_PreviousCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Batches_BatchId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplyDetails_Employees_CustodianId",
                table: "AssetSupplyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Ranks_RankId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "ItemDepartmentAssignments");

            migrationBuilder.DropTable(
                name: "UserDepots");

            migrationBuilder.DropTable(
                name: "WeaponSupplySelections");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RankId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Assets_BatchId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CurrentTokenId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeliveryType",
                table: "Announcements");

            migrationBuilder.AlterColumn<string>(
                name: "CustodianId",
                table: "AssetSupplyDetails",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PreviousCustodianId",
                table: "AssetHistory",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewCustodianId",
                table: "AssetHistory",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustodianId",
                table: "AssetAssignments",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_AspNetUsers_CustodianId",
                table: "AssetAssignments",
                column: "CustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_AspNetUsers_NewCustodianId",
                table: "AssetHistory",
                column: "NewCustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_AspNetUsers_PreviousCustodianId",
                table: "AssetHistory",
                column: "PreviousCustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetSupplyDetails_AspNetUsers_CustodianId",
                table: "AssetSupplyDetails",
                column: "CustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
