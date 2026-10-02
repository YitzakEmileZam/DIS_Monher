using System;
using System.ComponentModel.DataAnnotations;

namespace DIS_Monher.Models
{
    public class MovimientoStock
    {
        public int Id { get; set; }

        [Display(Name = "Material")]
        public int MaterialId { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty; // "entrada" o "salida"

        [Required]
        public int Cantidad { get; set; }

        [StringLength(200)]
        public string? Motivo { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Display(Name = "Responsable")]
        [StringLength(100)]
        public string? Responsable { get; set; }

        public Material? Material { get; set; }
    }
}
