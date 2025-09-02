using System.ComponentModel.DataAnnotations;

namespace WaRazorDB.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 80 caracteres.")]
        public string nombreTarea { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string estado {  get; set; }
        public int idUsuario { get; set; }


    }
}
