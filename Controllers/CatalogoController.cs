using DIS_Monher.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Controllers
{
    // Vista del cliente: cualquiera puede ver el catálogo
    [AllowAnonymous]
    public class CatalogoController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CatalogoController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? q, string? categoria)
        {
            var query = _db.Productos.Where(p => p.Activo);

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria == categoria);

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p => p.Nombre.Contains(q) || (p.Descripcion != null && p.Descripcion.Contains(q)));

            ViewBag.Q = q;
            ViewBag.Categoria = categoria;
            ViewBag.Categorias = await _db.Productos.Where(p => p.Activo)
                .Select(p => p.Categoria).Distinct().OrderBy(c => c).ToListAsync();

            return View(await query.OrderBy(p => p.Categoria).ThenBy(p => p.Nombre).ToListAsync());
        }
    }
}
