
using DIS_Monher.Data;
using DIS_Monher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Controllers
{
    public class DespachoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DespachoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR DESPACHOS
        public async Task<IActionResult> Index()
        {
            var despachos = await _context.Despachos
                .OrderByDescending(d => d.FechaRegistro)
                .ToListAsync();

            return View(despachos);
        }

        // MOSTRAR FORMULARIO
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Despacho
            {
                NumeroPedido = "",
                FechaProgramada = DateTime.Today.AddDays(1),
                RangoHorario = "Tarde (14:00 - 19:00)",
                Estado = "Programando Despacho"
            });
        }

        // GUARDAR PROGRAMACIÓN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Despacho despacho)
        {
            var horariosPermitidos = new[]
            {
                "Mañana (08:00 - 13:00)",
                "Tarde (14:00 - 19:00)"
            };

            if (!horariosPermitidos.Contains(despacho.RangoHorario))
            {
                ModelState.AddModelError(
                    nameof(despacho.RangoHorario),
                    "Selecciona un horario válido."
                );
            }

            if (despacho.FechaProgramada.Date < DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(despacho.FechaProgramada),
                    "La fecha no puede ser anterior al día actual."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(despacho);
            }

            despacho.Id = 0;
            despacho.FechaRegistro = DateTime.Now;
            despacho.Estado = "Programando Despacho";

            _context.Despachos.Add(despacho);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] =
                "La programación del despacho se guardó correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // CONSULTAR DETALLE
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var despacho = await _context.Despachos
                .FirstOrDefaultAsync(d => d.Id == id);

            if (despacho == null)
            {
                return NotFound();
            }

            return View(despacho);
        }
    }
}
