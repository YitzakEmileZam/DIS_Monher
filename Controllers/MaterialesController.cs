
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIS_Monher.Data;
using DIS_Monher.Models;

namespace DIS_Monher.Controllers
{
    public class MaterialesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Materiales
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materiales.ToListAsync());
        }

        // GET: Materiales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materiales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Codigo,Nombre,Descripcion,Categoria,UnidadMedida,StockMinimo,UsuarioRegistro")]
            Material material)
        {
            if (await _context.Materiales.AnyAsync(m => m.Codigo == material.Codigo))
            {
                ModelState.AddModelError("Codigo", "El código ya existe");
            }

            if (ModelState.IsValid)
            {
                material.FechaIngreso = DateTime.Now;
                material.StockActual = 0;

                _context.Add(material);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(material);
        }

        // GET: Materiales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var material = await _context.Materiales.FindAsync(id);

            if (material == null)
                return NotFound();

            return View(material);
        }

        // POST: Materiales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Codigo,Nombre,Descripcion,Categoria,UnidadMedida,StockMinimo,UsuarioRegistro")]
            Material material)
        {
            if (id != material.Id)
                return NotFound();

            if (await _context.Materiales.AnyAsync(
                m => m.Codigo == material.Codigo && m.Id != material.Id))
            {
                ModelState.AddModelError("Codigo", "El código ya existe");
            }

            if (!ModelState.IsValid)
                return View(material);

            var existente = await _context.Materiales.FindAsync(id);

            if (existente == null)
                return NotFound();

            existente.Codigo = material.Codigo;
            existente.Nombre = material.Nombre;
            existente.Descripcion = material.Descripcion;
            existente.Categoria = material.Categoria;
            existente.UnidadMedida = material.UnidadMedida;
            existente.StockMinimo = material.StockMinimo;
            existente.UsuarioRegistro = material.UsuarioRegistro;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Materiales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var material = await _context.Materiales.FindAsync(id);

            if (material == null)
                return NotFound();

            return View(material);
        }

        // POST: Materiales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materiales.FindAsync(id);

            if (material == null)
                return NotFound();

            _context.Materiales.Remove(material);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
