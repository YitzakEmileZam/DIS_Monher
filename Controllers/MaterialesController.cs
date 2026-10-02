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
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Descripcion,Categoria,UnidadMedida,StockMinimo,UsuarioRegistro")] Material material)
        {
            // Validar código repetido
            if (await _context.Materiales.AnyAsync(m => m.Codigo == material.Codigo))
            {
                ModelState.AddModelError("Codigo", "El código ya existe");
                return View(material);
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
    }
}
