using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InputWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddHashToRecording : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "hash",
                table: "recordings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hash",
                table: "recordings");
        }
    }
}
