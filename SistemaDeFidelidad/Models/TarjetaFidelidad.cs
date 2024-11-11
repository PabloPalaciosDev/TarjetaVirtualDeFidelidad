using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaDeFidelidad.Models
{
    public class TarjetaFidelidad
    {
        [Key]
        public Guid IdTarjeta { get; set; } = Guid.NewGuid();
        public Guid IdCliente { get; set; }
        [Range(0, 10)]
        [Required]
        public int Puntos { get; set; }
        
        public bool Activa { get; set; } = true;

        //Llaves foraneas
        [JsonIgnore]
        [ForeignKey("IdCliente")]
        public ClienteParticipante Cliente { get; set; } = null!;

        [JsonIgnore]
        public ICollection<DescuentosCliente> Descuentos { get; set; } = null!;


    }
}
