using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceProjectWithWebAPI.Migrations
{
    public partial class ParentChildCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParentChildCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryIdId = table.Column<int>(type: "int", nullable: true),
                    ChildCategoryIdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentChildCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentChildCategories_Categories_ChildCategoryIdId",
                        column: x => x.ChildCategoryIdId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParentChildCategories_Categories_ParentCategoryIdId",
                        column: x => x.ParentCategoryIdId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentChildCategories_ChildCategoryIdId",
                table: "ParentChildCategories",
                column: "ChildCategoryIdId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentChildCategories_ParentCategoryIdId",
                table: "ParentChildCategories",
                column: "ParentCategoryIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentChildCategories");
        }
    }
}
