using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class getUserViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Correo_Electronico { get; set; }
        public DateTime Fecha_nacimiento { get; set; }
        public int Codigo_Pais { get; set; }

    }
}