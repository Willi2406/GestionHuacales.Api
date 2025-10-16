using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionarHuacales.Api.Models;

public class EntradasHuacales
{
    [Key]
    public int EntradaId {  get; set; }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
    public string NombreCliente { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public DateTime Fecha{ get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(EntradaId))]
    public ICollection<DetalleHuacales> DetalleHuacales { get; set; } = new List<DetalleHuacales>();
}
