using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskService.Migrations
{
    public partial class updateddbmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "Employee",
                table: "TaskObjs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employee",
                table: "TaskObjs");

            migrationBuilder.AddColumn<int>(
                name: "TaskObjectId",
                table: "TaskXEmployee",
                type: "integer",
                nullable: true);

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
    }
}
