using DIS_Monher.Data;
using DIS_Monher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Controllers
{
    public class ProduccionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProduccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR ÓRDENES
        public async Task<IActionResult> Index()
        {
            var ordenes = await _context.OrdenesProduccion
                .Include(o => o.Avances)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();

            return View(ordenes);
        }

        // MOSTRAR FORMULARIO
        [HttpGet]
        public IActionResult Create()
        {
            return View(new OrdenProduccion
            {
                Fecha = DateTime.Now,
                FechaEntrega = DateTime.Now.AddDays(1)
            });
        }

        // GUARDAR ORDEN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdenProduccion ordenProduccion)
        {
            if (!ModelState.IsValid)
            {
                return View(ordenProduccion);
            }

            // Estado inicial requerido por HU006
            ordenProduccion.Estado = "Pendiente";

            _context.OrdenesProduccion.Add(ordenProduccion);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Orden de producción creada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // VER DETALLE
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.OrdenesProduccion
                .Include(o => o.Avances)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }
        // MOSTRAR FORMULARIO DE AVANCE
        [HttpGet]
        public async Task<IActionResult> RegistrarAvance(int id)
        {
            var orden = await _context.OrdenesProduccion.FindAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            ViewBag.Orden = orden;

            return View(new AvanceProduccion
            {
                OrdenProduccionId = orden.Id,
                Estado = orden.Estado == "Pendiente"
                    ? "En Proceso"
                    : orden.Estado
            });
        }

        // GUARDAR AVANCE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarAvance(
            [Bind("OrdenProduccionId,Estado,CantidadProducida,Observaciones")]
            AvanceProduccion avance)
        {
            var orden = await _context.OrdenesProduccion
                .FindAsync(avance.OrdenProduccionId);

            if (orden == null)
            {
                return NotFound();
            }

            if (avance.CantidadProducida > orden.Cantidad)
            {
                ModelState.AddModelError(
                    "CantidadProducida",
                    "La cantidad producida no puede superar la cantidad solicitada."
                );
            }

            var estadosPermitidos = new[]
            {
                "En Proceso",
                "Terminado",
                "Cancelado"
            };

            if (!estadosPermitidos.Contains(avance.Estado))
            {
                ModelState.AddModelError(
                    "Estado",
                    "El estado seleccionado no es válido."
                );
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Orden = orden;
                return View(avance);
            }

            avance.Id = 0;
            avance.Fecha = DateTime.Now;

            var estadoAnterior = orden.Estado;

            _context.AvancesProduccion.Add(avance);

            // Actualizar estado de la orden
            orden.Estado = avance.Estado;
            
            // Crear notificación cuando pase a TERMINADO
            if (avance.Estado == "Terminado" && estadoAnterior != "Terminado")
            {
                var notificacion = new Notificacion
                {
                    Mensaje = $"La orden {orden.NumeroOrden} ha terminado su producción y está disponible para distribución.",
                    Fecha = DateTime.Now,
                    Area = "Distribución",
                    Leida = false,
                    OrdenProduccionId = orden.Id
                };

                _context.Notificaciones.Add(notificacion);
            }

            await _context.SaveChangesAsync();

            TempData["Mensaje"] =
                "Avance de producción registrado correctamente.";

            return RedirectToAction("Details", new { id = orden.Id });
        }
    }
}