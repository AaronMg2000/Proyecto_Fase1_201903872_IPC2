using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class EncuentroGet
    {
		public int Codigo_Encuentro { get; set; }
		public int  Codigo_Equipo1 { get; set; }
		public int Codigo_Equipo2 { get; set; }
		public int Numero_Fase { get; set; }
		public string Ganador { get; set; }
		public int Punteo_Equipo1 { get; set; }
		public int Punteo_Equipo2 { get; set; }

	}
}