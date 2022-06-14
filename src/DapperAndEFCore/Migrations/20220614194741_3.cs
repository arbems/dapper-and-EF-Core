using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dapper_NET_6.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Posts_PostId",
                table: "Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Details",
                table: "Details");

            migrationBuilder.RenameTable(
                name: "Details",
                newName: "PostDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostDetails",
                table: "PostDetails",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDetails_Posts_PostId",
                table: "PostDetails",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDetails_Posts_PostId",
                table: "PostDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostDetails",
                table: "PostDetails");

            migrationBuilder.RenameTable(
                name: "PostDetails",
                newName: "Details");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Details",
                table: "Details",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Posts_PostId",
                table: "Details",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
