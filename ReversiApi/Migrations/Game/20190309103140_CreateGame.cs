using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApi.Migrations.Game
{
    public partial class CreateGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    GameToken = table.Column<string>(nullable: true),
                    PlayerWhiteToken = table.Column<string>(nullable: true),
                    PlayerBlackToken = table.Column<string>(nullable: true),
                    OnSet = table.Column<int>(nullable: false),
                    Winner = table.Column<int>(nullable: true),
                    Board = table.Column<string>(nullable: true),
                    GameStatus = table.Column<string>(nullable : false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
