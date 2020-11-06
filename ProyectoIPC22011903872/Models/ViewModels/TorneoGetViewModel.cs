using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class TorneoGetViewModel
    {
        public int codigo_Torneo { get; set; }
        public string nombre { get; set; }
        public int Tipo { get; set; }
        public DateTime fecha { get; set; }
        public int? ganador { get; set; }
    }
}