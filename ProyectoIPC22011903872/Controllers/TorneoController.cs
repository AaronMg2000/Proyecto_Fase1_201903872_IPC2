using ProyectoIPC22011903872.Models;
using ProyectoIPC22011903872.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIPC22011903872.Controllers
{
    public class TorneoController : Controller
    {
        // GET: Torneo
        public static List<TorneoViewModel> Torneos = new List<TorneoViewModel>();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.user = this.Session["user"];
            return View();
        }

        [HttpPost]
        public ActionResult CrearTorneo(string NT, int Tipo, List<Equipo> Equipos)
        {
            /*if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.user = this.Session["user"];*/
            List<getUserViewModel> ListaUsuarios;
            List<TorneoGetViewModel> ListaTorneo;
            using (OthelloEntities db = new OthelloEntities())
            {
                ListaTorneo = (from d in db.TORNEO
                               select new TorneoGetViewModel
                               {
                                   codigo_Torneo = d.Codigo_Torneo,
                                   nombre = d.Nombre,
                                   Tipo = d.TIPO,
                                   fecha = d.fecha_inicio,
                                   ganador = d.Ganador
                               }).ToList();
            }
            foreach (var torneo in ListaTorneo)
            {
                if(torneo.nombre == NT)
                {
                    ViewBag.mensaje = "El nombre del torneo ya esta regitrado en la base de datos";
                    return View("Index");
                }
            }
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
            var x = 0;
            foreach (var tor in Torneos)
            {
                x++;
            }
            TorneoViewModel Torneo = new TorneoViewModel();
            Torneo = FuncionesTorneo.CrearTorneo(x,Equipos, NT, Tipo);
            Torneos.Add(Torneo);
            return View("Esquema", Torneo);
        }

        public ActionResult Esquema(int nombre)
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.user = this.Session["user"];
            TorneoViewModel Torneo = new TorneoViewModel();

            foreach (var torn in Torneos)
            {
                if (torn.codigo == nombre)
                {
                    Torneo = torn;
                    break;
                }
            }
            return View(Torneo);
        }

        [HttpPost]
        public ActionResult CantidadEquipo(int Cantidad)
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
            ViewBag.cantidad = Cantidad;
            return View("_VistaEquipos", ListaUsuarios);
        }

        [HttpGet]
        public ActionResult Partida(int nombre, int encuentro, int fase, int part)
        {
            TorneoViewModel Torneo = new TorneoViewModel();
            ViewBag.Fase = fase;
            fase = fase - 1;
            foreach (var torn in Torneos)
            {
                if (torn.codigo == nombre)
                {
                    Torneo = torn;
                    break;
                }
            }
            var EncuentroActual = Torneo.Fases[fase].Encuentros[encuentro];
            var partidaActual = EncuentroActual.Partidas.partidas[part - 1];
            ViewBag.Torneo = nombre;
            ViewBag.Encuentro = encuentro;
            ViewBag.partida = partidaActual;
            partidaActual = Funciones.Movimientos(partidaActual);
            return View(partidaActual);
        }

        [HttpPost]
        public ActionResult Partida(int nom, string fila, string columna, int nombre, int encuentro, int fase, int parti)
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user = (USUARIO)this.Session["user"];
            
            TorneoViewModel Torneo = new TorneoViewModel();
            ViewBag.Fase = fase;
            fase = fase - 1;
            foreach (var torn in Torneos)
            {
                if (torn.codigo == nombre)
                {
                    Torneo = torn;
                    break;
                }
            }
            var EncuentroActual = Torneo.Fases[fase].Encuentros[encuentro];
            var partidaActual = EncuentroActual.Partidas.partidas[parti - 1];
            ViewBag.Torneo = nombre;
            ViewBag.Encuentro = encuentro;
            ViewBag.partida = partidaActual;
            var Registrar = true;
            ViewBag.mensaje = "funciono";
            var partida = new PartidaViewModel();
            foreach (var part in EncuentroActual.Partidas.partidas)
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
                        if (partida.modalidad == "Inversa")
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
                            if ((punteomayor < partida.punteo_jugador2 && partida.modalidad == "Normal") || (punteomayor > partida.punteo_jugador2 && partida.modalidad == "Inversa"))
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
                    partida.terminado = "si";
                }
            }
            else if (partida.colores_jugador2.Contains(partida.siguiente_tiro) && partida.tipo == "M")
            {
                ViewBag.maquina = true;
            }

            return View(model);
        }
        
        [HttpPost]
        public ActionResult DeterminarGanador(int nombre, int punteo1, int punteo2, int encuentro, int fase, int part)
        {
            TorneoViewModel Torneo = new TorneoViewModel();
            var ter = false;
            fase = fase - 1;
            var reg = false;
            foreach (var torn in Torneos)
            {
                if (torn.codigo == nombre)
                {
                    Torneo = torn;
                    break;
                }
            }
            var EncuentroActual = Torneo.Fases[fase].Encuentros[encuentro];
            var Equipo1 = new Equipo();
            var Equipo2 = new Equipo();
            foreach (var eq in Torneo.Equipos)
            {
                if(eq.nombre == EncuentroActual.Equipo1)
                {
                    Equipo1 = eq;
                }
                else if(eq.nombre == EncuentroActual.Equipo2)
                {
                    Equipo2 = eq;
                }
            }
            var FaseActal = Torneo.Fases[fase];
            var partidaActual = EncuentroActual.Partidas.partidas[part-1];
            partidaActual.terminado = "si";
            partidaActual.punteo_jugador1 = punteo1;
            partidaActual.punteo_jugador2 = punteo2;
            if (punteo1>punteo2)
            {
                EncuentroActual.punteo1 += 3;
                Equipo1.puntaje += 3;
            }
            else if(punteo1 == punteo2)
            {
                EncuentroActual.punteo1 += 1;
                EncuentroActual.punteo2 += 1;
                Equipo1.puntaje += 1;
                Equipo2.puntaje += 1;
            }
            else
            {
                EncuentroActual.punteo2 += 3;
                Equipo2.puntaje += 3;
            }
            foreach (var partida in EncuentroActual.Partidas.partidas)
            {
                if (partida.terminado!="si")
                {
                    reg = false;
                    break;
                }
                else
                {
                    reg = true;
                }
            }
            if (reg)
            {
                if (EncuentroActual.punteo1 > EncuentroActual.punteo2)
                {
                    EncuentroActual.Ganador = EncuentroActual.Equipo1;
                    EncuentroActual.resultado1 = "Ganador";
                    EncuentroActual.resultado2 = "Perdedor";
                    var colocado = false;
                    foreach (var fas in Torneo.Fases)
                    {
                        foreach (var enc in fas.Encuentros)
                        {
                            if (enc.Equipo1=="")
                            {
                                enc.Equipo1 = Equipo1.nombre;
                                colocado = true;
                                break;
                            }
                            else if (enc.Equipo2=="")
                            {
                                enc.Equipo2 = Equipo1.nombre;
                                colocado = true;
                                FuncionesTorneo.CrearPartidas(Torneo,fas,enc);
                                break;
                            }
                        }
                        if (colocado)   
                        {
                            break;
                        }
                    }
                }
                else if(EncuentroActual.punteo1 < EncuentroActual.punteo2)
                {
                    EncuentroActual.Ganador = EncuentroActual.Equipo2;
                    EncuentroActual.resultado2 = "Ganador";
                    EncuentroActual.resultado1 = "Perdedor";
                    var colocado = false;
                    foreach (var fas in Torneo.Fases)
                    {
                        foreach (var enc in fas.Encuentros)
                        {
                            if (enc.Equipo1 == "")
                            {
                                enc.Equipo1 = Equipo2.nombre;
                                colocado = true;
                                break;
                            }
                            else if (enc.Equipo2 == "")
                            {
                                enc.Equipo2 = Equipo2.nombre;
                                colocado = true;
                                break;
                            }
                        }
                        if (colocado)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    string[] colores = { "negro", "blanco" };
                    Random ran = new Random();
                    int indi = 0;
                    indi = ran.Next(colores.Length);
                    List<string> colo1 = new List<string>() { colores[indi] };
                    List<string> colo2 = new List<string>();
                    if (colo1.Contains("negro"))
                    {
                        colo2.Add("blanco");
                    }
                    else
                    {
                        colo2.Add("negro");
                    }
                    var partidaNueva = new PartidaViewModel();
                    ViewBag.empate = true;
                    partidaNueva = Funciones.CrearPartida(EncuentroActual.Partidas,colo1,colo2,"negro","","",8,8,"Normal", "Normal", "Normal");
                }
            }
            foreach (var fas in Torneo.Fases)
            {
                foreach (var equip in fas.Encuentros)
                {
                    if (equip.Ganador=="")
                    {
                        ter = false;
                        break;
                    }
                    else
                    {
                        ter = true;
                    }
                }
            }
            if (ter)
            {
                ViewBag.Terminado = "Si";
                if (EncuentroActual.punteo1> EncuentroActual.punteo2)
                {
                    Torneo.Ganador = EncuentroActual.Equipo1;
                }
                else
                {
                    Torneo.Ganador = EncuentroActual.Equipo2;
                }
                var TorneoActual = new TORNEO();
                var EquipoActual = new EQUIPO();
                using (OthelloEntities db = new OthelloEntities())
                {
                    var NuevoTorneo = new TORNEO();
                    NuevoTorneo.Nombre = Torneo.Nombre;
                    var Ttipo = 0;
                    if (Torneo.Tipo == 16)
                    {
                        Ttipo = 3;
                    }
                    if (Torneo.Tipo == 8)
                    {
                        Ttipo = 2;
                    }
                    if (Torneo.Tipo == 4)
                    {
                        Ttipo = 1;
                    }
                    NuevoTorneo.TIPO = Ttipo;
                    NuevoTorneo.fecha_inicio = DateTime.Now;
                    db.TORNEO.Add(NuevoTorneo);
                    db.SaveChanges();
                    TorneoActual = db.TORNEO.FirstOrDefault(e => e.Nombre == Torneo.Nombre);
                }
                
                foreach (var eq in Torneo.Equipos)
                {
                    using (OthelloEntities db = new OthelloEntities())
                    {
                        EQUIPO nuevoEquipo = new EQUIPO();
                        nuevoEquipo.Nombre = eq.nombre;
                        nuevoEquipo.PUNTEO = eq.puntaje;
                        nuevoEquipo.Codigo_Torneo = TorneoActual.Codigo_Torneo;
                        db.EQUIPO.Add(nuevoEquipo);
                        db.SaveChanges();
                        var EquipoUti = db.EQUIPO.FirstOrDefault(e => e.Nombre == eq.nombre);
                        using (OthelloEntities db2 = new OthelloEntities())
                        {
                            foreach (var jug in eq.jugadores)
                            {
                                var usu = db2.USUARIO.FirstOrDefault(e => e.Usuario1 == jug);
                                DETALLE_EQUIPO nuevoDetalle = new DETALLE_EQUIPO();
                                nuevoDetalle.Codigo_equipo = EquipoUti.Codigo_Equipo;
                                nuevoDetalle.Codigo_Usuario = usu.Codigo_Usuario;
                                db2.DETALLE_EQUIPO.Add(nuevoDetalle);
                            }
                            if (eq.nombre == Torneo.Ganador)
                            {
                                TorneoActual.Ganador = EquipoActual.Codigo_Equipo;
                            }
                            db2.SaveChanges();
                        }
                    }
                }
                using (OthelloEntities db = new OthelloEntities())
                {
                    List<EquiposGetViewModel> ListaEquipos = new List<EquiposGetViewModel>();
                    ListaEquipos = (from d in db.EQUIPO.Where(e => e.Codigo_Torneo == TorneoActual.Codigo_Torneo)
                                     select new EquiposGetViewModel
                                     {
                                        Codigo_Equipo = d.Codigo_Equipo,
                                        Nombre = d.Nombre,
                                        Codigo_Torneo = d.Codigo_Torneo,
                                        PUNTEO = d.PUNTEO
                                     }).ToList();
                    foreach (var fas in Torneo.Fases)
                    {
                        foreach (var encu in fas.Encuentros)
                        {
                            using (OthelloEntities db3 = new OthelloEntities())
                            {
                                ENCUENTRO nuevoEncuentro = new ENCUENTRO();
                                var E1 = db3.EQUIPO.FirstOrDefault(e => e.Nombre == encu.Equipo1);
                                var E2 = db3.EQUIPO.FirstOrDefault(e => e.Nombre == encu.Equipo2);
                                nuevoEncuentro.Codigo_Equipo1 = E1.Codigo_Equipo;
                                nuevoEncuentro.Codigo_Equipo2 = E2.Codigo_Equipo;
                                nuevoEncuentro.Numero_Fase = fas.nombre;
                                nuevoEncuentro.Punteo_Equipo1 = encu.punteo1;
                                nuevoEncuentro.Punteo_Equipo2 = encu.punteo2;
                                nuevoEncuentro.Ganador = encu.Ganador;
                                db3.ENCUENTRO.Add(nuevoEncuentro);
                                db3.SaveChanges();
                                var EncuentroNuevo = db3.ENCUENTRO.FirstOrDefault(e => e.Codigo_Equipo1 == E1.Codigo_Equipo && e.Codigo_Equipo2 == E2.Codigo_Equipo && e.Numero_Fase == fas.nombre);
                                using (OthelloEntities db2 = new OthelloEntities())
                                {
                                    foreach (var par in encu.Partidas.partidas)
                                    {
                                        PARTIDA nuevaPartida = new PARTIDA();
                                        var u1 = db2.USUARIO.FirstOrDefault(e => e.Usuario1 == par.jugador1);
                                        var u2 = db2.USUARIO.FirstOrDefault(e => e.Usuario1 == par.jugador2);
                                        nuevaPartida.Codigo_Usuario_1 = u1.Codigo_Usuario;
                                        nuevaPartida.Codigo_Usuario_2 = u2.Codigo_Usuario;
                                        nuevaPartida.Codigo_Encuentro = EncuentroNuevo.Codigo_Encuentro;
                                        if (par.punteo_jugador1 > par.punteo_jugador2)
                                        {
                                            nuevaPartida.GanadorUsuario = u1.Codigo_Usuario;
                                        }
                                        if (par.punteo_jugador1 < par.punteo_jugador2)
                                        {
                                            nuevaPartida.GanadorUsuario = u2.Codigo_Usuario;
                                        }
                                        nuevaPartida.TIPO = 2;
                                        nuevaPartida.MODO = 2;
                                        nuevaPartida.Punteo_1 = par.punteo_jugador1;
                                        nuevaPartida.Punteo_2 = par.punteo_jugador2;
                                        nuevaPartida.movimientos_1 = par.movimientos_1;
                                        nuevaPartida.movimientos_2 = par.movimientos_2;
                                        nuevaPartida.Tiempo_Jugador1 = par.Ttiempo1;
                                        nuevaPartida.Tiempo_Jugador2 = par.Ttiempo2;
                                        db2.PARTIDA.Add(nuevaPartida);
                                        db2.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return View("_TablaTorneo",Torneo);
        }
        [HttpPost]
        public ActionResult Cronometro1(int nombre, int encuentro, int fase, int part)
        {
            ViewBag.j = "1";
            TorneoViewModel Torneo = new TorneoViewModel();

            fase = fase - 1;
            foreach (var torn in Torneos)
            {
                if (torn.codigo == nombre)
                {
                    Torneo = torn;
                    break;
                }
            }
            var EncuentroActual = Torneo.Fases[fase].Encuentros[encuentro];
            var partida = EncuentroActual.Partidas.partidas[part - 1];

            if (partida.pt1 && partida.terminado == "")
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
            return View("_Cronometro", partida);
        }
        [HttpPost]
        public ActionResult Cronometro2(int nombre, int encuentro, int fase, int part)
        {
            ViewBag.j = "2";
            TorneoViewModel Torneo = new TorneoViewModel();

            fase = fase - 1;
            foreach (var torn in Torneos)
            {
                if (torn.codigo == nombre)
                {
                    Torneo = torn;
                    break;
                }
            }
            var EncuentroActual = Torneo.Fases[fase].Encuentros[encuentro];
            var partida = EncuentroActual.Partidas.partidas[part - 1];
            
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
        [HttpPost]
        public ActionResult Empate(int nombre, int encuentro, int fase, int part, string jugador1, string jugador2)
        {
            TorneoViewModel Torneo = new TorneoViewModel();

            fase = fase - 1;
            foreach (var torn in Torneos)
            {
                if (torn.codigo == nombre)
                {
                    Torneo = torn;
                    break;
                }
            }
            var EncuentroActual = Torneo.Fases[fase].Encuentros[encuentro];
            var FaseActal = Torneo.Fases[fase];
            var partidaActual = EncuentroActual.Partidas.partidas[part - 1];
            partidaActual.jugador1 = jugador1;
            partidaActual.jugador2 = jugador2;
            return View("_TablaTorneo", Torneo);
        }
    }
}