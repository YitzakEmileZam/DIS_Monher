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

        // GET: Stock
        public async Task<IActionResult> Index()
        {
            var materiales = await _context.Materiales.ToListAsync();
            return View(materiales);
        }

        // GET: Stock/Entrada
        public async Task<IActionResult> Entrada()
        {
            ViewBag.Materiales = await _context.Materiales.ToListAsync();
            return View();
        }

        // POST: Stock/Entrada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entrada([Bind("MaterialId,Cantidad,Motivo,Responsable")] MovimientoStock movimiento)
        {
            var material = await _context.Materiales.FindAsync(movimiento.MaterialId);
            if (material == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Actualizar stock automáticamente
                material.StockActual += movimiento.Cantidad;

                movimiento.Tipo = "entrada";
                movimiento.Fecha = DateTime.Now;

                _context.MovimientosStock.Add(movimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Materiales = await _context.Materiales.ToListAsync();
            return View(movimiento);
        }

        // GET: Stock/Salida
        public async Task<IActionResult> Salida()
        {
            ViewBag.Materiales = await _context.Materiales.ToListAsync();
            return View();
        }

        // POST: Stock/Salida
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Salida([Bind("MaterialId,Cantidad,Motivo,Responsable")] MovimientoStock movimiento)
        {
            var material = await _context.Materiales.FindAsync(movimiento.MaterialId);
            if (material == null)
            {
                return NotFound();
            }

            // Validar stock suficiente
            if (material.StockActual < movimiento.Cantidad)
            {
                ModelState.AddModelError("Cantidad", $"Stock insuficiente. Stock actual: {material.StockActual}");
                ViewBag.Materiales = await _context.Materiales.ToListAsync();
                return View(movimiento);
            }

            if (ModelState.IsValid)
            {
                // Actualizar stock automáticamente
                material.StockActual -= movimiento.Cantidad;

                movimiento.Tipo = "salida";
                movimiento.Fecha = DateTime.Now;

                _context.MovimientosStock.Add(movimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Materiales = await _context.Materiales.ToListAsync();
            return View(movimiento);
        }

        // GET: Stock/Historial/5
        public async Task<IActionResult> Historial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimientos = await _context.MovimientosStock
                .Where(m => m.MaterialId == id)
                .Include(m => m.Material)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();

            return View(movimientos);
        }
    }
}
