using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.BookStore.Migrations
{
    /// <inheritdoc />
    public partial class Added_PublicationId_To_Book : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PublicationId",
                table: "AppBooks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppBooks_PublicationId",
                table: "AppBooks",
                column: "PublicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppBooks_AppPublications_PublicationId",
                table: "AppBooks",
                column: "PublicationId",
                principalTable: "AppPublications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBooks_AppPublications_PublicationId",
                table: "AppBooks");

            migrationBuilder.DropIndex(
                name: "IX_AppBooks_PublicationId",
                table: "AppBooks");

            migrationBuilder.DropColumn(
                name: "PublicationId",
                table: "AppBooks");
        }
    }
}
