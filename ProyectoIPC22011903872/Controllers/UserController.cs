using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using ProyectoIPC22011903872.Models;
using ProyectoIPC22011903872.Models.ViewModels;

namespace ProyectoIPC22011903872.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login(string Usuario, string Contraseña) {
            ViewBag.Usuario = Usuario;
            ViewBag.Contraseña = Contraseña;
            if(User.Identity.IsAuthenticated && this.Session["user"]!=null){
                return RedirectToAction("Index", "Principal");
            }
            return View();
        }
        public ActionResult getUser()
        {
            List<getUserViewModel> ListaUsuarios;
            using (OthelloEntities db = new OthelloEntities())
            {
                ListaUsuarios = (from d in db.USUARIO
                                 select new getUserViewModel
                                 {
                                     Nombre = d.Nombre,
                                     Apellido = d.Apellido,
                                     Usuario = d.Usuario1,
                                     Correo_Electronico = d.Correo_Electronico,
                                     Codigo_Pais = d.Codigo_Pais,
                                     Fecha_nacimiento = d.Fecha_nacimiento
                                 }).ToList();
            }
            return View(ListaUsuarios);
        }
        [HttpPost]
        public ActionResult Validar(string Usuario, string password) {
            var us = "";
            var Contraseña = "";
            using (OthelloEntities db = new OthelloEntities())
            {
                var pass = Encriptar.CrearHASH(password);
                var usuario2 = db.USUARIO.FirstOrDefault(e => e.Usuario1 == Usuario);
                var usuario = db.USUARIO.FirstOrDefault(e => e.Usuario1 == Usuario && e.Contraseña == pass);
                if (usuario2 != null)
                {
                    if (usuario != null)
                    {
                        FormsAuthentication.SetAuthCookie(usuario.Usuario1, true);
                        Session["user"] = usuario;
                        return RedirectToAction("Index", "Principal");
                    }
                    else
                    {
                        Contraseña = "Contraseña Incorrecta";
                    }
                }
                else
                {
                    us = "El usuario ingresado no existe.";
                }
            }
            return RedirectToAction("Login", "User", new { Usuario = us, Contraseña = Contraseña});
        }
        public ActionResult Registrar() {
            List<getPaisViewModel> ListaPais;
            using (OthelloEntities db = new OthelloEntities())
            {
                ListaPais = (from d in db.PAIS
                             select new getPaisViewModel
                             {
                                 codigo = d.codigo_Pais,
                                 nombre = d.Nombre,
                                 ISO3 = d.ISO3

                             }).ToList();
            }
            ViewBag.paises = ListaPais;
            if (User.Identity.IsAuthenticated && this.Session["user"]!=null)
            {
                return RedirectToAction("Index", "Principal");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(addUserViewModel model) {
            List<getPaisViewModel> ListaPais;
            using (OthelloEntities db = new OthelloEntities())
            {
                ListaPais = (from d in db.PAIS
                             select new getPaisViewModel
                             {
                                 codigo = d.codigo_Pais,
                                 nombre = d.Nombre,
                                 ISO3 = d.ISO3

                             }).ToList();
            }
            ViewBag.paises = ListaPais;

                if (ModelState.IsValid) {
                    using (OthelloEntities db= new OthelloEntities())
                    {
                        var user = db.USUARIO.FirstOrDefault(e => e.Usuario1 == model.Usuario);
                        var user2 = db.USUARIO.FirstOrDefault(e => e.Correo_Electronico == model.Correo_Electronico);
                        if (user==null)
                        {
                            if (user2 == null) { 
                                if (model.Password==model.RePassword)
                                {
                                    var Usuario = new USUARIO();
                                    Usuario.Nombre = model.Nombre;
                                    Usuario.Apellido = model.Apellido;
                                    Usuario.Correo_Electronico = model.Correo_Electronico;
                                    Usuario.Usuario1 = model.Usuario;
                                    Usuario.Contraseña = Encriptar.CrearHASH(model.Password);
                                    Usuario.Codigo_Pais = model.Codigo_Pais;
                                    Usuario.Fecha_nacimiento = model.Fecha_nacimiento;
                                    db.USUARIO.Add(Usuario);
                                    db.SaveChanges();
                                    return Redirect("~/");
                                }
                                else
                                {
                                    ViewBag.mensajeContraseña = "Contraseña no coincide";
                                }
                            }
                            else
                            {
                                ViewBag.mensajeCorreo = "Correo ya registrado";
                            }
                        }
                        else
                        {
                            ViewBag.mensajeUsuario = "Usuario ya Registrado";
                        }
                    }
                }
                return View(model);
            
        }
        
        public ActionResult VerPerfil()
        {
            /*if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }*/
            USUARIO user = (USUARIO) this.Session["user"];

            List<DetalleViewModel> ListaDetalle;
            List<EQUIPO> listaEquipo = new List<EQUIPO>();
            List<EncuentroGet> listaEncuentrosGanados = new List<EncuentroGet>();
            List<EncuentroGet> listaEncuentros = new List<EncuentroGet>();
            List<EncuentroGet> listaEncuentrosPerdidos = new List<EncuentroGet>();
            List<PartidaGet> ListaPartidas = new List<PartidaGet>();
            List<TORNEO> listaTorneoGanado = new List<TORNEO>();
            List<TORNEO> listaTorneoPerdido = new List<TORNEO>();
            int PartidasOthelloGanadas = 0;
            int PartidasXtreamGanadas = 0;
            int partidasOthelloperdidas = 0;
            int partidasOthelloempatadas = 0;
            int partidasXtreamperdidas = 0;
            int prtidasXtreamEmpatadas = 0;
            int partidasGanadasTorneo = 0;
            int partidasEmpatadasTorneo = 0;
            int partidasperdidasTorneo = 0;
            int torneosGanados = 0;
            int torneosPerdidos = 0;
            int puntostorneo = 0;
            using (OthelloEntities db = new OthelloEntities())
            {
                ListaDetalle =  (from d in db.DETALLE_EQUIPO.Where(a=> a.Codigo_Usuario == user.Codigo_Usuario)
                    select new DetalleViewModel
                    {
                        codigo_detalle = d.codigo_detalle,
                        Codigo_equipo = d.Codigo_equipo,
                        Codigo_Usuario = d.Codigo_Usuario
                    }).ToList();
            }
            foreach (var det in ListaDetalle)
            {
                using (OthelloEntities db = new OthelloEntities()) { 
                    var E = db.EQUIPO.FirstOrDefault(e => e.Codigo_Equipo == det.Codigo_equipo);
                    if (E!=null)
                    {
                        listaEquipo.Add(E);
                        puntostorneo += E.PUNTEO;
                    }
                }
            }
            foreach (var det in listaEquipo)
            {
                using (OthelloEntities db = new OthelloEntities())
                {
                    var E = db.TORNEO.FirstOrDefault(e => e.Codigo_Torneo == det.Codigo_Torneo);
                    if (E != null)
                    {
                        if (E.Ganador == det.Codigo_Equipo)
                        {
                            listaTorneoGanado.Add(E);
                            torneosGanados++;
                        }
                        else
                        {
                            listaTorneoPerdido.Add(E);
                            torneosPerdidos++;
                        }
                    }
                }
            }
            
            foreach (var det in listaEquipo)
            {
                using (OthelloEntities db = new OthelloEntities())
                {
                    var ListaEn = (from d in db.ENCUENTRO.Where(e => e.Codigo_Equipo1 == det.Codigo_Equipo || e.Codigo_Equipo2 == det.Codigo_Equipo)
                                     select new EncuentroGet
                                     {
                                         Codigo_Encuentro = d.Codigo_Encuentro,
                                         Codigo_Equipo1 = d.Codigo_Equipo1,
                                         Codigo_Equipo2 = d.Codigo_Equipo2,
                                         Ganador = d.Ganador,
                                         Numero_Fase = d.Numero_Fase, 
                                         Punteo_Equipo1 = d.Punteo_Equipo1,
                                         Punteo_Equipo2 = d.Punteo_Equipo2
                                     }).ToList();
                    foreach (var E in ListaEn)
                    {
                        if (E.Ganador == det.Nombre)
                        {
                            listaEncuentrosGanados.Add(E);
                        }
                        else
                        {
                            listaEncuentrosPerdidos.Add(E);
                        }
                        listaEncuentros.Add(E);
                    }  
                }
            }

            foreach (var det in listaEncuentros)
            {
                using (OthelloEntities db = new OthelloEntities())
                {
                    var LP = (from d in db.PARTIDA.Where(e => (e.Codigo_Encuentro == det.Codigo_Encuentro) && (e.Codigo_Usuario_1 == user.Codigo_Usuario || e.Codigo_Usuario_2 == user.Codigo_Usuario))
                                     select new PartidaGet
                                     {
                                         Codigo_Partida = d.Codigo_Partida,
                                         Codigo_Usuario_1 = d.Codigo_Usuario_1,
                                         Codigo_Encuentro = d.Codigo_Encuentro,
                                         Codigo_Usuario_2 = d.Codigo_Usuario_2,
                                         ResultadoLocal = d.ResultadoLocal,
                                         GanadorUsuario = d.GanadorUsuario,
                                         TIPO = d.TIPO,
                                         MODO = d.MODO,
                                         CONTRINCANTE = d.CONTRINCANTE,
                                         Punteo_1 = d.Punteo_1,
                                         Punteo_2 = d.Punteo_2,
                                         movimientos_1 = d.movimientos_1,
                                         movimientos_2 = d.movimientos_2,
                                         Tiempo_Jugador1 = d.Tiempo_Jugador1,
                                         Tiempo_Jugador2 = d.Tiempo_Jugador2,
                                         Fecha = d.Fecha
                                     }).ToList();
                    foreach (var E in LP) { 
                        if (E.GanadorUsuario==user.Codigo_Usuario)
                        {
                            partidasGanadasTorneo ++;
                        }
                        else if (E.GanadorUsuario==null)
                        {
                            partidasEmpatadasTorneo++;
                        }
                        else
                        {
                            partidasperdidasTorneo++;
                        }
                    }
                }
            }

            using (OthelloEntities db = new OthelloEntities())
            {
                ListaPartidas = (from d in db.PARTIDA.Where(a => a.Codigo_Usuario_1 == user.Codigo_Usuario && a.Codigo_Usuario_2==null)
                                select new PartidaGet
                                {
                                    Codigo_Partida = d.Codigo_Partida,
                                    Codigo_Usuario_1 = d.Codigo_Usuario_1,
                                    Codigo_Encuentro = d.Codigo_Encuentro,
                                    Codigo_Usuario_2 = d.Codigo_Usuario_2,
                                    ResultadoLocal = d.ResultadoLocal,
                                    GanadorUsuario = d.GanadorUsuario,
                                    TIPO = d.TIPO,
                                    MODO = d.MODO,
                                    CONTRINCANTE = d.CONTRINCANTE,
                                    Punteo_1 = d.Punteo_1,
                                    Punteo_2 = d.Punteo_2,
                                    movimientos_1 = d.movimientos_1,
                                    movimientos_2 = d.movimientos_2,
                                    Tiempo_Jugador1 = d.Tiempo_Jugador1,
                                    Tiempo_Jugador2 = d.Tiempo_Jugador2,
                                    Fecha = d.Fecha
                                }).ToList();
            }
            
            foreach (var det in ListaPartidas)
            {
                if (det.TIPO == 2)
                {
                    if (det.ResultadoLocal == "Ganador")
                    {
                        PartidasOthelloGanadas++;
                    }
                    else if (det.ResultadoLocal == "Perdedor")
                    {
                        partidasOthelloperdidas++;
                    }
                    else
                    {
                        partidasOthelloempatadas++;
                    }
                }
                else
                {
                    if (det.ResultadoLocal == "Ganador")
                    {
                        PartidasXtreamGanadas++;
                    }
                    else if (det.ResultadoLocal == "Perdedor")
                    {
                        partidasXtreamperdidas++;
                    }
                    else
                    {
                        prtidasXtreamEmpatadas++;
                    }
                }
            }

            var Pais = "";
            using (OthelloEntities db = new OthelloEntities())
            {
                var E = db.PAIS.FirstOrDefault(e => e.codigo_Pais == user.Codigo_Pais);
                Pais = E.Nombre;
            }

            ViewBag.OG = PartidasOthelloGanadas;
            ViewBag.XG = PartidasXtreamGanadas;
            ViewBag.OP = partidasOthelloperdidas;
            ViewBag.OE = partidasOthelloempatadas;
            ViewBag.XP = partidasXtreamperdidas;
            ViewBag.XE = prtidasXtreamEmpatadas;
            ViewBag.TG = partidasGanadasTorneo;
            ViewBag.TE = partidasEmpatadasTorneo;
            ViewBag.TP = partidasperdidasTorneo;
            ViewBag.TorG = torneosGanados;
            ViewBag.TorP = torneosPerdidos;
            ViewBag.PT = puntostorneo;
            ViewBag.Pais = Pais;
            return View(user);
        }
    }
}