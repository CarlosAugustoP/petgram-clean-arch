using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImproveReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moments_Reports_ReportId",
                table: "Moments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Reports_ReportId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ReportId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Moments_ReportId",
                table: "Moments");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "Moments");

            migrationBuilder.CreateTable(
                name: "MomentReport",
                columns: table => new
                {
                    MomentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MomentReport", x => new { x.MomentsId, x.ReportsId });
                    table.ForeignKey(
                        name: "FK_MomentReport_Moments_MomentsId",
                        column: x => x.MomentsId,
                        principalTable: "Moments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MomentReport_Reports_ReportsId",
                        column: x => x.ReportsId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostReport",
                columns: table => new
                {
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReport", x => new { x.PostsId, x.ReportsId });
                    table.ForeignKey(
                        name: "FK_PostReport_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostReport_Reports_ReportsId",
                        column: x => x.ReportsId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MomentReport_ReportsId",
                table: "MomentReport",
                column: "ReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_ReportsId",
                table: "PostReport",
                column: "ReportsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MomentReport");

            migrationBuilder.DropTable(
                name: "PostReport");

            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "Moments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ReportId",
                table: "Posts",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Moments_ReportId",
                table: "Moments",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Moments_Reports_ReportId",
                table: "Moments",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Reports_ReportId",
                table: "Posts",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");
        }
    }
}
