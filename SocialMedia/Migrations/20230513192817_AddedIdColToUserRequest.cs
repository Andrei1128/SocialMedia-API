using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdColToUserRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRequest",
                table: "UserRequest");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserRequest",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRequest",
                table: "UserRequest",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRequest_UserId",
                table: "UserRequest",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRequest",
                table: "UserRequest");

            migrationBuilder.DropIndex(
                name: "IX_UserRequest_UserId",
                table: "UserRequest");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserRequest");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRequest",
                table: "UserRequest",
                columns: new[] { "UserId", "RequestedUserId" });
        }
    }
}
