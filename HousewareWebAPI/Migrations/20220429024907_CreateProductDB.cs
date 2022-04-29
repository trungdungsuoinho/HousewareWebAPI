using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousewareWebAPI.Migrations
{
    public partial class CreateProductDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sort",
                table: "Classifications",
                type: "int",
                nullable: false,
                defaultValue: 2147483647,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 2147483647);

            migrationBuilder.AlterColumn<int>(
                name: "Sort",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 2147483647,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 2147483647);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    View = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Highlights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Design = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Performance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Enable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "Sort",
                table: "Classifications",
                type: "int",
                nullable: true,
                defaultValue: 2147483647,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2147483647);

            migrationBuilder.AlterColumn<int>(
                name: "Sort",
                table: "Categories",
                type: "int",
                nullable: true,
                defaultValue: 2147483647,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2147483647);
        }
    }
}
