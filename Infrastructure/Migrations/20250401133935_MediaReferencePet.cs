using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MediaReferencePet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MediaId",
                table: "Pets",
                type: "uuid",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Medias_MediaId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_MediaId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Pets");
        }
    }
}
