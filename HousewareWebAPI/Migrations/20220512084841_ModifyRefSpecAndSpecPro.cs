using Microsoft.EntityFrameworkCore.Migrations;

namespace HousewareWebAPI.Migrations
{
    public partial class ModifyRefSpecAndSpecPro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecification_Products_ProductId",
                table: "ProductSpecification");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecification_Specifications_SpecificationId",
                table: "ProductSpecification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSpecification",
                table: "ProductSpecification");

            migrationBuilder.RenameTable(
                name: "ProductSpecification",
                newName: "ProductSpecifications");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecification_SpecificationId",
                table: "ProductSpecifications",
                newName: "IX_ProductSpecifications_SpecificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSpecifications",
                table: "ProductSpecifications",
                columns: new[] { "ProductId", "SpecificationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecifications_Products_ProductId",
                table: "ProductSpecifications",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecifications_Specifications_SpecificationId",
                table: "ProductSpecifications",
                column: "SpecificationId",
                principalTable: "Specifications",
                principalColumn: "SpecificationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecifications_Products_ProductId",
                table: "ProductSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecifications_Specifications_SpecificationId",
                table: "ProductSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSpecifications",
                table: "ProductSpecifications");

            migrationBuilder.RenameTable(
                name: "ProductSpecifications",
                newName: "ProductSpecification");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecifications_SpecificationId",
                table: "ProductSpecification",
                newName: "IX_ProductSpecification_SpecificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSpecification",
                table: "ProductSpecification",
                columns: new[] { "ProductId", "SpecificationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecification_Products_ProductId",
                table: "ProductSpecification",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecification_Specifications_SpecificationId",
                table: "ProductSpecification",
                column: "SpecificationId",
                principalTable: "Specifications",
                principalColumn: "SpecificationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
