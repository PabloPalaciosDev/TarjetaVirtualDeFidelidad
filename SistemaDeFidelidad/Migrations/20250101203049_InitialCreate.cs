using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeFidelidad.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientesParticipantes",
                columns: table => new
                {
                    IdCliente = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CedulaCliente = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreCliente = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoCliente = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailCliente = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelefonoCliente = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contrasena = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesParticipantes", x => x.IdCliente);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TarjetasFidelidad",
                columns: table => new
                {
                    IdTarjeta = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdCliente = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Puntos = table.Column<int>(type: "int", nullable: false),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarjetasFidelidad", x => x.IdTarjeta);
                    table.ForeignKey(
                        name: "FK_TarjetasFidelidad_ClientesParticipantes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "ClientesParticipantes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DescuentosClientes",
                columns: table => new
                {
                    IdDescuento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCliente = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdTarjeta = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CantidadDescuento = table.Column<int>(type: "int", nullable: false),
                    Usado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescuentosClientes", x => x.IdDescuento);
                    table.ForeignKey(
                        name: "FK_DescuentosClientes_ClientesParticipantes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "ClientesParticipantes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DescuentosClientes_TarjetasFidelidad_IdTarjeta",
                        column: x => x.IdTarjeta,
                        principalTable: "TarjetasFidelidad",
                        principalColumn: "IdTarjeta");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DescuentosClientes_IdCliente",
                table: "DescuentosClientes",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_DescuentosClientes_IdTarjeta",
                table: "DescuentosClientes",
                column: "IdTarjeta");

            migrationBuilder.CreateIndex(
                name: "IX_TarjetasFidelidad_IdCliente",
                table: "TarjetasFidelidad",
                column: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescuentosClientes");

            migrationBuilder.DropTable(
                name: "TarjetasFidelidad");

            migrationBuilder.DropTable(
                name: "ClientesParticipantes");
        }
    }
}
