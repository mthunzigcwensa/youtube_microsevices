using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace youtube.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicLocalPath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicLocalPath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicUrl",
                table: "AspNetUsers");
        }
    }
}
