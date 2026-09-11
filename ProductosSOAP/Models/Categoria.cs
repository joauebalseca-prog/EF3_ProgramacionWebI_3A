using System.Runtime.Serialization;

namespace ProductosSOAP.Models;

[DataContract(Namespace = "http://programacionweb/examen3a/models")]
public class Categoria
{
    [DataMember(Order = 1)] public int IdCategoria { get; set; }
    [DataMember(Order = 2)] public string Nombre { get; set; } = string.Empty;
    [DataMember(Order = 3)] public string? Descripcion { get; set; }
    [DataMember(Order = 4)] public bool Estado { get; set; }
}
