using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSupplyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Status = table.Column<int>(type: "int", nullable: false),
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
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 881, DateTimeKind.Unspecified).AddTicks(3656));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 881, DateTimeKind.Unspecified).AddTicks(3673));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 881, DateTimeKind.Unspecified).AddTicks(3675));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 881, DateTimeKind.Unspecified).AddTicks(3676));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 881, DateTimeKind.Unspecified).AddTicks(3678));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 881, DateTimeKind.Unspecified).AddTicks(3679));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 881, DateTimeKind.Unspecified).AddTicks(3680));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 881, DateTimeKind.Unspecified).AddTicks(3682));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(2543));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(2565));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(2567));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(2568));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(2570));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(5891));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(5909));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(5911));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(5912));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(5914));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(9554));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 882, DateTimeKind.Unspecified).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(1305));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(1343));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(1345));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(1347));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(3707));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(3725));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(5284));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(5293));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(5295));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(5296));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 883, DateTimeKind.Unspecified).AddTicks(5297));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(3297));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(3320));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(3321));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(3323));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(3324));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(4823));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(4834));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(4836));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(4838));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 884, DateTimeKind.Unspecified).AddTicks(4839));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(3167));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(3188));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(3189));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(3191));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(3192));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(4649));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(4651));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(4652));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(4654));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(6066));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(6075));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(6077));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(6078));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(6079));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7490));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7500));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7503));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7504));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7505));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7506));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7508));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7509));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 885, DateTimeKind.Unspecified).AddTicks(7510));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1220));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1239));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1240));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1242));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1243));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1244));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1245));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1247));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1248));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(1249));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(3431));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(3433));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(3434));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(3436));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(4872));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(4881));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(4883));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(4884));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 886, DateTimeKind.Unspecified).AddTicks(4885));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 890, DateTimeKind.Unspecified).AddTicks(8546));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 890, DateTimeKind.Unspecified).AddTicks(8570));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 890, DateTimeKind.Unspecified).AddTicks(8572));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 890, DateTimeKind.Unspecified).AddTicks(8573));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 890, DateTimeKind.Unspecified).AddTicks(8574));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 892, DateTimeKind.Unspecified).AddTicks(1420));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 892, DateTimeKind.Unspecified).AddTicks(1445));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 11, 26, 19, 892, DateTimeKind.Unspecified).AddTicks(1447));

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplyDetails");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(536));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(554));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(556));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(557));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(559));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(560));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(562));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(563));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(9037));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(9060));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(9062));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(9063));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 100, DateTimeKind.Unspecified).AddTicks(9065));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(2208));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(2209));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(2210));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(5697));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(5713));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(5715));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(5716));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(5718));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(7324));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(7335));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(7404));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(7406));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(9738));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 101, DateTimeKind.Unspecified).AddTicks(9743));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(1314));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(1324));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(1325));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(1328));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(9830));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 102, DateTimeKind.Unspecified).AddTicks(9831));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(1426));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(1435));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(1437));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(1438));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(1439));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(9014));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(9035));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(9037));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(9038));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 103, DateTimeKind.Unspecified).AddTicks(9040));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(422));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(432));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(434));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(435));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(436));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(1799));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(1812));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(1814));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(1815));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(1817));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3074));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3083));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3085));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3086));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3087));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3089));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3090));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3091));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3092));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(3093));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6606));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6624));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6626));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6627));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6628));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6629));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6630));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6632));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6633));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6634));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(6635));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(8578));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(8595));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(8597));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(8598));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(8600));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(9934));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(9943));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(9945));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 104, DateTimeKind.Unspecified).AddTicks(9947));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 105, DateTimeKind.Unspecified).AddTicks(1351));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 105, DateTimeKind.Unspecified).AddTicks(1360));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 105, DateTimeKind.Unspecified).AddTicks(1362));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 105, DateTimeKind.Unspecified).AddTicks(1363));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 105, DateTimeKind.Unspecified).AddTicks(1365));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 106, DateTimeKind.Unspecified).AddTicks(2279));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 106, DateTimeKind.Unspecified).AddTicks(2301));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 17, 16, 44, 56, 106, DateTimeKind.Unspecified).AddTicks(2302));
        }
    }
}
