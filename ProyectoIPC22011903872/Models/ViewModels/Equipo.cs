using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class Equipo
    {
        public Equipo()
        {
            jugadores = new List<string>();
        }
        public string nombre { get; set; }
        public int puntaje { get; set; }
        public List<string> jugadores { get; set; }
    }
}