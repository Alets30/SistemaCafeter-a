using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCafeteria.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El Campo Nombre es Requerido")]
        [MaxLength(60, ErrorMessage = "El nombre se compone con 60  caracteres como maximo")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Campo Descripcion es Requerido")]
        [MaxLength(100, ErrorMessage = "La Descripcion se compone con 100 caracteres como maximo")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El estado de la Bodega es Requerido")]
        public bool Estado { get; set; }
    }
}
