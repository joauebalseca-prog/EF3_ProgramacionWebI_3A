using CoreWCF;
using ProductosSOAP.Data;
using ProductosSOAP.Models;

namespace ProductosSOAP.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
public class ProductoService : IProductoService
{
    private readonly InventarioDbContext _context;

    public ProductoService(InventarioDbContext context)
    {
        _context = context;
    }

    public List<Producto> ListarProductos()
    {
        return _context.Productos.OrderBy(x => x.IdProducto).ToList();
    }

    public Producto? ObtenerProducto(int idProducto)
    {
        return _context.Productos.FirstOrDefault(x => x.IdProducto == idProducto);
    }

    public Producto CrearProducto(Producto producto)
    {
        producto.IdProducto = 0;
        _context.Productos.Add(producto);
        _context.SaveChanges();
        return producto;
    }

    public Producto? ActualizarProducto(Producto producto)
    {
        var actual = _context.Productos.Find(producto.IdProducto);
        if (actual is null) return null;

        actual.IdCategoria = producto.IdCategoria;
        actual.Nombre = producto.Nombre;
        actual.Descripcion = producto.Descripcion;
        actual.Precio = producto.Precio;
        actual.Stock = producto.Stock;
        actual.Estado = producto.Estado;
        _context.SaveChanges();
        return actual;
    }

    public bool EliminarProducto(int idProducto)
    {
        var actual = _context.Productos.Find(idProducto);
        if (actual is null) return false;

        _context.Productos.Remove(actual);
        _context.SaveChanges();
        return true;
    }

    public bool ActualizarStock(int idProducto, int nuevoStock)
    {
        if (nuevoStock < 0) return false;

        var producto = _context.Productos.Find(idProducto);
        if (producto is null) return false;

        producto.Stock = nuevoStock;
        _context.SaveChanges();
        return true;
    }
}
