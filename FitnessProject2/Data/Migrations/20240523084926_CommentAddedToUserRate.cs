using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProject2.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommentAddedToUserRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "UserRates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "UserRates");
        }
    }
}
