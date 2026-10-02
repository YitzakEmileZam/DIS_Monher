using System;
using System.ComponentModel.DataAnnotations;

namespace DIS_Monher.Models
{
    public class Material
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [StringLength(100)]
        public string? Categoria { get; set; }

        [StringLength(50)]
        public string? UnidadMedida { get; set; }

        [Display(Name = "Stock Mínimo")]
        public int StockMinimo { get; set; }

        [Display(Name = "Stock Actual")]
        public int StockActual { get; set; }

        [Display(Name = "Fecha Ingreso")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Display(Name = "Usuario Responsable")]
        [StringLength(100)]
        public string? UsuarioRegistro { get; set; }
    }
}
