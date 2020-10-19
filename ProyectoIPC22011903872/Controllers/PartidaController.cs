using ProyectoIPC22011903872.Models.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using ProyectoIPC22011903872.Models;

namespace ProyectoIPC22011903872.Controllers
{
    public class PartidaController : Controller
    {
        public static DatosModel modelo = new DatosModel();
        public static PartidaViewModel PartidaCargada = new PartidaViewModel();
        public static bool cargar = false;
        // GET: Partida
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
        public ActionResult Xtream()
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
        public ActionResult Xtream(string mensaje)
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
        public ActionResult Partida(List<string> color1, List<string> color2, string jugador2, int N, int M, string Tpartida, string modalidad, string apertura)
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
            var partida = new PartidaViewModel();
            if (Tpartida == "Normal" && !cargar)
            {
                indice = 0;
                if (color1.Count() == 0)
                {
                    indice = rnd.Next(ran.Length);
                    color1.Add(ran[indice]);
                    if (color1[0] == "negro")
                    {
                        color2.Add("blanco");
                    }
                    else
                    {
                        color2.Add("negro");
                    }
                }
            }
            if (!cargar)
            {
                partida = Funciones.CrearPartida(modelo, color1, color2, siguiente, user.Usuario1, jugador2, N, M, Tpartida,modalidad,apertura);
            }
            else
            {
                partida = PartidaCargada;
                cargar = false;
                PartidaCargada = new PartidaViewModel();
            }
            if (partida.jugador2.Contains(partida.siguiente_tiro) && partida.tipo == "M")
            {
                ViewBag.maquina = true;
            }
            bool[] tiros = { true, true };
            if (partida.centro)
            {
                tiros = Funciones.CantidadMovimientos(partida);
            }
            if ((!tiros[0] && tiros[1] && partida.tipo != "M") || (partida.siguiente_tiro == "M" && !tiros[0] && tiros[1] && partida.colores_jugador2.Contains(partida.siguiente_tiro)))
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
                        par.Punteo_1 = partida.punteo_jugador1;
                        par.Punteo_2 = partida.punteo_jugador2;
                        par.Codigo_Usuario_1 = user.Codigo_Usuario;
                        par.movimientos_1 = partida.movimientos_1;
                        par.movimientos_2 = partida.movimientos_2;
                        par.Tiempo_Jugador1 = partida.Ttiempo1;
                        par.Tiempo_Jugador2 = partida.Ttiempo2;
                        if (partida.modalidad=="Normal")
                        {
                            par.MODO = 2;
                        }
                        else
                        {
                            par.MODO = 1;
                        }
                        if (partida.TipoPartida=="Normal")
                        {
                            par.TIPO = 2;
                        }
                        else
                        {
                            par.TIPO = 1;
                        }
                        if (partida.tipo=="M")
                        {
                            par.CONTRINCANTE = 1;
                        }
                        else
                        {
                            par.CONTRINCANTE = 2;
                        }
                        if ((partida.punteo_jugador1 > partida.punteo_jugador2 && partida.modalidad=="Normal")|| (partida.punteo_jugador1 < partida.punteo_jugador2 && partida.modalidad == "Inversa"))
                        {
                            par.ResultadoLocal = "Ganador";
                        }
                        else if ((partida.punteo_jugador1 < partida.punteo_jugador2 && partida.modalidad == "Normal") || (partida.punteo_jugador1 > partida.punteo_jugador2 && partida.modalidad == "Inversa"))
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
        public ActionResult Partida(int nom, string fila, string columna, PartidaViewModel pr)
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user = (USUARIO)this.Session["user"];
            var Registrar = true;
            ViewBag.mensaje = "funciono";
            var partida = new PartidaViewModel();
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
                Random rnd = new Random();
                int indice = 0;
                if (movi.Count > 0)
                {
                    
                    if (partida.centro)
                    {
                        List<object[]> filasActual = new List<object[]>();
                        List<object[]> Ultima = new List<object[]>();
                        int punteo1actual = partida.punteo_jugador1;
                        int punto2actual = partida.punteo_jugador2;
                        int movimiento1actual = partida.movimientos_1;
                        int movimiento2actual = partida.movimientos_2;
                        int punteo1final = 0;
                        var Act2 = partida.colorA2;
                        var sigT = Act2;
                        int punteomayor = 0;
                        if (partida.modalidad=="Inversa")
                        {
                            punteomayor = 100;
                        }
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
                            partida = Funciones.AgregarFicha(partida, mov[0], mov[1]);
                            if ((punteomayor < partida.punteo_jugador2 && partida.modalidad=="Normal") || (punteomayor > partida.punteo_jugador2 && partida.modalidad == "Inversa"))
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
                        model = Funciones.AgregarFicha(partida, fila, columna);
                        partida = Funciones.Movimientos(partida);
                    }
                    else
                    {
                        indice = rnd.Next(movi.Count);
                        fila = movi[indice][0];
                        columna = movi[indice][1];
                        model = Funciones.AgregarFicha(partida, fila, columna);
                    }
                    partida = Funciones.Punteos(partida);
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
                model = Funciones.AgregarFicha(partida, fila, columna);
                partida.ultimacolumna = columna;
                partida.ultimafila = fila;
            }
            else if (sig)
            {
                partida.ultimacolumna = "";
                partida.ultimafila = "";
            }
            ViewBag.partida = partida;
            bool[] tiros = { true, true };
            if (partida.centro)
            {
                tiros = Funciones.CantidadMovimientos(partida);
            }
            if ((!tiros[0] && tiros[1] && partida.tipo != "M") || (partida.siguiente_tiro == "M" && !tiros[0] && tiros[1] && !partida.jugador2.Contains(partida.siguiente_tiro)))
            {
                ViewBag.saltar = true;
            }
            else if (!tiros[0] && tiros[1] && partida.tipo == "M" && partida.jugador2.Contains(partida.siguiente_tiro))
            {
                partida.siguiente_tiro = partida.colorA1;
                partida = Funciones.Movimientos(partida);
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
                        par.Tiempo_Jugador1 = partida.Ttiempo1;
                        par.Tiempo_Jugador2 = partida.Ttiempo2;
                        if (partida.modalidad == "Normal")
                        {
                            par.MODO = 2;
                        }
                        else
                        {
                            par.MODO = 1;
                        }
                        if (partida.TipoPartida == "Normal")
                        {
                            par.TIPO = 2;
                        }
                        else
                        {
                            par.TIPO = 1;
                        }
                        if (partida.tipo == "M")
                        {
                            par.CONTRINCANTE = 1;
                        }
                        else
                        {
                            par.CONTRINCANTE = 2;
                        }
                        if ((partida.punteo_jugador1 > partida.punteo_jugador2 && partida.modalidad == "Normal") || (partida.punteo_jugador1 < partida.punteo_jugador2 && partida.modalidad == "Inversa"))
                        {
                            par.ResultadoLocal = "Ganador";
                        }
                        else if ((partida.punteo_jugador1 < partida.punteo_jugador2 && partida.modalidad == "Normal") || (partida.punteo_jugador1 > partida.punteo_jugador2 && partida.modalidad == "Inversa"))
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
        [HttpPost]
        public ActionResult CargarPartida(HttpPostedFileBase archivo, string jugador22, string Tpartida)
        {
            var apertura = "Normal";
            var N = 0;
            var M = 0;
            var p = 0;
            var modalidad = "";
            List<string> ColoresValidos = new List<string>(){"blanco", "negro", "rojo", "cafe", "celeste", "naranja", "morado", "rosado", "amarillo", "verde", "marron", "purpura", "turquesa", "cyan", "salmon" , "azul"};
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var Filename = Path.GetFileName(archivo.FileName);
            var path = Path.Combine(Server.MapPath("~/Public/XML"), Filename);
            archivo.SaveAs(path);
            XmlDocument documento = new XmlDocument();
            documento.Load(path);
            List<string> color11 = new List<string>();
            List<string> color22 = new List<string>();
            USUARIO user = (USUARIO)this.Session["user"];

            /*Colores*/
            foreach (XmlNode node in documento.SelectNodes("/partida/Jugador1/color"))
            {
                p++;
                var color = node.InnerText;
                if (!ColoresValidos.Contains(color))
                {
                    if (Tpartida == "Normal")
                    {
                        return RedirectToAction("Index", "Partida", new { mensaje = "Error colores Invalidos" });
                    }
                    else
                    {
                        return RedirectToAction("Xtream", "Partida", new { mensaje = "Error colores Invalidos" });
                    }
                }
                else if (color11.Contains(color))
                {
                    if (Tpartida=="Normal")
                    {
                        return RedirectToAction("Index", "Partida", new { mensaje = "Error colores repetidos" });
                    }
                    else
                    {
                        return RedirectToAction("Xtream", "Partida", new { mensaje = "Error colores repetidos" });
                    }
                }
                else
                {
                    color11.Add(color);
                }
            }
            if (p==0)
            {
                if (Tpartida=="Normal")
                {
                    return RedirectToAction("Index", "Partida", new { mensaje = "Error en estructura de la partida" });
                }
                else
                {
                    return RedirectToAction("Xtream", "Partida", new { mensaje = "Error en estructura de la partida" });
                }
            }
            p = 0;
            foreach (XmlNode node in documento.SelectNodes("/partida/Jugador2/color"))
            {
                p++;
                var color = node.InnerText;
                if (!ColoresValidos.Contains(color))
                {
                    if (Tpartida == "Normal")
                    {
                        return RedirectToAction("Index", "Partida", new { mensaje = "Error colores Invalidos" });
                    }
                    else
                    {
                        return RedirectToAction("Xtream", "Partida", new { mensaje = "Error colores Invalidos" });
                    }
                }
                else if(color11.Contains(color) || color22.Contains(color))
                {
                    if (Tpartida == "Normal")
                    {
                        return RedirectToAction("Index", "Partida", new { mensaje = "Error colores repetidos" });
                    }
                    else
                    {
                        return RedirectToAction("Xtream", "Partida", new { mensaje = "Error colores repetidos" });
                    }
                }
                else
                {
                    color22.Add(color);
                }
            }
            if (p == 0)
            {
                if (Tpartida == "Normal")
                {
                    return RedirectToAction("Index", "Partida", new { mensaje = "Error en estructura de la partida" });
                }
                else
                {
                    return RedirectToAction("Xtream", "Partida", new { mensaje = "Error en estructura de la partida" });
                }
            }
            if (Tpartida == "Normal")
            {
                if ((color11.Count()!=1 && color22.Count()!=1) && (!color11.Contains("negro") || !color11.Contains("blanco") || !color22.Contains("negro") || !color22.Contains("blanco")))
                {
                    return RedirectToAction("Index", "Partida", new { mensaje = "Error en estructura de la partida" });
                }
            }
            
            /**/
            if (Tpartida == "Normal")
            {
                N = 8;
                M = 8;
            }
            else
            {
                foreach (XmlNode node in documento.SelectNodes("/partida"))
                {
                    if (N!=0 || M!=0)
                    {
                        return RedirectToAction("Xtream", "Partida", new { mensaje = "Error en estructura de la partida" });
                    }
                    try
                    {
                        N = int.Parse(node["filas"].InnerText);
                        M = int.Parse(node["columnas"].InnerText);
                    }
                    catch(Exception e)
                    {
                        if (Tpartida=="Xtream")
                        {
                            return RedirectToAction("Xtream", "Partida", new { mensaje = "Error en estructura de la partida" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Partida", new { mensaje = "Error en estructura de la partida" });
                        }
                    }

                }
            }
            foreach (XmlNode node in documento.SelectNodes("/partida"))
            {
                if (modalidad!="")
                {
                    if (Tpartida=="Normal")
                    {
                        return RedirectToAction("Index", "Partida", new { mensaje = "Error en estructura de la partida" });
                    }
                    else
                    {
                        return RedirectToAction("Xtream", "Partida", new { mensaje = "Error en estructura de la partida" });
                    }
                }
                modalidad = node["Modalidad"].InnerText;
            }
            if (modalidad=="")
            {
                if (Tpartida == "Normal")
                {
                    return RedirectToAction("Index", "Partida", new { mensaje = "Error en estructura de la partida" });
                }
                else
                {
                    return RedirectToAction("Xtream", "Partida", new { mensaje = "Error en estructura de la partida" });
                }
            }
            PartidaCargada = Funciones.CrearPartida(modelo, color11, color22, "", user.Usuario1, jugador22, N, M, Tpartida, modalidad,apertura);
            PartidaCargada.movimientos_1 = 0;
            PartidaCargada.movimientos_2 = 0;
            PartidaCargada.punteo_jugador1 = 0;
            PartidaCargada.punteo_jugador2 = 0;
            PartidaCargada.siguiente_tiro = "";
            PartidaCargada.terminado = "";
            string[] le =  { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R" ,"S", "T"};
            string[] nu = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"};
            
            List<string> letras = new List<string>();
            List<string> numeros = new List<string>();
            for (int i = 0; i < N; i++)
            {
                numeros.Add(nu[i]);
            }
            for (int i = 0; i < M; i++)
            {
                letras.Add(le[i]);
            }
            foreach (var fil in PartidaCargada.Filas)
            {
                foreach (var col in fil.columnas)
                {
                    col.color = "b";
                }
            }
            /*Fichas*/
            foreach (XmlNode node in documento.SelectNodes("/partida/tablero/ficha"))
            {
                var f = node["fila"].InnerText;
                var c = node["columna"].InnerText;
                var color = node["color"].InnerText;
                var CF = numeros.Contains(f);
                var CC = letras.Contains(c);
                if (CC && CF)
                {
                    foreach (var fila in PartidaCargada.Filas)
                    {
                        foreach (var columna in fila.columnas)
                        {
                            if (c == columna.nombre && f == fila.nombre && columna.color == "b")
                            {
                                columna.color = color;
                            }
                            else if (c == columna.nombre && f == fila.nombre)
                            {
                                if (Tpartida == "Normal")
                                {
                                    return RedirectToAction("Index", "Partida", new { mensaje = "Error hay posiciones repetidas en su partida" });
                                }
                                else
                                {
                                    return RedirectToAction("Xtream", "Partida", new { mensaje = "Error hay posiciones repetidas en su partida" });
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Tpartida == "Normal")
                    {
                        return RedirectToAction("Index", "Partida", new { mensaje = "Error hay columnas y filas que no existen en una partida" });
                    }
                    else
                    {
                        return RedirectToAction("Xtream", "Partida", new { mensaje = "Error hay columnas y filas que no existen en una partida" });
                    }
                    
                }
            }
            /*Siguiente tiro*/
            foreach (XmlNode node in documento.SelectNodes("/partida/tablero/siguienteTiro"))
            {
                if (PartidaCargada.siguiente_tiro == "")
                {
                    PartidaCargada.siguiente_tiro = node["color"].InnerText;
                }
                else
                {
                    if (Tpartida == "Normal")
                    {
                        return RedirectToAction("Index", "Partida", new { mensaje = "Error el turno siguiente esta repetido en su partida" });
                    }
                    else
                    {
                        return RedirectToAction("Xtream", "Partida", new { mensaje = "Error el turno siguiente esta repetido en su partida" });
                    }
                    
                }
            }
            /*siguiente tiro*/
            if (PartidaCargada.colores_jugador1.Contains(PartidaCargada.siguiente_tiro))
            {
                PartidaCargada.colorA1 = PartidaCargada.siguiente_tiro;
                PartidaCargada.colorA2 = PartidaCargada.colores_jugador2[0];
                PartidaCargada.colores_contrario = PartidaCargada.colores_jugador1;
            }
            else
            {
                PartidaCargada.colorA2 = PartidaCargada.siguiente_tiro;
                PartidaCargada.colorA1 = PartidaCargada.colores_jugador1[0];
                PartidaCargada.colores_contrario = PartidaCargada.colores_jugador2;
            }
            /*Verificar centro*/
            if (Tpartida=="Xtream")
            {
                var centrof1 = (N / 2) - 1;
                var centroc1 = (M / 2) - 1;
                var centrof2 = (N / 2);
                var centroc2 = (M / 2);
                var ficha1 = PartidaCargada.Filas[centrof1].columnas[centroc1].color;
                var ficha2 = PartidaCargada.Filas[centrof1].columnas[centroc2].color;
                var ficha3 = PartidaCargada.Filas[centrof2].columnas[centroc1].color;
                var ficha4 = PartidaCargada.Filas[centrof2].columnas[centroc2].color;
                if (ficha1!="" && ficha1!= "b" && ficha2 != "" && ficha2 != "b" && ficha3 != "" && ficha3 != "b" && ficha4 != "" && ficha4 != "b")
                {
                    PartidaCargada.centro = true;
                    PartidaCargada = Funciones.Movimientos(PartidaCargada);
                }
                else
                {
                    apertura = "Personalizada";
                    PartidaCargada.apertura = apertura;
                    PartidaCargada.centro = false;
                    if (ficha1=="b")
                    {
                        PartidaCargada.Filas[centrof1].columnas[centroc1].color = "";
                    }
                    if (ficha2 == "b")
                    {
                        PartidaCargada.Filas[centrof1].columnas[centroc2].color = "";
                    }
                    if (ficha3 == "b")
                    {
                        PartidaCargada.Filas[centrof2].columnas[centroc1].color = "";
                    }
                    if (ficha4 == "b")
                    {
                        PartidaCargada.Filas[centrof2].columnas[centroc2].color = "";
                    }
                }
            }
            else
            {
                PartidaCargada = Funciones.Movimientos(PartidaCargada);
            }
            object[] objeto = Funciones.ComprobarCasillas(PartidaCargada);
            bool respuesta = (bool)objeto[1];
            if (respuesta)
            {
                PartidaCargada = (PartidaViewModel)objeto[0];
                if (PartidaCargada.centro)
                {
                    PartidaCargada = Funciones.Movimientos(PartidaCargada);
                }
                PartidaCargada = Funciones.Punteos(PartidaCargada);
                cargar = true;
                return RedirectToAction("Partida", "Partida", new { N = 8, M = 8 });
            }
            if (Tpartida == "Normal")
            {
                return RedirectToAction("Index", "Partida", new { mensaje = "Error en estructura de la partida" });
            }
            else
            {
                return RedirectToAction("Xtream", "Partida", new { mensaje = "Error en estructura de la partida" });
            }
            
        }

        public ActionResult GuardarPartida(int nombre)
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            PartidaViewModel partida = new PartidaViewModel();
            foreach (var parti in modelo.partidas)
            {
                if (parti.nombre == nombre)
                {
                    partida = parti;
                    break;
                }
            }
            USUARIO user = (USUARIO)this.Session["user"];
            string Filename = "";
            if (partida.TipoPartida=="Xtream")
            {
                Filename = user.Usuario1 + "_PartidaXtream_#" + partida.nombre + ".xml";
            }
            else
            {
                Filename = user.Usuario1 + "_Partida_#" + partida.nombre + ".xml";
            }
            var path = Path.Combine(Server.MapPath("~/Public/XML_SAVE"), Filename);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            XDocument documento = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement Raiz = new XElement("tablero");
            XElement part = new XElement("partida");
            XElement alto = new XElement("filas",partida.N);
            XElement ancho = new XElement("columnas", partida.M);
            XElement mod = new XElement("Modalidad", partida.modalidad);
            XElement Cj1 = new XElement("Jugador1");
            XElement Cj2 = new XElement("Jugador2");

            documento.Add(part);
            part.Add(alto);
            part.Add(ancho);
            part.Add(Cj1);
            part.Add(Cj2);
            part.Add(mod);
            foreach (var color1 in partida.colores_jugador1)
            {
                XElement c = new XElement("color", color1);
                Cj1.Add(c);
            }
            foreach (var color1 in partida.colores_jugador2)
            {
                XElement c = new XElement("color", color1);
                Cj2.Add(c);
            }
            foreach (var fila in partida.Filas)
            {
                foreach (var columna in fila.columnas)
                {
                    if (!string.IsNullOrEmpty(columna.color) && columna.color != "b")
                    {
                        XElement ficha = new XElement("ficha");
                        XElement colors = new XElement("color", columna.color);
                        XElement col = new XElement("columna", columna.nombre);
                        XElement fil = new XElement("fila", fila.nombre);
                        ficha.Add(colors);
                        ficha.Add(col);
                        ficha.Add(fil);
                        Raiz.Add(ficha);
                    }
                }
            }
            XElement siguiente = new XElement("siguienteTiro");
            XElement color = new XElement("color", partida.siguiente_tiro);
            siguiente.Add(color);
            Raiz.Add(siguiente);
            part.Add(Raiz);
            documento.Save(path);
            if (partida.TipoPartida=="Normal")
            {
                return RedirectToAction("Index", "Partida", new { mensaje = "Partida Guardada" });
            }
            else
            {
                return RedirectToAction("Xtream", "Partida", new { mensaje = "Partida Guardada" });
            }
        }
        [HttpPost]
        public ActionResult Cronometro1(int nombre)
        {
            var partida = new PartidaViewModel();
            foreach (var part in modelo.partidas)
            {
                if (part.nombre == nombre)
                {
                    partida = part;
                    break;
                }
            }
            if (partida.pt1 && partida.terminado=="")
            {
                partida.tiempoJ1++;
            }
            
            partida.Ttiempo1 = Funciones.CalcularTiempo(partida.tiempoJ1);
            ViewBag.tiempo = partida.Ttiempo1;
            if (partida.colores_jugador1.Contains(partida.siguiente_tiro))
            {
                if (!partida.pt1)
                {
                    partida.pt1 = true;
                    partida.pt2 = false;
                }
            }
            return View("_Cronometro",partida);
        }
        [HttpPost]
        public ActionResult Cronometro2(int nombre)
        {
            ViewBag.j = "2";
            var partida = new PartidaViewModel();
            foreach (var part in modelo.partidas)
            {
                if (part.nombre == nombre)
                {
                    partida = part;
                    break;
                }
            }
            if (partida.pt2 && partida.terminado == "")
            {
                partida.tiempoJ2++;
            }
            partida.Ttiempo2 = Funciones.CalcularTiempo(partida.tiempoJ2);
            ViewBag.tiempo = partida.Ttiempo2;
            if (partida.colores_jugador2.Contains(partida.siguiente_tiro))
            {
                if (!partida.pt2)
                { 
                    partida.pt2 = true;
                    partida.pt1 = false;
                }
            }
            return View("_Cronometro", partida);
        }
    }

}
