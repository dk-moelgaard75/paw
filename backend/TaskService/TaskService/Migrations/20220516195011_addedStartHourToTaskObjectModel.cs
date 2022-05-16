using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskService.Migrations
{
    public partial class addedStartHourToTaskObjectModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskObjectId",
                table: "TaskXEmployee",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartHour",
                table: "TaskObjs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskXEmployee_TaskObjectId",
                table: "TaskXEmployee",
                column: "TaskObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskXEmployee_TaskObjs_TaskObjectId",
                table: "TaskXEmployee",
                column: "TaskObjectId",
                principalTable: "TaskObjs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskXEmployee_TaskObjs_TaskObjectId",
                table: "TaskXEmployee");

            migrationBuilder.DropIndex(
                name: "IX_TaskXEmployee_TaskObjectId",
                table: "TaskXEmployee");

            migrationBuilder.DropColumn(
                name: "TaskObjectId",
                table: "TaskXEmployee");

            migrationBuilder.DropColumn(
                name: "StartHour",
                table: "TaskObjs");
        }
    }
}
