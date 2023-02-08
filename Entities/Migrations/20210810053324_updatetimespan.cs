using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PanacealogicsSales.Entities.Migrations
{
    public partial class updatetimespan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "shift_start",
                table: "user_shift",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "shift_end",
                table: "user_shift",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "shift_start",
                table: "user_shift",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<DateTime>(
                name: "shift_end",
                table: "user_shift",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(TimeSpan));
        }
    }
}
