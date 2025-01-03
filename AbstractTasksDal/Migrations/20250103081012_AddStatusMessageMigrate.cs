using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbstractTasksDal.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusMessageMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusMessage",
                table: "_tasks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusMessage",
                table: "_tasks");
        }
    }
}
