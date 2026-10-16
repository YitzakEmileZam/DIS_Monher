using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Models
{
    // HU004: productos terminados (lo que ve el cliente en el catálogo)
    [Index(nameof(Codigo), IsUnique = true)]
    public class Producto
    {
        public int Id { get; set; }

        [Required, StringLength(30)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required, StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required, StringLength(60)]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = string.Empty;

        [StringLength(60)]
        public string? Acabado { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public bool Activo { get; set; } = true;

        // Estado que se muestra al cliente
        public string EstadoStock => Stock <= 0 ? "Agotado" : Stock <= 5 ? "Stock bajo" : "Disponible";
    }
}
