﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddingFK_Users_UserTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_UsertypeId",
                table: "Users",
                column: "UsertypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserTypes",
                table: "Users",
                column: "UsertypeId",
                principalTable: "UserTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserTypes",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UsertypeId",
                table: "Users");
        }
    }
}
