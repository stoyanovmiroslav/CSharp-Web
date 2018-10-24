using Microsoft.EntityFrameworkCore.Migrations;

namespace MishMash.Migrations
{
    public partial class AddedChanelTagModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Channels_ChannelId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ChannelId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "ChanelTags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false),
                    ChannelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChanelTags", x => new { x.ChannelId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ChanelTags_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChanelTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChanelTags_TagId",
                table: "ChanelTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChanelTags");

            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "Tags",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ChannelId",
                table: "Tags",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Channels_ChannelId",
                table: "Tags",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "ChannelId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
