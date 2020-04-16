using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JWT_API_NETCORE.Migrations
{
    public partial class modify_birthdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Employee",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "BirthDate",
                table: "Employee",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
