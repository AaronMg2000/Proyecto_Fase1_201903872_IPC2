using System;
using System.Collections.Generic;
using ProyectoIPC22011903872.Models.ViewModels;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    public class FilaViewModel
    {
        public string nombre { get; set; }
        public List<ColumnaViewModel> columnas { get; set; }
    }
}