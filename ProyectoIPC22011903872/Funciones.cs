using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoIPC22011903872.Models.ViewModels;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace ProyectoIPC22011903872
{
    public static class Funciones
    {

        public static string IsActive(this HtmlHelper html, string control, string action)
        {
            var routeData = html.ViewContext.RouteData;
            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];
            var returnActive = control == routeControl && action == routeAction;
            return returnActive ? "active" : "joo";
        }

        public static PartidaViewModel CrearPartida(DatosModel dato){
            PartidaViewModel partida = new PartidaViewModel();
            string[] columnas = { "A", "B", "C", "D", "E", "F", "G", "H" };
            partida.nombre = dato.nombre + 1;
            partida.jugador1 = "jugador1";
            partida.jugador2 = "jugador2";
            partida.color_jugador1 = "negro";
            partida.color_jugador2 = "blanco";
            partida.movimientos_1 = 0;
            partida.movimientos_2 = 0;
            partida.punteo_jugador1 = 2;
            partida.punteo_jugador2 = 2;
            partida.siguiente_tiro = "blanco";
            for (int i = 1; i <= 8; i++)
            {
                FilaViewModel fila = new FilaViewModel();
                fila.nombre = i.ToString();
                for (int j = 0; j < columnas.Length; j++)
                {
                    ColumnaViewModel col = new ColumnaViewModel();
                    col.color = "";
                    col.nombre = columnas[j];

                    if (j == 3 && i == 4)
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
            dato.partidas.Add(partida);
            dato.nombre++;
            return partida;
        }
        
        public static PartidaViewModel AgregarFicha(PartidaViewModel partida,string fila,string columna)
        {
            var color = partida.siguiente_tiro;
            if (color=="blanco")
            {
                partida.siguiente_tiro = "negro";
            }
            else
            {
                partida.siguiente_tiro = "blanco";
            }
            if (partida.color_jugador1 == color)
            {
                partida.punteo_jugador1++;
            }
            else
            {
                partida.punteo_jugador2++;
            }
            foreach (var fil in partida.Filas)
            {
                foreach (var col in fil.columnas)
                {
                    if (col.nombre == columna && fil.nombre == fila)
                    {
                        col.color = color;
                    }
                }
            }
            return partida;
        }
    }
}