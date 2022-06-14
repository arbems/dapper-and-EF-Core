using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dapper_NET_6.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Details",
                table: "Details");

            migrationBuilder.DropIndex(
                name: "IX_Details_PostId",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Details");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Details",
                table: "Details",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Details",
                table: "Details");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Details",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Details",
                table: "Details",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Details_PostId",
                table: "Details",
                column: "PostId",
                unique: true);
        }
    }
}
