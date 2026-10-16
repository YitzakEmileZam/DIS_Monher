using DIS_Monher.Data;
using DIS_Monher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Controllers
{
    // Solo el administrador gestiona materiales e inventario
    [Authorize(Roles = "Admin")]
    public class MaterialesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MaterialesController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? q)
        {
            var query = _db.Materiales.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(m => m.Codigo.Contains(q) || m.Nombre.Contains(q) || m.Categoria.Contains(q));
            }
            ViewBag.Q = q;
            return View(await query.OrderBy(m => m.Nombre).ToListAsync());
        }

        public IActionResult Create() => View(new Material());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material m)
        {
            if (await _db.Materiales.AnyAsync(x => x.Codigo == m.Codigo))
                ModelState.AddModelError(nameof(m.Codigo), "Ya existe un material con este código");

            if (!ModelState.IsValid) return View(m);

            m.FechaIngreso = DateTime.Now;
            m.UsuarioResponsable = User.Identity?.Name ?? "Sistema";

            _db.Add(m);
            await _db.SaveChangesAsync();
            TempData["Ok"] = $"Material {m.Codigo} registrado";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var m = await _db.Materiales.FindAsync(id);
            return m == null ? NotFound() : View(m);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Material m)
        {
            if (id != m.Id) return NotFound();

            if (await _db.Materiales.AnyAsync(x => x.Codigo == m.Codigo && x.Id != id))
                ModelState.AddModelError(nameof(m.Codigo), "Ya existe un material con este código");

            if (!ModelState.IsValid) return View(m);

            _db.Update(m);
            await _db.SaveChangesAsync();
            TempData["Ok"] = $"Material {m.Codigo} actualizado";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Materiales.FindAsync(id);
            if (m != null)
            {
                _db.Remove(m);
                await _db.SaveChangesAsync();
                TempData["Ok"] = $"Material {m.Codigo} eliminado";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
