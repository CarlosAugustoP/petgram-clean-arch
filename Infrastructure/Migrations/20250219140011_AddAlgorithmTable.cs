using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlgorithmTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DogMeter = table.Column<int>(type: "integer", nullable: false),
                    CatMeter = table.Column<int>(type: "integer", nullable: false),
                    BirdMeter = table.Column<int>(type: "integer", nullable: false),
                    FishMeter = table.Column<int>(type: "integer", nullable: false),
                    ReptileMeter = table.Column<int>(type: "integer", nullable: false),
                    RodentMeter = table.Column<int>(type: "integer", nullable: false),
                    InsectMeter = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preference_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Preference_UserId",
                table: "Preference",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preference");
        }
    }
}
