using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class Fase
    {
        public Fase()
        {
            Encuentros = new List<Encuentro>();
        }
        public int nombre { get; set; }
        public List<Encuentro> Encuentros { get; set; }
    }
}