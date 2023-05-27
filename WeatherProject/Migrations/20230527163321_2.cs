using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherProject.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cloudiness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relativehumidity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Atmosphericpressure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Directionofthewind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temperature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    H = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dewpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Windspeed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weatherconditions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weather");
        }
    }
}
