using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class updatenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderHistories",
                columns: table => new
                {
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderDetailID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistories", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_OrderHistories_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderHistories_OrderDetails_OrderDetailID",
                        column: x => x.OrderDetailID,
                        principalTable: "OrderDetails",
                        principalColumn: "OrderDetailID");
                    table.ForeignKey(
                        name: "FK_OrderHistories_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_OrderDetailID",
                table: "OrderHistories",
                column: "OrderDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_OrderID",
                table: "OrderHistories",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_UserID",
                table: "OrderHistories",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderHistories");
        }
    }
}
