using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceProjectWithWebAPI.Migrations
{
    public partial class ParentChildCategoriesModified2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentChildCategories_Categories_ChildCategoryIdId",
                table: "ParentChildCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentChildCategories_Categories_ParentCategoryIdId",
                table: "ParentChildCategories");

            migrationBuilder.DropIndex(
                name: "IX_ParentChildCategories_ChildCategoryIdId",
                table: "ParentChildCategories");

            migrationBuilder.DropIndex(
                name: "IX_ParentChildCategories_ParentCategoryIdId",
                table: "ParentChildCategories");

            migrationBuilder.DropColumn(
                name: "ChildCategoryIdId",
                table: "ParentChildCategories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryIdId",
                table: "ParentChildCategories");

            migrationBuilder.AddColumn<int>(
                name: "ChildCategoryId",
                table: "ParentChildCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "ParentChildCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildCategoryId",
                table: "ParentChildCategories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "ParentChildCategories");

            migrationBuilder.AddColumn<int>(
                name: "ChildCategoryIdId",
                table: "ParentChildCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryIdId",
                table: "ParentChildCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentChildCategories_ChildCategoryIdId",
                table: "ParentChildCategories",
                column: "ChildCategoryIdId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentChildCategories_ParentCategoryIdId",
                table: "ParentChildCategories",
                column: "ParentCategoryIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentChildCategories_Categories_ChildCategoryIdId",
                table: "ParentChildCategories",
                column: "ChildCategoryIdId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentChildCategories_Categories_ParentCategoryIdId",
                table: "ParentChildCategories",
                column: "ParentCategoryIdId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
