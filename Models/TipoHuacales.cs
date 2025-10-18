using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionarHuacales.Api.Models;

public class TipoHuacales
{
    [Key]
    [Required(ErrorMessage = "Tienes que elegir el tipo de Huacal")]
    public int TipoId { get; set; }

    public string Descripcion { get; set; }

    public int Existencia { get; set; }

}
