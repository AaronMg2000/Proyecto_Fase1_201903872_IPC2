using ProyectoIPC22011903872.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIPC22011903872.Controllers
{
    public class PartidaController : Controller
    {
        // GET: Partida
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partida() {
            PartidaViewModel partida = new PartidaViewModel();
            string[] columnas = {"A","B","C","D","E","F","G","H"};
            partida.jugador1 = "jugador1";
            partida.jugador2 = "jugador2";
            partida.movimientos_1 = 0;
            partida.movimientos_2 = 0;
            partida.punteo_jugador1 = 2;
            partida.punteo_jugador2 = 2;
            partida.siguiente_tiro = "blanco";
            partida.Filas = new List<FilaViewModel>();
            for (int i = 1; i <= 8; i++)
            {
                FilaViewModel fila = new FilaViewModel();
                fila.nombre = i.ToString();
                fila.columnas = new List<ColumnaViewModel>();
                for (int j = 0; j < columnas.Length; j++)
                {
                    ColumnaViewModel col = new ColumnaViewModel();
                    col.color = "";
                    col.nombre = columnas[j];

                    if (j==3 && i==4)
                    {
                        col.color = "blanco";
                    }
                    if (j == 3 && i == 5)
                    {
                        col.color = "negro";
                    }
                    if (j == 4 && i == 4)
                    {
                        col.color = "negro";
                    }
                    if (j == 4 && i == 5)
                    {
                        col.color = "blanco";
                    }
                    fila.columnas.Add(col);
                }
                partida.Filas.Add(fila);
            }
            ViewBag.partida = partida;
            return View();
        }
        [HttpPost]
        public ActionResult Partida(FilaViewModel model) {
            return View(model);
        }
    }
}