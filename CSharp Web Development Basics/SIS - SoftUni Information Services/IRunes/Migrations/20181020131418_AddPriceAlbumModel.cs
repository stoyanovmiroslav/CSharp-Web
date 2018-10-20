using Microsoft.EntityFrameworkCore.Migrations;

namespace IRunes.Migrations
{
    public partial class AddPriceAlbumModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Albums");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Albums",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
