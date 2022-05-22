using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Link2Me.Migrations
{
    public partial class UpdatedFriend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Friends",
                newName: "UserEmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEmployeeId",
                table: "Friends",
                newName: "UserId");
        }
    }
}
