using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CalendarService.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CalenderGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeDone = table.Column<int>(type: "integer", nullable: false),
                    TaskDone = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CalendarGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    CalenderGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEmployee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarTaskObj",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CalendarGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StartTime = table.Column<int>(type: "integer", nullable: false),
                    EstimatedHours = table.Column<int>(type: "integer", nullable: false),
                    Employee = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarTaskObj", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "CalendarEmployee");

            migrationBuilder.DropTable(
                name: "CalendarTaskObj");
        }
    }
}
