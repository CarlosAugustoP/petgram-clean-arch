using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MediaReferencePet2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Medias_MediaId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Pets_PetId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PetId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Pets_MediaId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Pets");

            migrationBuilder.CreateTable(
                name: "MediaPet",
                columns: table => new
                {
                    MediasId = table.Column<Guid>(type: "uuid", nullable: false),
                    MentionedPetsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaPet", x => new { x.MediasId, x.MentionedPetsId });
                    table.ForeignKey(
                        name: "FK_MediaPet_Medias_MediasId",
                        column: x => x.MediasId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaPet_Pets_MentionedPetsId",
                        column: x => x.MentionedPetsId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaPet_MentionedPetsId",
                table: "MediaPet",
                column: "MentionedPetsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaPet");

            migrationBuilder.AddColumn<Guid>(
                name: "PetId",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MediaId",
                table: "Pets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PetId",
                table: "Posts",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_MediaId",
                table: "Pets",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Medias_MediaId",
                table: "Pets",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Pets_PetId",
                table: "Posts",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }
    }
}
