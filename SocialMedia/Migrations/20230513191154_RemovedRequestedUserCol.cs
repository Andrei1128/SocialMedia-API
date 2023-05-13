using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRequestedUserCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRequest_Users_RequestedUserId",
                table: "UserRequest");

            migrationBuilder.DropIndex(
                name: "IX_UserRequest_RequestedUserId",
                table: "UserRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequest_Users_UserId",
                table: "UserRequest",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRequest_Users_UserId",
                table: "UserRequest");

            migrationBuilder.CreateIndex(
                name: "IX_UserRequest_RequestedUserId",
                table: "UserRequest",
                column: "RequestedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequest_Users_RequestedUserId",
                table: "UserRequest",
                column: "RequestedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
