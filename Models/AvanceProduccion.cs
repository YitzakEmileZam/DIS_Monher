using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIS_Monher.Models
{
    public class AvanceProduccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrdenProduccionId { get; set; }

        public OrdenProduccion? OrdenProduccion { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe seleccionar un estado")]
        public string Estado { get; set; } = "En Proceso";

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad producida no puede ser negativa")]
        [Display(Name = "Cantidad producida")]
        public int CantidadProducida { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }
}