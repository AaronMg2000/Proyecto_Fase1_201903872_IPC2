using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class addUserViewModel
    {
        [Display(Name = "Nombre")]
        [Required]
        [StringLength(100, ErrorMessage = "Longitud máxima de 100 caracteres")]
        [MinLength(1, ErrorMessage = "Longitud minima de 1 caracter")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }


        [Display(Name = "Apellido")]
        [Required]
        [StringLength(100, ErrorMessage = "Longitud máxima de 100 caracteres")]
        [MinLength(1, ErrorMessage = "Longitud minima de 1 caracter")]
        [DataType(DataType.Text)]
        public string Apellido { get; set; }

        [Display(Name = "Usuario")]
        [Required]
        [StringLength(100, ErrorMessage = "Longitud máxima de 100 caracteres")]
        [MinLength(5, ErrorMessage = "Longitud minima de 5 caracteres")]
        [DataType(DataType.Text)]
        public string Usuario { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(100, ErrorMessage = "Longitud máxima de 100 caracteres")]
        [MinLength(8, ErrorMessage = "Longitud minima de 8 caracteres")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){7,100}", ErrorMessage = "Error. La contraseña debe tener una mayúscula, un carácter especial y un carácter numérico.No puede comenzar con un carácter especial o un dígito.")]
        public string Password { get; set; }

        [Display(Name = "RePassword")]
        [Required]
        [StringLength(100, ErrorMessage = "Longitud máxima de 100 caracteres")]
        [MinLength(8, ErrorMessage = "Longitud minima de 8 caracteres")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){7,100}", ErrorMessage = "Error. La contraseña debe tener una mayúscula, un carácter especial y un carácter numérico.No puede comenzar con un carácter especial o un dígito.")]
        public string RePassword { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessage = "Dirección de Correo electrónico incorrecta.")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        [DataType(DataType.EmailAddress)]
        public string Correo_Electronico { get; set; }
        [Display(Name ="Fecha Nacimiento")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha_nacimiento { get; set; }
        [Display(Name = "Codigo Pais")]
        [Required]
        [DataType(DataType.Text)]
        public int Codigo_Pais { get; set; }
    }
}