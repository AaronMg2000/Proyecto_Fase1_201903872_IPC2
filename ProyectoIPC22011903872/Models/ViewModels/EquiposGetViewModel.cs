using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class EquiposGetViewModel
    {
        public int Codigo_Equipo { get; set; }
        public string Nombre { get; set; }
        public int Codigo_Torneo { get; set; }
        public int PUNTEO { get; set; }
    }
}