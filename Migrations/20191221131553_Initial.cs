using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessGame.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "boards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Identifier = table.Column<string>(nullable: true),
                    User1Name = table.Column<string>(nullable: true),
                    User2Identifier = table.Column<string>(nullable: true),
                    User2Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boards", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "boards");
        }
    }
}
