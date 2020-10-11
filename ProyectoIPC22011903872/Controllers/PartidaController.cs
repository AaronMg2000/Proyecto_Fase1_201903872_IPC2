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
        public static bool cargar=false;
        // GET: Partida
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.mensaje = "";
            return View();
        }
        [HttpGet]
        public ActionResult Index(string mensaje) {
            ViewBag.mensaje = mensaje;
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Partida(string color1,string color2,string jugador2)
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user =(USUARIO) this.Session["user"];
            string[] ran = { "negro", "blanco" };
            Random rnd = new Random();
            int indice = 0;
            if (color1=="" || color1==null)
            {
                indice = rnd.Next(ran.Length);
                color1 = ran[indice];
                
                if (color1 == "negro")
                {
                    color2 = "blanco";
                }
                else
                {
                    color2 = "negro";
                }
            }
            indice = rnd.Next(ran.Length);
            string siguiente = ran[indice];
            var partida = new PartidaViewModel();
            if (!cargar)
            {
                partida = Funciones.CrearPartida(modelo,color1,color2,siguiente,user.Usuario1,jugador2);
            }
            else
            {
                partida = PartidaCargada;
                cargar = false;
                PartidaCargada = new PartidaViewModel();
            }
            if (partida.siguiente_tiro == partida.color_jugador2 && partida.tipo=="M")
            {
                ViewBag.maquina = true;
            }
            bool[] tiros = Funciones.CantidadMovimientos(partida);
            if ((!tiros[0] && tiros[1] && partida.tipo!="M") || (partida.siguiente_tiro=="M" && !tiros[0] && tiros[1] && partida.siguiente_tiro!=partida.color_jugador2))
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
                            par.Ganador = user.Codigo_Usuario;
                        }
                        else
                        {
                            par.Ganador = 0;
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
        public ActionResult Partida(int nom, string fila, string columna,PartidaViewModel pr)
        {
            if (!User.Identity.IsAuthenticated && this.Session["user"] == null)
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
                        foreach(var col in fil.columnas)
                        {
                            if (fil.nombre==fila && col.nombre == columna)
                            {
                                if (col.color != ""){
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
            var model=partida;
            var sig = true;
            if(partida.color_jugador2==partida.siguiente_tiro && partida.tipo == "M")
            {   
                List<List<string>> movi = new List<List<string>>();
                foreach (var fil in partida.Filas)
                {
                    foreach(var col in fil.columnas)
                    {
                        if (col.color == "")
                        {
                            List<string> fc = new List<string> {fil.nombre,col.nombre};
                            movi.Add(fc);
                        }
                    }
                }
                /*Random rnd = new Random();
                int indice = 0;*/
                if (movi.Count >0)
                {
                    /*indice = rnd.Next(movi.Count);
                    fila = movi[indice][0];
                    columna = movi[indice][1];*/
                    List<object[]> filasActual = new List<object[]>();
                    List<object[]> Ultima = new List<object[]>();
                    int punteo1actual=partida.punteo_jugador1;
                    int punto2actual=partida.punteo_jugador2;
                    int movimiento1actual = partida.movimientos_1;
                    int movimiento2actual = partida.movimientos_2;
                    int punteo1final = 0;
                    int punteomayor=0;
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
                    foreach(var mov in movi)
                    {
                        partida = Funciones.AgregarFicha(partida, mov[0], mov[1]);
                        if (punteomayor < partida.punteo_jugador2)
                        {
                            punteomayor = partida.punteo_jugador2;
                            punteo1final = partida.punteo_jugador1;
                            Ultima = new List<object[]>();
                            fila = mov[0];
                            columna = mov[1];
                            foreach(var ff in partida.Filas)
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
                        partida.siguiente_tiro = partida.color_jugador2;
                        partida.punteo_jugador1 = punteo1actual;
                        partida.movimientos_1 = movimiento1actual;
                        partida.movimientos_2 = movimiento2actual;
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
                    partida.siguiente_tiro = partida.color_jugador1;
                    partida.ultimafila = fila;
                    partida.ultimacolumna = columna;
                    partida = Funciones.Movimientos(partida);
                    partida = Funciones.Punteos(partida);
                    model = partida;
                    Registrar = false;
                    sig = false;

                }
            }
            if (Registrar){ 
                model = Funciones.AgregarFicha(partida, fila, columna);
                partida.ultimacolumna = columna;
                partida.ultimafila = fila;
            }
            else if(sig)
            {
                 partida.ultimacolumna = "";
                 partida.ultimafila = "";
            }
            ViewBag.partida = partida;

            bool[] tiros = Funciones.CantidadMovimientos(partida);
            if ((!tiros[0] && tiros[1] && partida.tipo != "M") || (partida.siguiente_tiro == "M" && !tiros[0] && tiros[1] && partida.siguiente_tiro != partida.color_jugador2))
            {
                ViewBag.saltar = true;
            }
            else if (!tiros[0] && tiros[1] && partida.tipo=="M" && partida.siguiente_tiro==partida.color_jugador2)
            {
                partida.siguiente_tiro = partida.color_jugador1;
                partida = Funciones.Movimientos(partida);
            }
            else if (!tiros[0] && !tiros[1])
            {
                ViewBag.terminado = true;
                if (partida.terminado == "") { 
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
                            par.Ganador = user.Codigo_Usuario;
                        }
                        else
                        {
                            par.Ganador = 0;
                        }
                        db.PARTIDA.Add(par);
                        db.SaveChanges();
                        partida.terminado = "si";
                    }
                }
            }
            
            else if (partida.siguiente_tiro == partida.color_jugador2 && partida.tipo == "M")
            {
                ViewBag.maquina = true;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CargarPartida(HttpPostedFileBase archivo,string jugador22, string color11, string color22) {
            string[] ran = { "negro", "blanco" };
            Random rnd = new Random();
            int indice = 0;
            if (color11 == "" || color11 == null)
            {
                indice = rnd.Next(ran.Length);
                color11 = ran[indice];

                if (color11 == "negro")
                {
                    color22 = "blanco";
                }
                else
                {
                    color22 = "negro";
                }
            }
            
            var Filename = Path.GetFileName(archivo.FileName);
            var path = Path.Combine(Server.MapPath("~/Public/XML"), Filename);
            archivo.SaveAs(path);
            XmlDocument documento = new XmlDocument();
            documento.Load(path);
            
            USUARIO user = (USUARIO)this.Session["user"];
            PartidaCargada = Funciones.CrearPartida(modelo,color11,color22,"",user.Usuario1,jugador22);
            PartidaCargada.movimientos_1 = 0;
            PartidaCargada.movimientos_2 = 0;
            PartidaCargada.punteo_jugador1 = 0;
            PartidaCargada.punteo_jugador2 = 0;
            foreach (var fil in PartidaCargada.Filas)
            {
                foreach (var col in fil.columnas)
                {
                    col.color = "b";
                }
            }
            foreach (XmlNode node in documento.SelectNodes("/tablero/ficha"))
            {
                var f = node["fila"].InnerText;
                var c = node["columna"].InnerText;
                var color = node["color"].InnerText;
                foreach (var fila in PartidaCargada.Filas)
                {
                    foreach(var columna in fila.columnas)
                    {
                        if (c == columna.nombre && f == fila.nombre)
                        {
                            columna.color = color;
                        }
                    }
                }
            }
            foreach (XmlNode node in documento.SelectNodes("/tablero/siguienteTiro"))
            {
                PartidaCargada.siguiente_tiro = node["color"].InnerText;
            }
            object[] objeto = Funciones.ComprobarCasillas(PartidaCargada);
            bool respuesta = (bool)objeto[1];
            if (respuesta) { 
                PartidaCargada = (PartidaViewModel) objeto[0];
                PartidaCargada = Funciones.Movimientos(PartidaCargada);
                PartidaCargada = Funciones.Punteos(PartidaCargada);
                cargar = true;
                return RedirectToAction("Partida", "Partida");
            }
            return RedirectToAction("Index", "Partida", new { mensaje = "Error en estructura de la partida" });
        }

        public ActionResult GuardarPartida(int nombre)
        {
            

            PartidaViewModel partida = new PartidaViewModel();
            foreach (var part in modelo.partidas)
            {
                if (part.nombre == nombre)
                {
                    partida = part;
                    break;
                }
            }
            string Filename = "Partida_#" + partida.nombre + ".xml";
            var path = Path.Combine(Server.MapPath("~/Public/XML_SAVE"), Filename);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            XDocument documento = new XDocument(new XDeclaration("1.0","utf-8",null));
            XElement Raiz = new XElement("tablero");
            documento.Add(Raiz);
            foreach ( var fila in partida.Filas)
            {
                foreach (var columna in fila.columnas)
                {
                    if (!string.IsNullOrEmpty(columna.color) && columna.color!="b")
                    {
                        XElement ficha = new XElement("ficha");
                        XElement colors = new XElement("color",columna.color);
                        XElement col = new XElement("columna",columna.nombre);
                        XElement fil = new XElement("fila",fila.nombre);
                        ficha.Add(colors);
                        ficha.Add(col);
                        ficha.Add(fil);
                        Raiz.Add(ficha);
                    }
                }
            }
            XElement siguiente =new XElement("siguienteTiro");
            XElement color =new XElement("color",partida.siguiente_tiro);
            siguiente.Add(color);
            Raiz.Add(siguiente);
            
            documento.Save(path);
            return RedirectToAction("Index","Partida",new { mensaje = "Partida Guardada"});
        }
    }
    
}