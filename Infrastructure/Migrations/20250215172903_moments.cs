using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class moments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MomentId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MomentId",
                table: "Likes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Moments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    mediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moments_Medias_mediaId",
                        column: x => x.mediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Moments_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Moments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_MomentId",
                table: "Users",
                column: "MomentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_MomentId",
                table: "Likes",
                column: "MomentId");

            migrationBuilder.CreateIndex(
                name: "IX_Moments_AuthorId",
                table: "Moments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Moments_mediaId",
                table: "Moments",
                column: "mediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Moments_ReportId",
                table: "Moments",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Moments_MomentId",
                table: "Likes",
                column: "MomentId",
                principalTable: "Moments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Moments_MomentId",
                table: "Users",
                column: "MomentId",
                principalTable: "Moments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Moments_MomentId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Moments_MomentId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Moments");

            migrationBuilder.DropIndex(
                name: "IX_Users_MomentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Likes_MomentId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "MomentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MomentId",
                table: "Likes");
        }
    }
}
