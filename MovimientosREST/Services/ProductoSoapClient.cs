using System.Globalization;
using System.Text;
using System.Xml.Linq;
using MovimientosREST.Models;

namespace MovimientosREST.Services;

public class ProductoSoapClient
{
    private const string ContractNamespace = "http://programacionweb/examen3a";
    private readonly HttpClient _httpClient;
    private readonly string _soapUrl;

    public ProductoSoapClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _soapUrl = configuration["ProductosSoapUrl"]
            ?? throw new InvalidOperationException("No se configuró ProductosSoapUrl.");
    }

    public async Task<ProductoSoapDto?> ObtenerProductoAsync(int idProducto)
    {
        var body = $"""
            <ObtenerProducto xmlns="{ContractNamespace}">
              <idProducto>{idProducto}</idProducto>
            </ObtenerProducto>
            """;

        var xml = await EnviarAsync(
            "http://programacionweb/examen3a/IProductoService/ObtenerProducto",
            body);

        var producto = xml.Descendants().FirstOrDefault(x => x.Name.LocalName == "ObtenerProductoResult");
        if (producto is null || producto.Attributes().Any(a => a.Name.LocalName == "nil" && a.Value == "true")) return null;

        return new ProductoSoapDto
        {
            IdProducto = LeerInt(producto, "IdProducto"),
            IdCategoria = LeerInt(producto, "IdCategoria"),
            Nombre = LeerTexto(producto, "Nombre"),
            Precio = LeerDecimal(producto, "Precio"),
            Stock = LeerInt(producto, "Stock"),
            Estado = LeerBool(producto, "Estado")
        };
    }

    public async Task<bool> ActualizarStockAsync(int idProducto, int nuevoStock)
    {
        var body = $"""
            <ActualizarStock xmlns="{ContractNamespace}">
              <idProducto>{idProducto}</idProducto>
              <nuevoStock>{nuevoStock}</nuevoStock>
            </ActualizarStock>
            """;

        var xml = await EnviarAsync(
            "http://programacionweb/examen3a/IProductoService/ActualizarStock",
            body);

        var result = xml.Descendants()
            .FirstOrDefault(x => x.Name.LocalName == "ActualizarStockResult")?.Value;

        return bool.TryParse(result, out var ok) && ok;
    }

    private async Task<XDocument> EnviarAsync(string soapAction, string body)
    {
        var envelope = $"""
            <?xml version="1.0" encoding="utf-8"?>
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                {body}
              </soap:Body>
            </soap:Envelope>
            """;

        using var request = new HttpRequestMessage(HttpMethod.Post, _soapUrl);
        request.Content = new StringContent(envelope, Encoding.UTF8, "text/xml");
        request.Headers.TryAddWithoutValidation("SOAPAction", $"\"{soapAction}\"");

        using var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"ProductoSOAP respondió {(int)response.StatusCode}: {content}");

        return XDocument.Parse(content);
    }

    private static XElement? Campo(XElement parent, string localName) =>
        parent.Descendants().FirstOrDefault(x => x.Name.LocalName == localName);

    private static string LeerTexto(XElement parent, string localName) =>
        Campo(parent, localName)?.Value ?? string.Empty;

    private static int LeerInt(XElement parent, string localName) =>
        int.TryParse(LeerTexto(parent, localName), out var value) ? value : 0;

    private static decimal LeerDecimal(XElement parent, string localName) =>
        decimal.TryParse(LeerTexto(parent, localName), NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            ? value : 0m;

    private static bool LeerBool(XElement parent, string localName) =>
        bool.TryParse(LeerTexto(parent, localName), out var value) && value;
}
