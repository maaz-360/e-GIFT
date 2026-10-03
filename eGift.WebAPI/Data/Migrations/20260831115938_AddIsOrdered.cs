using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eGift.WebAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOrdered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOrdered",
                table: "MyCarts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrdered",
                table: "MyCarts");
        }
    }
}
