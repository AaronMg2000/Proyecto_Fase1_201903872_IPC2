using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class Encuentro
    {
        public Encuentro()
        {
            Partidas = new DatosModel();
            Partidas.nombre = 0;
        }
        public int codigo { get; set; }
        public string Equipo1 { get; set; }
        public string Equipo2 { get; set; }
        public int punteo1 { get; set; }
        public int punteo2 { get; set; }
        public string Ganador { get; set; }
        public string actual { get; set; }
        public string resultado1 { get; set; }
        public string resultado2 { get; set; }
        public DatosModel Partidas {get;set;}
    }
}