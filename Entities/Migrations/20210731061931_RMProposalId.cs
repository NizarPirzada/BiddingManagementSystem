using Microsoft.EntityFrameworkCore.Migrations;

namespace PanacealogicsSales.Entities.Migrations
{
    public partial class RMProposalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Project_proposal_proposal_id",
            //    table: "Project");

            //migrationBuilder.DropIndex(
            //    name: "IX_Project_proposal_id",
            //    table: "Project");

            migrationBuilder.DropColumn(
                name: "proposal_id",
                table: "Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "proposal_id",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Project_proposal_id",
                table: "Project",
                column: "proposal_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_proposal_proposal_id",
                table: "Project",
                column: "proposal_id",
                principalTable: "proposal",
                principalColumn: "proposal_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
