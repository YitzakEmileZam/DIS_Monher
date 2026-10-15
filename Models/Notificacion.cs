using System.ComponentModel.DataAnnotations;

namespace DIS_Monher.Models
{
    public class Notificacion
    {
        public int Id { get; set; }

        [Required]
        public string Mensaje { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public string Area { get; set; } = "Distribución";

        public bool Leida { get; set; } = false;

        public int OrdenProduccionId { get; set; }

        public OrdenProduccion? OrdenProduccion { get; set; }
    }
}