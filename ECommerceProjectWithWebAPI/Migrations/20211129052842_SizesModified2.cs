using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceProjectWithWebAPI.Migrations
{
    public partial class SizesModified2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SmallName",
                table: "Sizes",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sizes",
                newName: "SmallName");
        }
    }
}
