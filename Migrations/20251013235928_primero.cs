using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionHuacales.Api.Migrations
{
    /// <inheritdoc />
    public partial class primero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Huacales",
                columns: table => new
                {
                    EntradaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCliente = table.Column<string>(type: "TEXT", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Huacales", x => x.EntradaId);
                });

            migrationBuilder.CreateTable(
                name: "TipoHuacales",
                columns: table => new
                {
                    TipoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    Existencia = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoHuacales", x => x.TipoId);
                });

            migrationBuilder.CreateTable(
                name: "DetalleHuacales",
                columns: table => new
                {
                    DetalleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntradaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleHuacales", x => x.DetalleId);
                    table.ForeignKey(
                        name: "FK_DetalleHuacales_Huacales_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Huacales",
                        principalColumn: "EntradaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TipoHuacales",
                columns: new[] { "TipoId", "Descripcion", "Existencia" },
                values: new object[,]
                {
                    { 1, "Pequeña Verde", 0 },
                    { 2, "Pequeña Roja", 0 },
                    { 3, "Mediana Verde", 0 },
                    { 4, "Mediana Roja", 0 },
                    { 5, "Jumbo Verde", 0 },
                    { 6, "Jumbo Roja", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleHuacales_EntradaId",
                table: "DetalleHuacales",
                column: "EntradaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleHuacales");

            migrationBuilder.DropTable(
                name: "TipoHuacales");

            migrationBuilder.DropTable(
                name: "Huacales");
        }
    }
}
