using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAppIGTI.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaProfileTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TabProfileTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepoType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepoLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabProfileTest", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TabProfileTest");
        }
    }
}
