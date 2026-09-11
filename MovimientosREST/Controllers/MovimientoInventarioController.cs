using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovimientosREST.Data;
using MovimientosREST.Models;
using MovimientosREST.Services;

namespace MovimientosREST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientoInventarioController : ControllerBase
{
    private readonly MovimientosDbContext _context;
    private readonly ProductoSoapClient _productoSoapClient;

    public MovimientoInventarioController(
        MovimientosDbContext context,
        ProductoSoapClient productoSoapClient
    )
    {
        _context = context;
        _productoSoapClient = productoSoapClient;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovimientoInventario>>> Listar()
    {
        return await _context.MovimientosInventario
            .OrderByDescending(x => x.Fecha)
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovimientoInventario>> Obtener(int id)
    {
        var movimiento = await _context.MovimientosInventario.FindAsync(id);

        return movimiento is null
            ? NotFound()
            : Ok(movimiento);
    }

    [HttpGet("producto/{idProducto:int}")]
    public async Task<ActionResult<List<MovimientoInventario>>> ObtenerPorProducto(int idProducto)
    {
        var movimientos = await _context.MovimientosInventario
            .Where(x => x.IdProducto == idProducto)
            .OrderByDescending(x => x.Fecha)
            .ToListAsync();

        if (movimientos.Count == 0)
        {
            return NotFound();
        }

        return Ok(movimientos);
    }

    [HttpPost]
    public async Task<IActionResult> Registrar(MovimientoInventario movimiento)
    {
        movimiento.Tipo = movimiento.Tipo
            .Trim()
            .ToUpperInvariant();

        if (movimiento.Tipo != "ENTRADA" &&
            movimiento.Tipo != "SALIDA")
        {
            return BadRequest("El tipo debe ser ENTRADA o SALIDA.");
        }

        if (movimiento.Cantidad <= 0)
        {
            return BadRequest("La cantidad debe ser mayor que cero.");
        }

        var producto =
            await _productoSoapClient
                .ObtenerProductoAsync(movimiento.IdProducto);

        if (producto is null)
        {
            return NotFound("El producto no existe.");
        }

        if (!producto.Estado)
        {
            return BadRequest("El producto está inactivo.");
        }

        movimiento.StockAnterior = producto.Stock;

        if (movimiento.Tipo == "ENTRADA")
        {
            movimiento.StockResultante =
                producto.Stock + movimiento.Cantidad;
        }
        else
        {
            if (movimiento.Cantidad > producto.Stock)
            {
                return BadRequest(
                    "La cantidad de salida supera el stock disponible.");
            }

            movimiento.StockResultante =
                producto.Stock - movimiento.Cantidad;
        }

        var actualizado =
            await _productoSoapClient.ActualizarStockAsync(
                movimiento.IdProducto,
                movimiento.StockResultante);

        if (!actualizado)
        {
            return BadRequest(
                "No se pudo actualizar el stock del producto.");
        }

        movimiento.IdMovimiento = 0;
        movimiento.Fecha = DateTime.Now;

        _context.MovimientosInventario.Add(movimiento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Obtener),
            new { id = movimiento.IdMovimiento },
            movimiento);
    }
}