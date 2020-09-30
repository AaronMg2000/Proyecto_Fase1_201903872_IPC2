using ProyectoIPC22011903872.Models.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

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
            ViewBag.mensaje = "";
            return View();
        }
        [HttpGet]
        public ActionResult Index(string mensaje) {
            ViewBag.mensaje = mensaje;
            return View();
        }

        public ActionResult Partida(string color1,string color2)
        {
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
                partida = Funciones.CrearPartida(modelo,color1,color2,siguiente);
            }
            else
            {
                partida = PartidaCargada;
                cargar = false;
                PartidaCargada = new PartidaViewModel();
            }
            ViewBag.mensaje = "partida";
            ViewBag.partida = partida;
            return View(partida);
        }
        [HttpPost]
        public ActionResult Partida(int nom, string fila, string columna,PartidaViewModel pr)
        {
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
            if (Registrar){ 
                model = Funciones.AgregarFicha(partida, fila, columna);
            }

            ViewBag.partida = model;
            return View(model);
        }
        [HttpPost]
        public ActionResult CargarPartida(HttpPostedFileBase archivo) {
            var Filename = Path.GetFileName(archivo.FileName);
            var path = Path.Combine(Server.MapPath("~/Public/XML"), Filename);
            archivo.SaveAs(path);
            XmlDocument documento = new XmlDocument();
            documento.Load(path);
            PartidaCargada = Funciones.CrearPartida(modelo,"negro","blanco","");
            PartidaCargada.movimientos_1 = 0;
            PartidaCargada.movimientos_2 = 0;
            PartidaCargada.punteo_jugador1 = 0;
            PartidaCargada.punteo_jugador2 = 0;
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
                            if (PartidaCargada.color_jugador1==color)
                            {
                                PartidaCargada.punteo_jugador1++;
                                PartidaCargada.movimientos_1++;
                            }
                            else
                            {
                                PartidaCargada.punteo_jugador2++;
                                PartidaCargada.movimientos_2++;
                            }
                        }
                    }
                }
            }
            foreach (XmlNode node in documento.SelectNodes("/tablero/siguienteTiro"))
            {
                PartidaCargada.siguiente_tiro = node["color"].InnerText;
            }
            cargar = true;
            return RedirectToAction("Partida", "Partida");
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
                    if (!string.IsNullOrEmpty(columna.color))
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