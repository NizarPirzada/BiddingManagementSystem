using Microsoft.EntityFrameworkCore.Migrations;

namespace PanacealogicsSales.Entities.Migrations
{
    public partial class RemPropColMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "proposal",
                table: "proposal");

            migrationBuilder.AlterColumn<string>(
                name: "desc",
                table: "Project",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000) CHARACTER SET utf8mb4",
                oldMaxLength: 1000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "proposal",
                table: "proposal",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "desc",
                table: "Project",
                type: "varchar(1000) CHARACTER SET utf8mb4",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 5000,
                oldNullable: true);
        }
    }
}
