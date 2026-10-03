using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eGift.WebAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMyCartUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyCart",
                table: "MyCart");

            migrationBuilder.RenameTable(
                name: "MyCart",
                newName: "MyCarts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyCarts",
                table: "MyCarts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyCarts",
                table: "MyCarts");

            migrationBuilder.RenameTable(
                name: "MyCarts",
                newName: "MyCart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyCart",
                table: "MyCart",
                column: "Id");
        }
    }
}
