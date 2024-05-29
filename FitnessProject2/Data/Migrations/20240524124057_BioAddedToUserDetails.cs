using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProject2.Data.Migrations
{
    /// <inheritdoc />
    public partial class BioAddedToUserDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "UserDetails");
        }
    }
}
