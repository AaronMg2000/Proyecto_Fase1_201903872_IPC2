using ProyectoIPC22011903872.Models;
using ProyectoIPC22011903872.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIPC22011903872
{
    public class FuncionesTorneo
    {
        public static TorneoViewModel CrearTorneo(int cod,List<Equipo> Equipos, string NT, int tipo)
        {
            TorneoViewModel Torneo = new TorneoViewModel();
            Torneo.Nombre = NT;
            Torneo.codigo = cod;
            Torneo.Tipo = tipo;
            Fase NuevaFase = new Fase();
            NuevaFase.nombre = 1;
            int x = 0;
            int y = 0;
            var te = 0;
            var ListaEquipos = new List<Equipo>();
            while (te < Equipos.Count())
            {
                Random rnd = new Random();
                int indice = 0;
                indice = rnd.Next(Equipos.Count());
                if (!ListaEquipos.Contains(Equipos[indice]))
                {
                    ListaEquipos.Add(Equipos[indice]);
                    te++;
                }
            }
            Torneo.Equipos = ListaEquipos;
            int CE = 0;
            while (x < tipo/2){
                Encuentro NuevoEncuetro = new Encuentro();
                NuevoEncuetro.codigo = CE;
                NuevoEncuetro.Equipo1 = ListaEquipos[y].nombre;
                NuevoEncuetro.Equipo2 = ListaEquipos[y + 1].nombre;
                /*NuevoEncuetro.resultado1 = "Ganador";
                NuevoEncuetro.resultado2 = "Perdedor";*/
                NuevoEncuetro.Ganador = "";
                NuevoEncuetro.punteo1 = 0;
                NuevoEncuetro.punteo2 = 0;
                var ListaAleatoria1 = new List<string>();
                var ListaAleatoria2 = new List<string>();
                var tt = 0;
                while (tt < ListaEquipos[y].jugadores.Count())
                {
                    Random rnd = new Random();
                    int indice = 0;
                    indice = rnd.Next(ListaEquipos[y].jugadores.Count());
                    if (!ListaAleatoria1.Contains(ListaEquipos[y].jugadores[indice]))
                    {
                        ListaAleatoria1.Add(ListaEquipos[y].jugadores[indice]);
                        tt++;
                    }
                }
                tt = 0;
                while (tt < Equipos[y+1].jugadores.Count())
                {
                    Random rnd = new Random();
                    int indice = 0;
                    indice = rnd.Next(ListaEquipos[y+1].jugadores.Count());
                    if (!ListaAleatoria2.Contains(ListaEquipos[y+1].jugadores[indice]))
                    {
                        ListaAleatoria2.Add(ListaEquipos[y+1].jugadores[indice]);
                        tt++;
                    }
                }
                x++;
                y += 2;
                var partida1 = new PartidaViewModel();
                var partida2 = new PartidaViewModel();
                var partida3 = new PartidaViewModel();
                var datos = new DatosModel();
                string[] colores = {"negro","blanco" };
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
                partida1 = Funciones.CrearPartida(NuevoEncuetro.Partidas, colo1, colo2, "negro", ListaAleatoria1[0], ListaAleatoria2[0], 8, 8, "Normal", "Normal", null);
                partida2 = Funciones.CrearPartida(NuevoEncuetro.Partidas, colo1, colo2, "negro", ListaAleatoria1[1], ListaAleatoria2[1], 8, 8, "Normal", "Normal", null);
                partida3 = Funciones.CrearPartida(NuevoEncuetro.Partidas, colo1, colo2, "negro", ListaAleatoria1[2], ListaAleatoria2[2], 8, 8, "Normal", "Normal", null);

                NuevaFase.Encuentros.Add(NuevoEncuetro);
                CE++;
            }
            Torneo.Fases.Add(NuevaFase);
            int fas = 2;
            var jj = 0;
            if (tipo==4)
            {
                jj = 2;
            }
            else if (tipo == 8)
            {
                jj = 3;
            }
            else if (tipo==16)
            {
                jj = 4;
            }
            if (tipo == 16 || tipo == 8)
            {
                var T = 0;
                if (tipo == 16)
                {
                    T = 8;
                }
                if (tipo == 8)
                {
                    T = 4;
                }
                while (fas<jj)
                {
                    CE = 0;
                    var h = 0;
                    var NuevFase = new Fase();
                    NuevFase.nombre = fas;
                    while (T/2>h)
                    {
                        Encuentro NuevoEncuetro = new Encuentro();
                        NuevoEncuetro.codigo = CE;
                        NuevoEncuetro.Equipo1="";
                        NuevoEncuetro.Equipo2 = "";
                        NuevoEncuetro.Ganador = "";
                        NuevoEncuetro.punteo1 = 0;
                        NuevoEncuetro.punteo2 = 0;
                        h++;
                        CE++;
                        NuevFase.Encuentros.Add(NuevoEncuetro);
                    }
                    Torneo.Fases.Add(NuevFase);
                    T -= 4;
                    fas++;
                }
            }
            /*Fase final*/
            CE = 0;
            var NuevFase2 = new Fase();
            NuevFase2.nombre = fas;
            Encuentro NuevoEncuetro2 = new Encuentro();
            NuevoEncuetro2.codigo = CE;
            NuevoEncuetro2.Equipo1 = "";
            NuevoEncuetro2.Equipo2 = "";
            NuevoEncuetro2.Ganador = "";
            NuevoEncuetro2.punteo1 = 0;
            NuevoEncuetro2.punteo2 = 0;
            NuevFase2.Encuentros.Add(NuevoEncuetro2);
            Torneo.Fases.Add(NuevFase2);
            /**/
            return Torneo;
        }
        /*
            public static TorneoViewModel CrearFase(TorneoViewModel Torneo, Equipo equipo)
            {
                int ultimaFase=0;
                foreach (var fas in Torneo.Fases)
                {
                    ultimaFase = fas.nombre;
                }
                int nombre = ultimaFase + 1;
                Fase nuevaFase = new Fase();
                nuevaFase.nombre = nombre;
                if(Torneo.Tipo==4 && nombre == 1)
                {

                }
                return Torneo;
            }
        */
        public static TorneoViewModel CrearPartidas(TorneoViewModel Torneo, Fase fase, Encuentro encuentro)
        {
            int CE = 0;
            int tipo = Torneo.Tipo;
            int tt = 0;
            var equipo1 = new Equipo();
            var equipo2 = new Equipo();
            var ListaAleatoria1 = new List<string>();
            var ListaAleatoria2 = new List<string>();
            foreach (var eq in Torneo.Equipos)
            {
                if (eq.nombre == encuentro.Equipo1)
                {
                    equipo1 = eq;
                }
                else if (eq.nombre == encuentro.Equipo2)
                {
                    equipo2 = eq;
                }
            }
            while (tt < equipo1.jugadores.Count())
            {
                Random rnd = new Random();
                int indice = 0;
                indice = rnd.Next(equipo1.jugadores.Count());
                if (!ListaAleatoria1.Contains(equipo1.jugadores[indice]))
                {
                    ListaAleatoria1.Add(equipo1.jugadores[indice]);
                    tt++;
                }
            }
            tt = 0;
            while (tt < equipo2.jugadores.Count())
            {
                Random rnd = new Random();
                int indice = 0;
                indice = rnd.Next(equipo2.jugadores.Count());
                if (!ListaAleatoria2.Contains(equipo2.jugadores[indice]))
                {
                    ListaAleatoria2.Add(equipo2.jugadores[indice]);
                    tt++;
                }
            }
            var partida1 = new PartidaViewModel();
            var partida2 = new PartidaViewModel();
            var partida3 = new PartidaViewModel();
            var datos = new DatosModel();
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
            partida1 = Funciones.CrearPartida(encuentro.Partidas, colo1, colo2, "negro", ListaAleatoria1[0], ListaAleatoria2[0], 8, 8, "Normal", "Normal", null);
            partida2 = Funciones.CrearPartida(encuentro.Partidas, colo1, colo2, "negro", ListaAleatoria1[1], ListaAleatoria2[1], 8, 8, "Normal", "Normal", null);
            partida3 = Funciones.CrearPartida(encuentro.Partidas, colo1, colo2, "negro", ListaAleatoria1[2], ListaAleatoria2[2], 8, 8, "Normal", "Normal", null);
            return Torneo;
        }
    }
}