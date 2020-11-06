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
            this.colores_jugador1 = new List<string>();
            this.colores_jugador2 = new List<string>();
            this.colores_contrario = new List<string>();
            this.colores_actual = new List<string>();
            this.terminado = "";
            this.Ttiempo1 = "00:00:00";
            this.Ttiempo2 = "00:00:00";
            this.modalidad = "";
            this.centro = false;
            this.tipo = "";
        }
        public int nombre { get; set; }
        public string apertura { get; set; }
        public string terminado { get; set; }
        public string TipoPartida { get; set; }
        public string tipo { get; set; }
        public string modalidad { get; set; }
        public string jugador1 { get; set; }
        public string jugador2 { get; set; }
        public bool pt2 { get; set; }
        public bool pt1 { get; set; }
        public int tiempoJ1 { get; set; }
        public string Ttiempo1 { get; set; }
        public string Ttiempo2 { get; set; }
        public int tiempoJ2 { get; set; }
        public string ultimafila { get; set; }
        public string ultimacolumna { get; set; }
        public string colorA1 { get; set; }
        public string colorA2 { get; set; }
        public bool centro { get; set; }
        public List<string> colores_jugador1 { get; set; }
        public List<string> colores_jugador2 { get; set; }
        public int punteo_jugador1 { get; set; }
        public int punteo_jugador2 { get; set; }
        public int movimientos_1 { get; set; }
        public int movimientos_2 { get; set; }
        public int N { get; set; }
        public int M { get; set; }
        public string NP { get; set; }
        public string MP { get; set; }
        public List<string> colores_actual { get; set; }
        public List<string> colores_contrario { get; set; }
        public string siguiente_tiro { get; set; }
        public List<FilaViewModel> Filas { get; set; }
    }
}