using DIS_Monher.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalMateriales = await _db.Materiales.CountAsync();
            ViewBag.BajoMinimo = await _db.Materiales.CountAsync(m => m.StockActual <= m.StockMinimo);
            ViewBag.TotalProductos = await _db.Productos.CountAsync();
            ViewBag.ProductosBajos = await _db.Productos.CountAsync(p => p.Stock <= 5);
            ViewBag.Criticos = await _db.Materiales
                .Where(m => m.StockActual <= m.StockMinimo)
                .OrderBy(m => m.StockActual)
                .Take(6)
                .ToListAsync();
            return View();
        }
    }
}
