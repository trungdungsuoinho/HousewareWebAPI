using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousewareWebAPI.Migrations
{
    public partial class ModyfiTimeZoneAndRefSetNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Classifications_ClassificationId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "DATEADD(hh, 7, GETUTCDATE())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "DATEADD(hh, 7, GETUTCDATE())");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Classifications_ClassificationId",
                table: "Categories",
                column: "ClassificationId",
                principalTable: "Classifications",
                principalColumn: "ClassificationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Classifications_ClassificationId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "DATEADD(hh, 7, GETUTCDATE())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "DATEADD(hh, 7, GETUTCDATE())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Classifications_ClassificationId",
                table: "Categories",
                column: "ClassificationId",
                principalTable: "Classifications",
                principalColumn: "ClassificationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
