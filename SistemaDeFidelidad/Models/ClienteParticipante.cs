using System.ComponentModel.DataAnnotations;

namespace SistemaDeFidelidad.Models
{
    public class ClienteParticipante
    {
        //id guid
        [Key]
        public Guid IdCliente { get; set; }
        //Dato unico
        [Required]
        public string CedulaCliente { get; set; } = string.Empty;
        [Required]
        public String NombreCliente { get; set; } = string.Empty;
        [Required]
        public string ApellidoCliente { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string EmailCliente { get; set; } = string.Empty;
        [Required]
        public string TelefonoCliente { get; set; } = string.Empty;

        public ICollection<TarjetaFidelidad> Tarjetas { get; set; } = null!;

        public ICollection<DescuentosCliente> Descuentos { get; set; } = null!;

    }
}
