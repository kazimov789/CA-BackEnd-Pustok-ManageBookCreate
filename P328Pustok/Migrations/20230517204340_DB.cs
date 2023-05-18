using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P328Pustok.Migrations
{
    public partial class DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_TagId",
                table: "Books",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Tags_TagId",
                table: "Books",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Tags_TagId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Books_TagId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Books");
        }
    }
}
