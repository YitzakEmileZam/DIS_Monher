
using System.ComponentModel.DataAnnotations;

namespace DIS_Monher.Models
{
    public class Despacho
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de pedido es obligatorio.")]
        [Display(Name = "Número de pedido")]
        [StringLength(30)]
        public string NumeroPedido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa la dirección de destino.")]
        [Display(Name = "Dirección de destino")]
        [StringLength(250)]
        public string DireccionDestino { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona la fecha de entrega.")]
        [Display(Name = "Fecha programada")]
        [DataType(DataType.Date)]
        public DateTime FechaProgramada { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "Selecciona un rango horario.")]
        [Display(Name = "Rango horario")]
        public string RangoHorario { get; set; } = "Tarde (14:00 - 19:00)";

        [Display(Name = "Referencias adicionales")]
        [StringLength(500)]
        public string? ReferenciasAdicionales { get; set; }

        [Required(ErrorMessage = "Ingresa el vehículo asignado.")]
        [Display(Name = "Vehículo")]
        [StringLength(100)]
        public string Vehiculo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa el conductor responsable.")]
        [Display(Name = "Conductor responsable")]
        [StringLength(100)]
        public string Conductor { get; set; } = string.Empty;

        [Display(Name = "Estado de entrega")]
        public string Estado { get; set; } = "Programando Despacho";

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
