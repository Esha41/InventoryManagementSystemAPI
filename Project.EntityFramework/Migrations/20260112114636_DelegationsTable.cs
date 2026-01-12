using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class DelegationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6147));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6150));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6152));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6154));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(579));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(606));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(610));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(612));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(614));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3951));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3972));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3975));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3977));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3979));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3980));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3982));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3984));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3986));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3987));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3995));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4020));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4024));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4026));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4027));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4029));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4031));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4032));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(5739));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(5759));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(5764));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(5766));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5119));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5143));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5146));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5148));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5150));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7934));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7958));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7961));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7963));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7965));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9580));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9581));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9583));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9584));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9086));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9112));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9115));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9117));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(781));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(799));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(801));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(803));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(805));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2263));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2277));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2279));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2282));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3739));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3754));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3756));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3758));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3761));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3762));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3764));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3765));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3767));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3768));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7813));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7837));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7839));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7841));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7844));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7846));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7847));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7849));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7851));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7852));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7854));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1310));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1324));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1349));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1351));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1353));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(7744));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(7768));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 990, DateTimeKind.Unspecified).AddTicks(5633));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 990, DateTimeKind.Unspecified).AddTicks(5657));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 990, DateTimeKind.Unspecified).AddTicks(5660));

            migrationBuilder.CreateIndex(
                name: "IX_UserDelegations_DelegateeUserId",
                table: "UserDelegations",
                column: "DelegateeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDelegations_DelegatorUserId",
                table: "UserDelegations",
                column: "DelegatorUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDelegations");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9852));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9854));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9856));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4390));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4411));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4413));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7648));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7666));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7668));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7669));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7671));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7672));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7674));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7703));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7705));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7707));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7708));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7709));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7711));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7712));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7713));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7714));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7716));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7717));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(9360));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(9376));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(9378));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(9380));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8433));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8456));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8458));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8459));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8461));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1563));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1587));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1589));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1590));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1591));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3150));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3162));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3164));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3167));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3168));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3169));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3170));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3171));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3173));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3174));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1494));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1516));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1518));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1519));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1521));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3034));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3046));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3047));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3049));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3050));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4499));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4511));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4512));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4514));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5837));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5847));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5848));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5850));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5852));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5853));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5855));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5856));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5857));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9541));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9564));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1509));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1524));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1526));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1528));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1529));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2869));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2880));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2881));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2883));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2884));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(8827));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(8847));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(8849));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 411, DateTimeKind.Unspecified).AddTicks(4198));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 411, DateTimeKind.Unspecified).AddTicks(4220));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 411, DateTimeKind.Unspecified).AddTicks(4222));
        }
    }
}
