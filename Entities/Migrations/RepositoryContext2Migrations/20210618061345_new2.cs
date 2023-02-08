using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PanacealogicsSales.Entities.Migrations.RepositoryContext2Migrations
{
    public partial class new2 : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project2",
                columns: table => new
                {
                    project_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    external_project_id = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    proposal = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project2", x => x.project_id);
                });
        }
    }
}
