using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Models
{
    // HU001: Registrar materiales e insumos
    [Index(nameof(Codigo), IsUnique = true)]
    public class Material
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingresa el código")]
        [StringLength(30)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "IngresaS el nombre")]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Ingresa la categoría")]
        [StringLength(60)]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa la unidad de medida")]
        [StringLength(20)]
        [Display(Name = "Unidad de medida")]
        public string UnidadMedida { get; set; } = "unid";

        [Range(0, int.MaxValue, ErrorMessage = "Debe ser 0 o mayor")]
        [Display(Name = "Stock actual")]
        public int StockActual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Debe ser 0 o mayor")]
        [Display(Name = "Stock mínimo")]
        public int StockMinimo { get; set; }

        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Display(Name = "Registrado por")]
        public string UsuarioResponsable { get; set; } = "Sistema";

        public bool BajoMinimo => StockActual <= StockMinimo;
    }
}
