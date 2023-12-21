using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "WorkItems");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "WorkItems",
                newName: "WorkDate");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "WorkItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    TimeTrack = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    EventTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EventKind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBillable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "WorkItems");

            migrationBuilder.RenameColumn(
                name: "WorkDate",
                table: "WorkItems",
                newName: "StartTime");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WorkItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndTime",
                table: "WorkItems",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
