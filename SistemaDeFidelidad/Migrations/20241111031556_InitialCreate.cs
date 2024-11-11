using System;
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
            migrationBuilder.CreateTable(
                name: "ClientesParticipantes",
                columns: table => new
                {
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CedulaCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoCliente = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesParticipantes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "TarjetasFidelidad",
                columns: table => new
                {
                    IdTarjeta = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Puntos = table.Column<int>(type: "int", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "DescuentosClientes",
                columns: table => new
                {
                    IdDescuento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdTarjeta = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CantidadDescuento = table.Column<int>(type: "int", nullable: false),
                    Usado = table.Column<bool>(type: "bit", nullable: false)
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
                });

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
