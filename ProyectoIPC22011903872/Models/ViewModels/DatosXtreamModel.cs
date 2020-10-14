using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class DatosXtreamModel
    {
        public DatosXtreamModel()
        {
            nombre = 0;
            partidas = new List<PartidaXtreamViewModel>();
        }
        public int nombre { get; set; }
        public List<PartidaXtreamViewModel> partidas { get; set; }
    }
}