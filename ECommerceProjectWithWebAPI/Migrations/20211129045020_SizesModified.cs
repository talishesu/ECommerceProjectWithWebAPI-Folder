using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceProjectWithWebAPI.Migrations
{
    public partial class SizesModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sizes",
                newName: "SmallName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SmallName",
                table: "Sizes",
                newName: "Description");
        }
    }
}
