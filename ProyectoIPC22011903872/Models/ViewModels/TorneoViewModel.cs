using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class TorneoViewModel
    {
        public TorneoViewModel()
        {
            Equipos = new List<Equipo>();
            Fases = new List<Fase>();
        }
        public int codigo { get; set; }
        public string Nombre { get; set; }
        public int Tipo { get; set; }
        public List<Equipo> Equipos { get; set; }
        public List<Fase> Fases { get; set; }
        public string Ganador { get; set; }
    }
}