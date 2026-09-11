using System.ComponentModel.DataAnnotations;

namespace MovimientosREST.Models;

public class MovimientoInventario
{
    public int IdMovimiento { get; set; }

    [Required]
    public int IdProducto { get; set; }

    [Required, MaxLength(10)]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    public int Cantidad { get; set; }

    public DateTime Fecha { get; set; }

    [MaxLength(250)]
    public string? Observacion { get; set; }

    public int StockAnterior { get; set; }
    public int StockResultante { get; set; }
}
