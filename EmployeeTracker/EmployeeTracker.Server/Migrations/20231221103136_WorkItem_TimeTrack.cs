using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class WorkItem_TimeTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TimeTrack",
                table: "WorkItems",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeTrack",
                table: "WorkItems");
        }
    }
}
