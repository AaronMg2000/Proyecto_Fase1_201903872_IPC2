using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class DatosModel
    {
        public DatosModel() {
            nombre = 0;
            partidas = new List<PartidaViewModel>();
        }
        public int nombre { get; set; }
        public List<PartidaViewModel> partidas { get; set; }
    }
}