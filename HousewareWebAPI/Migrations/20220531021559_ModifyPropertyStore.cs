using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousewareWebAPI.Migrations
{
    public partial class ModifyPropertyStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Province",
                table: "Stores",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "District",
                table: "Stores",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Stores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Customers",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Addresses",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Stores");

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Customers",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Addresses",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");
        }
    }
}
