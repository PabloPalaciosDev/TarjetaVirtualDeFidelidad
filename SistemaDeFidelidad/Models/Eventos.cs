using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaDeFidelidad.Models
{
    public class Eventos
    {
        //id evento guid
        [Key]
        public Guid IdEvento { get; set; }
        [JsonIgnore]
        [ForeignKey("IdCliente")]
        public ClienteParticipante Cliente { get; set; } = null!;

        public DateTime fechaEvento { get; set; }
        public string nombreEvento { get; set; } = string.Empty;
        public string horaEvento { get; set; } = string.Empty;
        public string lugarEvento { get; set; } = string.Empty;

    }
}
