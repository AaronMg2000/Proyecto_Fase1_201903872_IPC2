using System;
using System.Collections.Generic;
using ProyectoIPC22011903872.Models.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIPC22011903872.Models.ViewModels
{
    [Serializable]
    public class FilaViewModel
    {
        public FilaViewModel() {
            this.columnas =new List<ColumnaViewModel>();
        }
        public string nombre;
        public List<ColumnaViewModel> columnas;
    }
}