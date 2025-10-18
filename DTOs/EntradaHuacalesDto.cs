namespace GestionHuacales.Api.DTOs;

public class EntradaHuacalesDto
{
    public string NombreCliente { get; set; } = string.Empty;
    public ICollection<DetalleHuacalesDto> Detalle { get; set; } = [];
}
