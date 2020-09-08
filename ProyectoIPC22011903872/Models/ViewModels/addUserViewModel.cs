using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class addUserViewModel
    {
        [Display(Name ="Nombre")]
        [Required]
        [StringLength(100,ErrorMessage = "Longitud máxima de 100 caracteres")]
        [MinLength(8,ErrorMessage ="Longitud minima de 8 caracteres")]
        [RegularExpression(@"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){7,100}", ErrorMessage = "Error. La contraseña debe tener una mayúscula, un carácter especial y un carácter numérico.No puede comenzar con un carácter especial o un dígito.")]
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }

        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Correo_Electronico { get; set; }
        public DateTime Fecha_nacimiento { get; set; }
        public int Codigo_Pais { get; set; }
    }
}