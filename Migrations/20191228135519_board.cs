using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessGame.Migrations
{
    public partial class board : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User1Color",
                table: "boards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User2Color",
                table: "boards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User1Color",
                table: "boards");

            migrationBuilder.DropColumn(
                name: "User2Color",
                table: "boards");
        }
    }
}
