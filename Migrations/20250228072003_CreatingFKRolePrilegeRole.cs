using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class CreatingFKRolePrilegeRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RolePriveleges_RoleId",
                table: "RolePriveleges",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePriveleges_Roles",
                table: "RolePriveleges",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePriveleges_Roles",
                table: "RolePriveleges");

            migrationBuilder.DropIndex(
                name: "IX_RolePriveleges_RoleId",
                table: "RolePriveleges");
        }
    }
}
