using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeFidelidad.Models
{
    public class DescuentosCliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDescuento { get; set; }

        // Relación obligatoria con Cliente
        public Guid IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public ClienteParticipante Cliente { get; set; } = null!;

        // Relación opcional con Tarjeta
        public Guid? IdTarjeta { get; set; }
        [ForeignKey("IdTarjeta")]
        public TarjetaFidelidad? Tarjeta { get; set; }

        // Propiedades adicionales
        public int CantidadDescuento { get; set; }
        public bool Usado { get; set; } = false;
    }
}
