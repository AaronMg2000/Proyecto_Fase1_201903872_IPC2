using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    [Serializable]
    public class PartidaViewModel
    {
        public PartidaViewModel() {
            this.Filas = new List<FilaViewModel>();
            this.terminado = "";
            this.tipo = "";
        }
        public int nombre { get; set; }
        public string terminado { get; set; }
        public string tipo { get; set; }
        public string jugador1 { get; set; }
        public string jugador2 { get; set; }
        public string ultimafila { get; set; }
        public string ultimacolumna { get; set; }
        public string color_jugador1 { get; set; }
        public string color_jugador2 { get; set; }
        public int punteo_jugador1 { get; set; }
        public int punteo_jugador2 { get; set; }
        public int movimientos_1 { get; set; }
        public int movimientos_2 { get; set; }
        public string siguiente_tiro { get; set; }
        public List<FilaViewModel> Filas { get; set; }
    }
}