using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maurice.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<string>(type: "TEXT", nullable: false),
                    Folio = table.Column<string>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RfcEmisor = table.Column<string>(type: "TEXT", nullable: false),
                    NombreEmisor = table.Column<string>(type: "TEXT", nullable: false),
                    RfcReceptor = table.Column<string>(type: "TEXT", nullable: false),
                    TotalImpuesto = table.Column<decimal>(type: "TEXT", nullable: false),
                    Base = table.Column<decimal>(type: "TEXT", nullable: false),
                    Tasa = table.Column<decimal>(type: "TEXT", nullable: false),
                    Importe = table.Column<decimal>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rfc = table.Column<string>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    CodigoPostal = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegimenFiscal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegimenFiscal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegimenFiscal_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LimitesIsrSat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LimiteInferior = table.Column<decimal>(type: "TEXT", nullable: false),
                    LimiteSuperior = table.Column<decimal>(type: "TEXT", nullable: false),
                    CuotaFija = table.Column<decimal>(type: "TEXT", nullable: false),
                    PorcentajeSobreLimiteInferior = table.Column<decimal>(type: "TEXT", nullable: false),
                    Periodo = table.Column<decimal>(type: "TEXT", nullable: false),
                    RegimenId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimitesIsrSat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LimitesIsrSat_RegimenFiscal_RegimenId",
                        column: x => x.RegimenId,
                        principalTable: "RegimenFiscal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LimitesIsrSat_RegimenId",
                table: "LimitesIsrSat",
                column: "RegimenId");

            migrationBuilder.CreateIndex(
                name: "IX_RegimenFiscal_UserId",
                table: "RegimenFiscal",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "LimitesIsrSat");

            migrationBuilder.DropTable(
                name: "RegimenFiscal");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
