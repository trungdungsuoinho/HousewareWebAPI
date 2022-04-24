using Microsoft.EntityFrameworkCore.Migrations;

namespace HousewareWebAPI.Migrations
{
    public partial class CreateClassificationDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    ClassificationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false, defaultValue: 2147483647),
                    ImageMenu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageBanner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Story = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifications", x => x.ClassificationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classifications");
        }
    }
}
