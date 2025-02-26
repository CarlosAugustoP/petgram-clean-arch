using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImproveClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_postId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "postId",
                table: "Likes",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_postId",
                table: "Likes",
                newName: "IX_Likes_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Likes",
                newName: "postId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_PostId",
                table: "Likes",
                newName: "IX_Likes_postId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_postId",
                table: "Likes",
                column: "postId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
