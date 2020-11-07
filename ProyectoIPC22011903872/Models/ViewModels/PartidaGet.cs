using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class PartidaGet
    {
        public int	Codigo_Partida { get; set; }
		public int Codigo_Usuario_1 { get; set; }
		public int? Codigo_Encuentro { get; set; }
		public int? Codigo_Usuario_2 { get; set; }
		public string ResultadoLocal { get; set; }
		public int? GanadorUsuario { get; set; }
		public int TIPO { get; set; }
		public int MODO { get; set; }
		public int? CONTRINCANTE { get; set; }
		public int? Punteo_1 { get; set; }
		public int?  Punteo_2 { get; set; }
		public int? movimientos_1 { get; set; }
		public int? movimientos_2 { get; set; }
		public string Tiempo_Jugador1 { get; set; }
		public string Tiempo_Jugador2 { get; set; }
		public DateTime Fecha { get; set; }

	}
}