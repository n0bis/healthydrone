using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LandingPoints.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Landingpoints",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    callsign = table.Column<string>(maxLength: 10, nullable: false),
                    description = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    address = table.Column<string>(nullable: false),
                    type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landingpoints", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Landingpoints",
                columns: new[] { "id", "address", "callsign", "description", "latitude", "longitude", "name", "type" },
                values: new object[,]
                {
                    { new Guid("57decbbe-668f-499e-afc8-1adb9b6b51d4"), "J. B. Winsløws Vej 4, 5000 Odense", "OUH-1", "A hospital", 55.059750000000001, 10.606870000000001, "Odense Universitets Hospital", (byte)1 },
                    { new Guid("fe04250f-a80a-4540-ba54-1b7e68832956"), "Baagøes Alle 31, 5700 Svendborg", "OUH-2", "A hospital", 55.385390999999998, 10.366899999999999, "Svendborg Sygehus (OUH)", (byte)1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Landingpoints");
        }
    }
}
