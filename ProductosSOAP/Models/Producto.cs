using System.Runtime.Serialization;

namespace ProductosSOAP.Models;

[DataContract(Namespace = "http://programacionweb/examen3a/models")]
public class Producto
{
    [DataMember(Order = 1)] public int IdProducto { get; set; }
    [DataMember(Order = 2)] public int IdCategoria { get; set; }
    [DataMember(Order = 3)] public string Nombre { get; set; } = string.Empty;
    [DataMember(Order = 4)] public string? Descripcion { get; set; }
    [DataMember(Order = 5)] public decimal Precio { get; set; }
    [DataMember(Order = 6)] public int Stock { get; set; }
    [DataMember(Order = 7)] public bool Estado { get; set; }
}
