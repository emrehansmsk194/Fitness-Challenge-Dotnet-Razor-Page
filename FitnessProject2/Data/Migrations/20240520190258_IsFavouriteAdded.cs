using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProject2.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsFavouriteAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "Participations",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "Participations");
        }
    }
}
