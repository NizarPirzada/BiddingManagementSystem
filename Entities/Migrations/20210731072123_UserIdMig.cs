using Microsoft.EntityFrameworkCore.Migrations;

namespace PanacealogicsSales.Entities.Migrations
{
    public partial class UserIdMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "proposal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_proposal_user_id",
                table: "proposal",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_proposal_User_user_id",
                table: "proposal",
                column: "user_id",
                principalTable: "User",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_proposal_User_user_id",
                table: "proposal");

            migrationBuilder.DropIndex(
                name: "IX_proposal_user_id",
                table: "proposal");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "proposal");
        }
    }
}
