
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIS_Monher.Data;
using DIS_Monher.Models;

namespace DIS_Monher.Controllers
{
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Stock
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var materiales = await _context.Materiales
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .ToListAsync();

            return View("Index", materiales);
        }

        // GET: /Stock/Entrada
        [HttpGet]
        public async Task<IActionResult> Entrada(int? materialId)
        {
            await CargarMateriales();

            var movimiento = new MovimientoStock();

            if (materialId.HasValue)
            {
                var existe = await _context.Materiales
                    .AnyAsync(m => m.Id == materialId.Value);

                if (!existe)
                {
                    return NotFound("El material seleccionado no existe.");
                }

                movimiento.MaterialId = materialId.Value;
            }

            return View(movimiento);
        }

        // POST: /Stock/Entrada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entrada(
            [Bind("MaterialId,Cantidad,Motivo,Responsable")]
            MovimientoStock movimiento)
        {
            movimiento.Tipo = "entrada";
            movimiento.Fecha = DateTime.Now;

            if (movimiento.Cantidad <= 0)
            {
                ModelState.AddModelError(
                    nameof(movimiento.Cantidad),
                    "La cantidad debe ser mayor que cero.");
            }

            var material = await _context.Materiales
                .FirstOrDefaultAsync(m => m.Id == movimiento.MaterialId);

            if (material == null)
            {
                ModelState.AddModelError(
                    nameof(movimiento.MaterialId),
                    "Selecciona un material válido.");
            }

            ModelState.Remove(nameof(movimiento.Tipo));
            ModelState.Remove(nameof(movimiento.Material));

            if (!ModelState.IsValid)
            {
                await CargarMateriales();
                return View(movimiento);
            }

            material!.StockActual += movimiento.Cantidad;

            _context.MovimientosStock.Add(movimiento);

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Entrada registrada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Stock/Salida
        [HttpGet]
        public async Task<IActionResult> Salida(int? materialId)
        {
            await CargarMateriales();

            var movimiento = new MovimientoStock();

            if (materialId.HasValue)
            {
                var existe = await _context.Materiales
                    .AnyAsync(m => m.Id == materialId.Value);

                if (!existe)
                {
                    return NotFound("El material seleccionado no existe.");
                }

                movimiento.MaterialId = materialId.Value;
            }

            return View(movimiento);
        }

        // POST: /Stock/Salida
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Salida(
            [Bind("MaterialId,Cantidad,Motivo,Responsable")]
            MovimientoStock movimiento)
        {
            movimiento.Tipo = "salida";
            movimiento.Fecha = DateTime.Now;

            if (movimiento.Cantidad <= 0)
            {
                ModelState.AddModelError(
                    nameof(movimiento.Cantidad),
                    "La cantidad debe ser mayor que cero.");
            }

            var material = await _context.Materiales
                .FirstOrDefaultAsync(m => m.Id == movimiento.MaterialId);

            if (material == null)
            {
                ModelState.AddModelError(
                    nameof(movimiento.MaterialId),
                    "Selecciona un material válido.");
            }
            else if (movimiento.Cantidad > 0 &&
                     material.StockActual < movimiento.Cantidad)
            {
                ModelState.AddModelError(
                    nameof(movimiento.Cantidad),
                    $"Stock insuficiente. Stock actual: {material.StockActual}.");
            }

            ModelState.Remove(nameof(movimiento.Tipo));
            ModelState.Remove(nameof(movimiento.Material));

            if (!ModelState.IsValid)
            {
                await CargarMateriales();
                return View(movimiento);
            }

            material!.StockActual -= movimiento.Cantidad;

            _context.MovimientosStock.Add(movimiento);

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Salida registrada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Stock/Historial?id=5
        [HttpGet]
        public async Task<IActionResult> Historial(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }

            var materialExiste = await _context.Materiales
                .AnyAsync(m => m.Id == id.Value);

            if (!materialExiste)
            {
                return NotFound("El material seleccionado no existe.");
            }

            var movimientos = await _context.MovimientosStock
                .AsNoTracking()
                .Include(m => m.Material)
                .Where(m => m.MaterialId == id.Value)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();

            return View("Historial", movimientos);
        }

        private async Task CargarMateriales()
        {
            ViewBag.Materiales = await _context.Materiales
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }
    }
}
