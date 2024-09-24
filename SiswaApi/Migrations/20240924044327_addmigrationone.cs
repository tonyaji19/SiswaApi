using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiswaApi.Migrations
{
    public partial class addmigrationone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Siswas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kelas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Umur = table.Column<int>(type: "int", nullable: false),
                    WaliKelas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AsalKota = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siswas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Siswas");
        }
    }
}
