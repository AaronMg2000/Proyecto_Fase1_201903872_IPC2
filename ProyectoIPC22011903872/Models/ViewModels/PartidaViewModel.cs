using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class PartidaViewModel
    {
        public string jugador1 { get; set; }
        public string jugador2 { get; set; }
        public int punteo_jugador1 { get; set; }
        public int punteo_jugador2 { get; set; }
        public int movimientos_1 { get; set; }
        public int movimientos_2 { get; set; }
        public string siguiente_tiro { get; set; }
        public List<FilaViewModel> Filas{get;set;}

    }
}