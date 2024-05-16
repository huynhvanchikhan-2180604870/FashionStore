using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class AddModelBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banner_Events_EventID",
                table: "Banner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banner",
                table: "Banner");

            migrationBuilder.RenameTable(
                name: "Banner",
                newName: "Banners");

            migrationBuilder.RenameIndex(
                name: "IX_Banner_EventID",
                table: "Banners",
                newName: "IX_Banners_EventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banners",
                table: "Banners",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Events_EventID",
                table: "Banners",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Events_EventID",
                table: "Banners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banners",
                table: "Banners");

            migrationBuilder.RenameTable(
                name: "Banners",
                newName: "Banner");

            migrationBuilder.RenameIndex(
                name: "IX_Banners_EventID",
                table: "Banner",
                newName: "IX_Banner_EventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banner",
                table: "Banner",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_Events_EventID",
                table: "Banner",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID");
        }
    }
}
