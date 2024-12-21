namespace SistemaDeFidelidad.Models.DTOs
{
    public class DTOAgregarPuntosTarjeta
    {
        public Guid IdTarjeta { get; set; }
        public Guid IdCliente { get; set; }
        public int Puntos { get; set; }
    }
}
