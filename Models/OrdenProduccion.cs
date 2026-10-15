using System.ComponentModel.DataAnnotations;

namespace DIS_Monher.Models
{
    public class OrdenProduccion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de orden es obligatorio")]
        [Display(Name = "Número de orden")]
        public string NumeroOrden { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El cliente es obligatorio")]
        public string Cliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de caja es obligatorio")]
        [Display(Name = "Tipo de caja")]
        public string TipoCaja { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de entrega")]
        public DateTime FechaEntrega { get; set; }

        [Required(ErrorMessage = "El responsable es obligatorio")]
        public string Responsable { get; set; } = string.Empty;

        public string Estado { get; set; } = "Pendiente";
        public ICollection<AvanceProduccion> Avances { get; set; }
            = new List<AvanceProduccion>();
    }
}