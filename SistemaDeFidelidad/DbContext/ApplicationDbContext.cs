namespace SistemaDeFidelidad.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using SistemaDeFidelidad.Models;
    public class ApplicationDbContext: DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<TarjetaFidelidad> TarjetasFidelidad { get; set; } = null!;
        public DbSet<DescuentosCliente> DescuentosClientes { get; set; } = null!;
        public DbSet<ClienteParticipante> ClientesParticipantes { get; set; } = null!;

    }
}
