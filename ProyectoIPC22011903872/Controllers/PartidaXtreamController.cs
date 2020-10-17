using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoIPC22011903872.Models.ViewModels;
using System.Web.Mvc;
using ProyectoIPC22011903872.Models;

namespace ProyectoIPC22011903872.Controllers
{
    public class PartidaXtreamController : Controller
    {
        public static DatosXtreamModel modelo = new DatosXtreamModel();
        public static PartidaXtreamViewModel PartidaCargada = new PartidaXtreamViewModel();
        public static bool cargar = false;
        // GET: PartidaXtream
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.user = this.Session["user"];
            ViewBag.mensaje = "";
            return View();
        }

        [HttpGet]
        public ActionResult Index(string mensaje)
        {
            ViewBag.mensaje = mensaje;
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.user = this.Session["user"];
            return View();
        }

        [HttpGet]
        public ActionResult Partida(List<string> color1, List<string> color2, string jugador2, int N, int M)
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user = (USUARIO)this.Session["user"];
            string[] ran = { "negro", "blanco" };
            Random rnd = new Random();
            int indice = 0;
            indice = rnd.Next(ran.Length);
            string siguiente = ran[indice];
            siguiente = "negro";
            var partida = new PartidaXtreamViewModel();
            color1 = new List<string>() { 
                "amarillo","negro"
            };
            color2 = new List<string>() {
                "rosado","celeste"
            };
            if (!cargar)
            {
                partida = FuncionesXtream.CrearPartida(modelo, color1, color2, siguiente, user.Usuario1, jugador2,N,M);
            }
            else
            {
                partida = PartidaCargada;
                cargar = false;
                PartidaCargada = new PartidaXtreamViewModel();
            }
            if (partida.jugador2.Contains(partida.siguiente_tiro) && partida.tipo == "M")
            {
                ViewBag.maquina = true;
            }
            bool[] tiros = { true, true };
            if (partida.centro)
            {
                tiros = FuncionesXtream.CantidadMovimientos(partida);
            }
            if ((!tiros[0] && !tiros[1] && partida.tipo != "M") || (partida.siguiente_tiro == "M" && !tiros[0] && !tiros[1] && partida.colores_jugador2.Contains(partida.siguiente_tiro)))
            {
                ViewBag.saltar = true;
            }
            else if (!tiros[0] && !tiros[1])
            {
                ViewBag.terminado = true;
                if (partida.terminado == "")
                {
                    using (OthelloEntities db = new OthelloEntities())
                    {
                        var par = new PARTIDA();
                        par.Codigo_Usuario_1 = 1;
                        par.Fecha = DateTime.Now;
                        par.TIPO = 2;
                        par.Punteo_1 = partida.punteo_jugador1;
                        par.Punteo_2 = partida.punteo_jugador2;
                        par.Codigo_Usuario_1 = user.Codigo_Usuario;
                        par.movimientos_1 = partida.movimientos_1;
                        par.movimientos_2 = partida.movimientos_2;
                        if (partida.punteo_jugador1 > partida.punteo_jugador2)
                        {
                            par.ResultadoLocal = "Ganador";
                        }
                        else if (partida.punteo_jugador2 > partida.punteo_jugador1)
                        {
                            par.ResultadoLocal = "Perdedor";
                        }
                        else
                        {
                            par.ResultadoLocal = "Empate";
                        }
                        db.PARTIDA.Add(par);
                        db.SaveChanges();
                        partida.terminado = "si";
                    }
                }
            }
            ViewBag.mensaje = "partida";
            ViewBag.partida = partida;
            return View(partida);
        }

        [HttpPost]
        public ActionResult Partida(int nom, string fila, string columna, PartidaXtreamViewModel pr)
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user = (USUARIO)this.Session["user"];
            var Registrar = true;
            ViewBag.mensaje = "funciono";
            var partida = new PartidaXtreamViewModel();
            foreach (var part in modelo.partidas)
            {
                if (part.nombre == nom)
                {
                    partida = part;
                    foreach (var fil in partida.Filas)
                    {
                        foreach (var col in fil.columnas)
                        {
                            if (fil.nombre == fila && col.nombre == columna)
                            {
                                if (col.color != "")
                                {
                                    Registrar = false;
                                    break;
                                }
                            }
                        }
                        if (!Registrar)
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            var model = partida;
            var sig = true;
            if (partida.colores_jugador2.Contains(partida.siguiente_tiro) && partida.tipo == "M")
            {
                List<List<string>> movi = new List<List<string>>();
                foreach (var fil in partida.Filas)
                {
                    foreach (var col in fil.columnas)
                    {
                        if (col.color == "")
                        {
                            List<string> fc = new List<string> { fil.nombre, col.nombre };
                            movi.Add(fc);
                        }
                    }
                }
                /*Random rnd = new Random();
                int indice = 0;*/
                if (movi.Count > 0)
                {
                    /*indice = rnd.Next(movi.Count);
                    fila = movi[indice][0];
                    columna = movi[indice][1];*/
                    List<object[]> filasActual = new List<object[]>();
                    List<object[]> Ultima = new List<object[]>();
                    int punteo1actual = partida.punteo_jugador1;
                    int punto2actual = partida.punteo_jugador2;
                    int movimiento1actual = partida.movimientos_1;
                    int movimiento2actual = partida.movimientos_2;
                    int punteo1final = 0;
                    var Act2=partida.colorA2;
                    var sigT = Act2;
                    int punteomayor = 0;
                    foreach (var ff in partida.Filas)
                    {
                        List<string[]> colu = new List<string[]>();
                        foreach (var cc in ff.columnas)
                        {
                            string[] co = { cc.color, cc.nombre };
                            colu.Add(co);
                        }
                        object[] fi = { ff.nombre, colu };
                        filasActual.Add(fi);
                    }
                    foreach (var mov in movi)
                    {
                        partida = FuncionesXtream.AgregarFicha(partida, mov[0], mov[1]);
                        if (punteomayor < partida.punteo_jugador2)
                        {
                            punteomayor = partida.punteo_jugador2;
                            punteo1final = partida.punteo_jugador1;
                            Ultima = new List<object[]>();
                            fila = mov[0];
                            columna = mov[1];
                            foreach (var ff in partida.Filas)
                            {
                                List<string[]> colu = new List<string[]>();
                                foreach (var cc in ff.columnas)
                                {
                                    string[] co = { cc.color, cc.nombre };
                                    colu.Add(co);
                                }
                                object[] fi = { ff.nombre, colu };
                                Ultima.Add(fi);
                            }

                        }
                        partida.punteo_jugador2 = punto2actual;
                        partida.siguiente_tiro = partida.colorA2;
                        partida.punteo_jugador1 = punteo1actual;
                        partida.movimientos_1 = movimiento1actual;
                        partida.movimientos_2 = movimiento2actual;
                        partida.colorA2 = Act2;
                        partida.siguiente_tiro = sigT;
                        partida.Filas = new List<FilaViewModel>();
                        foreach (var ff in filasActual)
                        {
                            FilaViewModel fff = new FilaViewModel();
                            foreach (var cc in (List<string[]>)ff[1])
                            {
                                ColumnaViewModel co = new ColumnaViewModel();
                                co.color = cc[0];
                                co.nombre = cc[1];
                                fff.columnas.Add(co);
                            }
                            fff.nombre = ff[0].ToString();
                            partida.Filas.Add(fff);
                        }

                    }
                    partida.punteo_jugador2 = punteomayor;
                    partida.Filas = new List<FilaViewModel>();
                    foreach (var ff in Ultima)
                    {
                        FilaViewModel fff = new FilaViewModel();
                        foreach (var cc in (List<string[]>)ff[1])
                        {
                            ColumnaViewModel co = new ColumnaViewModel();
                            co.color = cc[0];
                            co.nombre = cc[1];
                            fff.columnas.Add(co);
                        }
                        fff.nombre = ff[0].ToString();
                        partida.Filas.Add(fff);
                    }
                    partida.colorA2 = Act2;
                    partida.siguiente_tiro = sigT;
                    partida.ultimafila = fila;
                    partida.ultimacolumna = columna;
                    partida = FuncionesXtream.Movimientos(partida);
                    partida = FuncionesXtream.Punteos(partida);
                    model = partida;
                    Registrar = false;
                    sig = false;

                }
                else
                {
                    fila = "saltar";
                }
            }
            if (Registrar)
            {
                model = FuncionesXtream.AgregarFicha(partida, fila, columna);
                partida.ultimacolumna = columna;
                partida.ultimafila = fila;
            }
            else if (sig)
            {
                partida.ultimacolumna = "";
                partida.ultimafila = "";
            }
            ViewBag.partida = partida;
            bool[] tiros = { true,true};
            if (partida.centro)
            {
                tiros = FuncionesXtream.CantidadMovimientos(partida);
            }
            if ((!tiros[0] && tiros[1] && partida.tipo != "M") || (partida.siguiente_tiro == "M" && !tiros[0] && tiros[1] && !partida.jugador2.Contains(partida.siguiente_tiro)))
            {
                ViewBag.saltar = true;
            }
            else if (!tiros[0] && tiros[1] && partida.tipo == "M" && partida.jugador2.Contains(partida.siguiente_tiro))
            {
                partida.siguiente_tiro = partida.colorA1;
                partida = FuncionesXtream.Movimientos(partida);
            }
            else if (!tiros[0] && !tiros[1])
            {
                ViewBag.terminado = true;
                if (partida.terminado == "")
                {
                    using (OthelloEntities db = new OthelloEntities())
                    {
                        var par = new PARTIDA();
                        par.Codigo_Usuario_1 = 1;
                        par.Fecha = DateTime.Now;
                        par.TIPO = 2;
                        par.Punteo_1 = partida.punteo_jugador1;
                        par.Punteo_2 = partida.punteo_jugador2;
                        par.Codigo_Usuario_1 = user.Codigo_Usuario;
                        par.movimientos_1 = partida.movimientos_1;
                        par.movimientos_2 = partida.movimientos_2;
                        if (partida.punteo_jugador1 > partida.punteo_jugador2)
                        {
                            par.ResultadoLocal = "Ganador";
                        }
                        else if (partida.punteo_jugador2 > partida.punteo_jugador1)
                        {
                            par.ResultadoLocal = "Perdedor";
                        }
                        else
                        {
                            par.ResultadoLocal = "Empate";
                        }
                        db.PARTIDA.Add(par);
                        db.SaveChanges();
                        partida.terminado = "si";
                    }
                }
            }
            else if (partida.colores_jugador2.Contains(partida.siguiente_tiro) && partida.tipo == "M")
            {
                ViewBag.maquina = true;
            }

            return View(model);
        }

    }
}