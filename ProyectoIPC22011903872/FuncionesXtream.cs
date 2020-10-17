using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoIPC22011903872.Models.ViewModels;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ProyectoIPC22011903872
{
    public class FuncionesXtream
    {
        public static PartidaXtreamViewModel CrearPartida(DatosXtreamModel dato, List<string> color1, List<string> color2, string siguiente, string jugador1, string jugador2, int N, int M)
        {
            var centroF1 = (N / 2);
            var centroF2 = (N / 2) + 1;
            var centroC1 = (M / 2) - 1;
            var centroC2 = (M / 2);
            PartidaXtreamViewModel partida = new PartidaXtreamViewModel();
            string[] columnas = { "A", "B", "C", "D", "E", "F", "G", "H" ,"I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T" };
            partida.nombre = dato.nombre + 1;
            partida.jugador1 = jugador1;
            partida.jugador2 = jugador2;
            if (jugador2 == "Maquina")
            {
                partida.tipo = "M";
            }
            partida.colores_jugador1 = color1;
            partida.colores_jugador2 = color2;
            partida.colorA1 = color1[0];
            partida.colorA2 = color2[0];
            partida.movimientos_1 = 0;
            partida.movimientos_2 = 0;
            partida.punteo_jugador1 = 2;
            partida.punteo_jugador2 = 2;
            partida.N = N;
            partida.M = M;
            double PN = 100 / ((double)N + 2);
            double PM = 100 / ((double)M + 2);
            partida.NP = PN + "%";
            partida.MP = PM + "%";
            siguiente = partida.colores_jugador1[0];
            partida.siguiente_tiro = siguiente;
            for (int i = 1; i <= N; i++)
            {
                FilaViewModel fila = new FilaViewModel();
                fila.nombre = i.ToString();
                for (int j = 0; j < M; j++)
                {
                    ColumnaViewModel col = new ColumnaViewModel();
                    col.color = "";
                    col.nombre = columnas[j];

                    if (j == centroC1 && i == centroF1)
                    {
                        col.color = "";
                    }
                    else if (j == centroC2 && i == centroF1)
                    {
                        col.color = "";
                    }
                    else if (j == centroC1 && i == centroF2)
                    {
                        col.color = "";
                    }
                    else if (j == centroC2 && i == centroF2)
                    {
                        col.color = "";
                    }
                    else
                    {
                        col.color = "b";
                    }

                    fila.columnas.Add(col);
                }
                partida.Filas.Add(fila);
            }
            dato.partidas.Add(partida);
            dato.nombre++;
            return partida;
        }
        
        public static PartidaXtreamViewModel AgregarFicha(PartidaXtreamViewModel partida, string fila, string columna)
        {
            var centroF = partida.N / 2;
            var centroC = partida.M / 2;
            var color = partida.siguiente_tiro;
            if (partida.colores_jugador1.Contains(color) && fila != "saltar")
            {
                partida.punteo_jugador1++;
                partida.movimientos_1++;
            }
            else if (partida.colores_jugador2.Contains(color) && fila != "saltar")
            {
                partida.punteo_jugador2++;
                partida.movimientos_2++;
            }
            if (partida.colores_jugador1.Contains(color))
            {
                partida.siguiente_tiro = partida.colorA2;
            }
            else
            {
                partida.siguiente_tiro = partida.colorA1;
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
            var centro1 = partida.Filas[centroF - 1].columnas[centroC - 1];
            var centro2 = partida.Filas[centroF].columnas[centroC - 1];
            var centro3 = partida.Filas[centroF - 1].columnas[centroC];
            var centro4 = partida.Filas[centroF].columnas[centroC];

            if (centro1.color!="" && centro2.color != "" && centro3.color != "" && centro4.color != "")
            {
            partida.centro=true;
            partida = CambioColor(partida, fila, columna);
            partida = Movimientos(partida);
            }
            partida = Punteos(partida);
            
            if (partida.colores_jugador1.Contains(color)) {
                var index = partida.colores_jugador1.IndexOf(color);
                if (index == (partida.colores_jugador1.Count() - 1))
                {
                    partida.colorA1 = partida.colores_jugador1[0];
                }
                else
                {
                    index++;
                    partida.colorA1 = partida.colores_jugador1[index];
                }
            }
            else if (partida.colores_jugador2.Contains(color))
            {
                var index = partida.colores_jugador2.IndexOf(color);
                if (index == (partida.colores_jugador2.Count() - 1))
                {
                    partida.colorA2 = partida.colores_jugador2[0];
                }
                else
                {
                    index++;
                    partida.colorA2 = partida.colores_jugador2[index];
                }
            }
            return partida;
        }
        public static PartidaXtreamViewModel CambioColor(PartidaXtreamViewModel partida, string f, string c)    
        {
            var color = "";
            FilaViewModel fila = new FilaViewModel();
            var nc = 0;
            var nf = 0;
            var N = 0;
            var M = 0;
            foreach (var fil in partida.Filas)
            {
                if (fil.nombre == f)
                {
                    fila = fil;
                    foreach (var col in fil.columnas)
                    {
                        if (col.nombre == c)
                        {
                            N = partida.Filas.Count();
                            M = fil.columnas.Count();
                            break;
                        }
                        nc++;
                    }
                    break;
                }
                nf++;
            }

            if (partida.colores_jugador1.Contains(partida.siguiente_tiro))
            {
                color = partida.colorA2;
                partida.colores_contrario = partida.colores_jugador1;
                partida.colores_actual = partida.colores_jugador2;
            }
            else
            {
                color = partida.colorA1;
                partida.colores_contrario = partida.colores_jugador2;
                partida.colores_actual = partida.colores_jugador1;
            }
            /*Horizontal*/
            var i = 0;
            var reg = true;
            var r = false;
            var r2 = false;
            var n1 = 0;
            var r3 = false;
            foreach (var col in fila.columnas)
            {
                if (partida.colores_actual.Contains(col.color) && col.nombre == c)
                {
                    r = true;
                    if (r2 && !reg)
                    {
                        r2 = false;
                        i++;
                    }
                    r3 = true;
                }
                else if (partida.colores_actual.Contains(col.color) && col.nombre != c && !r2)
                {
                    r2 = true;
                    r3 = true;
                }
                else if (partida.colores_actual.Contains(col.color) & col.nombre != c && r2)
                {
                    r2 = true;
                    i = n1;
                    r3 = true;
                    reg = false;
                }

                if (col.color != "" && partida.colores_contrario.Contains(col.color) && col.color != "b")
                {
                    reg = true;
                    r3 = true;
                }
                else if (col.color == "")
                {
                    reg = false;
                    if (r)
                    {
                        break;
                    }
                    if (r2)
                    {
                        r2 = false;
                        i++;
                    }
                }
                else if (reg && partida.colores_actual.Contains(col.color) && r && r2)
                {
                    var u = 0;
                    var k = 0;
                    if (i < nc)
                    {
                        u = i + 1;
                        k = nc;
                    }
                    else
                    {
                        u = nc + 1;
                        k = i;
                    }
                    while (u < k)
                    {
                        fila.columnas[u].color = color;
                        u++;
                    }
                    if (i < nc)
                    {
                        r = true;
                        i = nc;
                        r2 = false;
                        reg = false;
                        r3 = true;
                    }
                }

                else
                {
                    if ((r && !r3))
                    {
                        break;
                    }
                    if ((r2 && !r3))
                    {
                        r2 = false;
                    }
                    r3 = false;
                    reg = false;
                }
                if (r && !r2)
                {
                    i++;
                }
                else if (!r2)
                {
                    i++;
                }
                n1++;
            }
            /*Vertical*/
            i = 0;
            reg = true;
            r = false;
            r2 = false;
            n1 = 0;
            r3 = false;
            foreach (var fil in partida.Filas)
            {
                var col = fil.columnas[nc];
                if (partida.colores_actual.Contains(col.color) && fil.nombre == f)
                {
                    r = true;
                    if (r2 && !reg)
                    {
                        r2 = false;
                        i++;
                    }
                    r3 = true;
                }
                else if (partida.colores_actual.Contains(col.color) && fil.nombre != f && !r2)
                {
                    r2 = true;
                    r3 = true;
                }
                else if (partida.colores_actual.Contains(col.color) & fil.nombre != f && r2)
                {
                    r2 = true;
                    i = n1;
                    r3 = true;
                    reg = false;
                }

                if (partida.colores_contrario.Contains(col.color) && fil.nombre != f && col.color != "b" && col.color != "")
                {
                    reg = true;
                    r3 = true;
                }
                else if (col.color == "")
                {
                    reg = false;
                    if (r)
                    {
                        break;
                    }
                    if (r2)
                    {
                        r2 = false;
                        i++;
                    }
                }
                else if (reg && partida.colores_actual.Contains(col.color) && r && r2)
                {
                    var u = 0;
                    var k = 0;
                    if (i < nf)
                    {
                        u = i + 1;
                        k = nf;
                    }
                    else
                    {
                        u = nf + 1;
                        k = i;
                    }
                    while (u < k)
                    {
                        partida.Filas[u].columnas[nc].color = color;
                        u++;
                    }
                    if (i < nf)
                    {
                        r = true;
                        i = nf;
                        r2 = false;
                        reg = false;
                        r3 = true;
                    }
                }

                else
                {
                    if ((r && !r3))
                    {
                        break;
                    }
                    if ((r2 && !r3))
                    {
                        r2 = false;
                    }
                    r3 = false;
                    reg = false;
                }
                if (r && !r2)
                {
                    i++;
                }
                else if (!r2)
                {
                    i++;
                }
                n1++;
            }
            /*Diagonal derecha*/
            i = 0;
            var j = 0;
            var n = 0;
            var ff = 0;
            var cc = 0;
            r = false;
            r2 = false;
            r3 = false;
            var G = 0;
            if (N > M)
            {
                if (nc < nf)
                {
                    if (nf - nc >= (N - 1) - (M - 1))
                    {
                        ff = nf - nc;
                        G = N;
                        cc = 0;
                        n = ff;
                        i = ff;
                        j = cc;
                    }
                    else
                    {
                        cc = 0;
                        ff = nf - nc;
                        G = M;
                        n = cc;
                        i = ff;
                        j = cc;
                    }
                }
                else
                {
                    ff = 0;
                    G = M;
                    cc = nc - nf;
                    n = cc;
                    i = ff;
                    j = cc;
                }
            }
            else if (N == M)
            {
                if (nc < nf)
                {
                    cc = 0;
                    ff = nf - nc;
                    n = ff;
                    G = N;
                    i = ff;
                    j = cc;
                }
                else
                {
                    ff = 0;
                    cc = nc - nf;
                    n = cc;
                    G = N;
                    i = ff;
                    j = cc;
                }

            }
            else
            {
                if (nc < nf)
                {
                    ff = nf - nc;
                    G = N;
                    cc = 0;
                    n = ff;
                    i = ff;
                    j = cc;
                }
                else
                {
                    if (nc - nf >= (M - 1) - (N - 1))
                    {
                        ff = 0;
                        G = M;
                        cc = nc - nf;
                        n = cc;
                        i = ff;
                        j = cc;
                    }
                    else
                    {
                        cc = nc - nf;
                        ff = 0;
                        G = N;
                        n = ff;
                        i = ff;
                        j = cc;
                    }
                }
            }
            while (n < G)
            {
                var col = partida.Filas[ff].columnas[cc];
                if (partida.colores_actual.Contains(col.color) && col.nombre == c)
                {
                    r = true;
                    if (r2 && !reg)
                    {
                        r2 = false;
                        i++;
                        j++;
                    }
                    r3 = true;
                }
                else if (partida.colores_actual.Contains(col.color) && col.nombre != c && !r2)
                {
                    r2 = true;
                    r3 = true;
                }
                else if (partida.colores_actual.Contains(col.color) & col.nombre != c && r2)
                {
                    r2 = true;
                    i = ff;
                    j = cc;
                    r3 = true;
                    reg = false;
                }

                if (col.color != "" && partida.colores_contrario.Contains(col.color) && col.color != "b")
                {
                    reg = true;
                    r3 = true;
                }
                else if (col.color == "")
                {
                    reg = false;
                    if (r)
                    {
                        break;
                    }
                    if (r2)
                    {
                        r2 = false;
                        i++;
                        j++;
                    }
                }
                else if (reg && partida.colores_actual.Contains(col.color) && r && r2)
                {
                    var u = 0;
                    var k = 0;
                    var u2 = 0;
                    var k2 = 0;
                    if (j < nc && i < nf)
                    {
                        u = i + 1;
                        k = nf;
                        k2 = nc;
                        u2 = j + 1;
                    }
                    else
                    {
                        u = nf + 1;
                        k = i;
                        u2 = nc + 1;
                        k2 = j;
                    }
                    while (u < k && u2 < k2)
                    {
                        partida.Filas[u].columnas[u2].color = color;
                        u++;
                        u2++;
                    }
                    if (j < nc && i < nf)
                    {
                        r = true;
                        i = nf;
                        j = nc;
                        r2 = false;
                        reg = false;
                        r3 = true;
                    }
                }

                else
                {
                    if ((r && !r3))
                    {
                        break;
                    }
                    if ((r2 && !r3))
                    {
                        r2 = false;
                    }
                    r3 = false;
                    reg = false;
                }
                if (r && !r2)
                {
                    i++;
                    j++;
                }
                else if (!r2)
                {
                    i++;
                    j++;
                }
                cc++;
                ff++;
                n++;
            }
            /*Diagonal izquierda*/
            i = 0;
            j = 0;
            n = 0;
            ff = 0;
            cc = 0;
            var nc2 = M - 1 - nc;
            r = false;
            r2 = false;
            r3 = false;
            var m = 0;
            if (N > M)
            {
                if (nc2 > nf)
                {
                    cc = 0;
                    ff = M - 1 - (nc2 - nf);
                    m = cc;
                    n = ff;
                    j = cc;
                    i = ff;
                }
                else
                {
                    if (M - 1 + (nf - nc2) >= N)
                    {
                        ff = N - 1;
                        m = (nf - nc2);
                        cc = M - 1 - (N - 1 - (nf - nc2));
                        n = N - 1;
                        j = cc;
                        i = ff;
                    }
                    else
                    {
                        ff = M - 1 + (nf - nc2);
                        cc = 0;
                        m = 0;
                        n = M - 1;
                        j = cc;
                        i = ff;
                    }
                }
            }
            else if (N == M)
            {
                if (nc2 < nf)
                {
                    cc = (nf - nc2);
                    ff = N - 1;
                    m = cc;
                    n = ff;
                    j = cc;
                    i = ff;
                }
                else
                {
                    ff = N - 1 - (nc2 - nf);
                    cc = 0;
                    m = 0;
                    n = ff;
                    j = cc;
                    i = ff;
                }

            }
            else
            {
                if (nc2 > nf)
                {
                    if (M - 1 - (nc2 - nf) <= N - 1)
                    {
                        cc = M - 1 - (nc2 - nf);
                        ff = M - 1 - (nc2 - nf);
                        m = M - 1;
                        n = cc;
                        j = cc;
                        i = ff;
                    }
                    else
                    {
                        cc = Math.Abs(((N - 1) - nf) - nc);
                        ff = N - 1;
                        m = 0;
                        n = ff;
                        j = cc;
                        i = ff;
                    }
                }
                else
                {
                    cc = nc - ((N - 1) - nf);
                    ff = N - 1;
                    m = nf - nc2;
                    n = ff;
                    j = cc;
                    i = ff;
                }
            }
            while (n >= m)
            {
                var col = partida.Filas[ff].columnas[cc];
                if (partida.colores_actual.Contains(col.color) && col.nombre == c)
                {
                    r = true;
                    if (r2 && !reg)
                    {
                        r2 = false;
                        i--;
                        j++;
                    }
                    r3 = true;
                }
                else if (partida.colores_actual.Contains(col.color) && col.nombre != c && !r2)
                {
                    r2 = true;
                    r3 = true;
                }
                else if (partida.colores_actual.Contains(col.color) & col.nombre != c && r2)
                {
                    r2 = true;
                    i = ff;
                    j = cc;
                    r3 = true;
                    reg = false;
                }

                if (col.color != "" && partida.colores_contrario.Contains(col.color) && col.color != "b")
                {
                    reg = true;
                    r3 = true;
                }
                else if (col.color == "")
                {
                    reg = false;
                    if (r)
                    {
                        break;
                    }
                    if (r2)
                    {
                        r2 = false;
                        i--;
                        j++;
                    }
                }
                else if (reg && partida.colores_actual.Contains(col.color) && r && r2)
                {
                    var u = 0;
                    var k = 0;
                    var u2 = 0;
                    var k2 = 0;
                    if (nf > i && nc < j)
                    {
                        u = nf - 1;
                        k = i;
                        k2 = j;
                        u2 = nc + 1;
                    }
                    else
                    {
                        u = i - 1;
                        k = nf;
                        k2 = nc;
                        u2 = j + 1;
                    }
                    while (u > k && u2 < k2)
                    {
                        partida.Filas[u].columnas[u2].color = color;
                        u--;
                        u2++;
                    }
                    if (j < nc && i > nf)
                    {
                        r = true;
                        i = nf;
                        j = nc;
                        r2 = false;
                        reg = false;
                        r3 = true;
                    }
                }

                else
                {
                    if ((r && !r3))
                    {
                        break;
                    }
                    if ((r2 && !r3))
                    {
                        r2 = false;
                    }
                    r3 = false;
                    reg = false;
                }
                if (r && !r2)
                {
                    i--;
                    j++;
                }
                else if (!r2)
                {
                    i--;
                    j++;
                }
                cc++;
                ff--;
                n--;
            }
            /*return*/
            return partida;

        }
        public static PartidaXtreamViewModel Movimientos(PartidaXtreamViewModel partida)
        {
            var N = 0;
            var M = 0;
            foreach (var fil in partida.Filas)
            {
                foreach (var col in fil.columnas)
                {
                    N = partida.Filas.Count();
                    M = fil.columnas.Count();
                    if (col.color == "")
                    {
                        col.color = "b";
                    }
                }
            }
            var i = 0;
            foreach (var fil in partida.Filas)
            {
                var j = 0;
                foreach (var col in fil.columnas)
                {
                    if (col.color == "b")
                    {
                        /*horizontal*/
                        var reg = false;
                        var cr = false;
                        var cd = false;
                        foreach (var c in fil.columnas)
                        {
                            if (!partida.colores_contrario.Contains(c.color) && c.color != "b" && c.color != "" && (cd || cr))
                            {
                                reg = true;
                            }
                            else if (c.color == "b" && cd)
                            {
                                cr = false;
                                break;
                            }
                            else if (c.color == "b" && cr && !reg)
                            {
                                cr = false;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (c.color == "" && cd)
                            {
                                break;
                            }
                            else if (c.nombre == col.nombre && cr && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else if (partida.colores_contrario.Contains(c.color) && cd && !reg)
                            {
                                break;
                            }
                            else if ((partida.colores_contrario.Contains(c.color) || c.nombre == col.nombre) && !reg)
                            {
                                cr = true;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (partida.colores_contrario.Contains(c.color) && c.nombre != col.nombre && reg && !cd)
                            {
                                cr = true;
                                reg = false;
                            }
                            else if ((partida.colores_contrario.Contains(c.color) && cd || c.nombre == col.nombre && cr) && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else
                            {
                                reg = false;
                                cr = false;
                            }
                        }
                        /*vertical*/
                        reg = false;
                        cr = false;
                        cd = false;
                        var h = 0;
                        while (h < N)
                        {
                            var c = partida.Filas[h].columnas[j];
                            var f = partida.Filas[h];
                            if (!partida.colores_contrario.Contains(c.color) && c.color != "b" && c.color != "" && (cd || cr))
                            {
                                reg = true;
                            }
                            else if (c.color == "b" && cd)
                            {
                                cr = false;
                                break;
                            }
                            else if (c.color == "b" && cr && !reg)
                            {
                                cr = false;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (c.color == "" && cd)
                            {
                                break;
                            }
                            else if (f.nombre == fil.nombre && cr && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else if (partida.colores_contrario.Contains(c.color) && cd && !reg)
                            {
                                break;
                            }
                            else if ((partida.colores_contrario.Contains(c.color) || f.nombre == fil.nombre) && !reg)
                            {
                                cr = true;
                                if (f.nombre == fil.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (partida.colores_contrario.Contains(c.color) && f.nombre != fil.nombre && reg && !cd)
                            {
                                cr = true;
                                reg = false;
                            }
                            else if ((partida.colores_contrario.Contains(c.color) && cd || f.nombre == fil.nombre && cr) && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else
                            {
                                reg = false;
                                cr = false;
                            }
                            h++;
                        }
                        /*digaonal derecha*/
                        var n = 0;
                        var ff = 0;
                        var cc = 0;
                        reg = false;
                        cr = false;
                        cd = false;
                        var nc = j;
                        var nf = i;
                        var G = 0;
                        if (N > M)
                        {
                            if (nc < nf)
                            {
                                if (nf - nc >= (N-1) - (M-1))
                                {
                                    ff = nf-nc;
                                    G = N;
                                    cc = 0;
                                    n = ff;
                                }
                                else
                                {
                                    cc = 0;
                                    ff = nf - nc;
                                    G = M;
                                    n = cc;
                                }
                            }
                            else
                            {
                                ff = 0;
                                G = M;
                                cc = nc - nf;
                                n = cc;
                            }
                        }
                        else if(N == M)
                        {
                            if (nc < nf)
                            {
                                cc = 0;
                                ff = nf - nc;
                                n = ff;
                                G = N;
                            }
                            else
                            {
                                ff = 0;
                                cc = nc - nf;
                                n = cc;
                                G = N;
                            }

                        }
                        else
                        {
                            if (nc < nf)
                            {
                                ff = nf - nc;
                                G = N;
                                cc = 0;
                                n = ff;
                            }
                            else
                            {
                                if (nc - nf >= (M - 1) - (N - 1))
                                {
                                    ff = 0;
                                    G = M;
                                    cc = nc-nf;
                                    n = cc;
                                }
                                else
                                {
                                    cc = nc - nf;
                                    ff = 0;
                                    G = N;
                                    n = ff;
                                }
                            }
                        }
                        while (n < G)
                        {
                            var c = partida.Filas[ff].columnas[cc];
                            if (!partida.colores_contrario.Contains(c.color) && c.color != "b" && c.color != "" && (cd || cr))
                            {
                                reg = true;
                            }
                            else if (c.color == "b" && cd)
                            {
                                cr = false;
                                break;
                            }
                            else if (c.color == "b" && cr && !reg)
                            {
                                cr = false;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (c.color == "" && cd)
                            {
                                break;
                            }
                            else if (c.nombre == col.nombre && cr && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else if (partida.colores_contrario.Contains(c.color) && cd && !reg)
                            {
                                break;
                            }
                            else if ((partida.colores_contrario.Contains(c.color) || c.nombre == col.nombre) && !reg)
                            {
                                cr = true;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (partida.colores_contrario.Contains(c.color) && c.nombre != col.nombre && reg && !cd)
                            {
                                cr = true;
                                reg = false;
                            }
                            else if ((partida.colores_contrario.Contains(c.color) && cd || c.nombre == col.nombre && cr) && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else
                            {
                                reg = false;
                                cr = false;
                            }
                            cc++;
                            ff++;
                            n++;
                        }
                        /*diagonal izquierda*/
                        n = 0;
                        ff = 0;
                        cc = 0;
                        reg = false;
                        cr = false;
                        cd = false;
                        var nc2 = M - 1 - nc;
                        var m = 0;
                        if (N>M)
                        {
                            if (nc2 > nf)
                            {
                                cc = 0;
                                ff = M - 1 - (nc2 - nf);
                                m = cc;
                                n = ff;
                            }
                            else
                            {
                                if (M - 1 + (nf - nc2) >= N)
                                {
                                    ff = N - 1;
                                    m = (nf - nc2);
                                    cc =M-1-( N - 1 - (nf - nc2));
                                    n = N - 1;
                                }
                                else
                                {
                                    ff = M - 1 + (nf - nc2);
                                    cc = 0;
                                    m = 0;
                                    n = M-1;
                                }
                            }
                        }
                        else if (N == M)
                        {
                            if (nc2 < nf)
                            {
                                cc = (nf - nc2);
                                ff = N - 1;
                                m = cc;
                                n = ff;
                            }
                            else
                            {
                                ff = N - 1 - (nc2 - nf);
                                cc = 0;
                                m = 0;
                                n = ff;
                            }

                        }
                        else
                        {
                            if (nc2 > nf)
                            {
                                if (M - 1 - (nc2 - nf)<=N-1)
                                {
                                    cc = M - 1 - (nc2 - nf);
                                    ff = M - 1 - (nc2 - nf);
                                    m = M-1;
                                    n = cc; 
                                }
                                else
                                {
                                    cc = Math.Abs(((N-1)-nf)-nc);
                                    ff = N - 1;
                                    m = 0;
                                    n = ff;
                                }
                            }
                            else
                            {
                                cc = nc-((N-1)-nf);
                                ff = N - 1;
                                m = nf-nc2;
                                n = ff;
                            }
                        }
                        while (n >= m)
                        {
                            var c = partida.Filas[ff].columnas[cc];
                            if (!partida.colores_contrario.Contains(c.color) && c.color != "b" && c.color != "" && (cd || cr))
                            {
                                reg = true;
                            }
                            else if (c.color == "b" && cd)
                            {
                                cr = false;
                                break;
                            }
                            else if (c.color == "b" && cr && !reg)
                            {
                                cr = false;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (c.color == "" && cd)
                            {
                                break;
                            }
                            else if (c.nombre == col.nombre && cr && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else if (partida.colores_contrario.Contains(c.color) && cd && !reg)
                            {
                                break;
                            }
                            else if ((partida.colores_contrario.Contains(c.color) || c.nombre == col.nombre) && !reg)
                            {
                                cr = true;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (partida.colores_contrario.Contains(c.color) && c.nombre != col.nombre && reg && !cd)
                            {
                                cr = true;
                                reg = false;
                            }
                            else if ((partida.colores_contrario.Contains(c.color) && cd || c.nombre == col.nombre && cr) && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else
                            {
                                reg = false;
                                cr = false;
                            }
                            cc++;
                            ff--;
                            n--;
                        }
                        /**/
                    }
                    j++;
                }
                i++;
            }
            /*Return*/
            return partida;
        }
        public static bool[] CantidadMovimientos(PartidaXtreamViewModel partida)
        {
            /*movimientos jugador actual*/
            bool j1 = false, j2 = false;
            string AC1 = partida.colorA1, AC2 = partida.colorA2;
            string sig = partida.siguiente_tiro;
            partida = Movimientos(partida);
            /*Probando movimientos del jugador siguiente*/
            foreach (var l in partida.Filas)
            {
                foreach (var c in l.columnas)
                {
                    if (c.color == "")
                    {
                        j1 = true;
                        break;
                    }
                }
            }
            if (partida.siguiente_tiro == partida.colorA1)
            {
                partida.siguiente_tiro = partida.colorA2;
                partida.colores_contrario = partida.colores_jugador2;
            }
            else
            {
                partida.siguiente_tiro = partida.colorA1;
                partida.colores_contrario = partida.colores_jugador1;
            }
            partida = Movimientos(partida);
            foreach (var l in partida.Filas)
            {
                foreach (var c in l.columnas)
                {
                    if (c.color == "")
                    {
                        j2 = true;
                        break;
                    }
                }
            }
           
            /*DEvolviendo original*/
            partida.siguiente_tiro = sig;
            partida.colorA1 = AC1;
            partida.colorA2 = AC2;
            if (partida.siguiente_tiro == partida.colorA1)
            {
                partida.colores_contrario = partida.colores_jugador1;
            }
            else
            {
                partida.colores_contrario = partida.colores_jugador2;
            }
            partida = Movimientos(partida);
            bool[] i = { j1, j2};
            return i;
        }
        public static PartidaXtreamViewModel Punteos(PartidaXtreamViewModel partida)
        {
            partida.punteo_jugador1 = 0;
            partida.punteo_jugador2 = 0;
            foreach (var fil in partida.Filas)
            {
                foreach (var col in fil.columnas)
                {
                    if (partida.colores_jugador1.Contains(col.color))
                    {
                        partida.punteo_jugador1++;
                    }
                    else if (partida.colores_jugador2.Contains(col.color))
                    {
                        partida.punteo_jugador2++;
                    }
                }
            }
            return partida;
        }
        public static object[] ComprobarCasillas(PartidaXtreamViewModel partida)
        {
            var N = partida.Filas.Count();
            var M = partida.Filas[0].columnas.Count();
            var centroF1 = (N / 2)-1;
            var centroF2 = (N / 1);
            var centroC1 = (M / 2) - 1;
            var centroC2 = (M / 1);
            List<List<bool>> mapa = new List<List<bool>>();
            foreach (var fil in partida.Filas)
            {
                List<bool> fila = new List<bool>();
                foreach (var col in fil.columnas)
                {
                    fila.Add(false);
                }
                mapa.Add(fila);
            }
            var f = centroF1;
            var c = centroC1;
            object[] res;
            var centro = true;
            /*sector 1*/
            while (f >= 0)
            {
                c = centroC1;

                while (c >= 0)
                {
                    var col = partida.Filas[f].columnas[c];
                    if (c == centroC1 && f == centroF1)
                    {
                        if (col.color == "" || col.color == "b")
                        {
                            centro = false;
                        }
                        else
                        {
                            var colA = partida.Filas[f - 1].columnas[c];
                            var colI = partida.Filas[f].columnas[c - 1];
                            var colAI = partida.Filas[f - 1].columnas[c - 1];
                            if (colA.color != "" && colA.color != "b")
                            {
                                mapa[f - 1][c] = true;
                            }
                            if (colI.color != "" && colI.color != "b")
                            {
                                mapa[f][c - 1] = true;
                            }
                            if (colAI.color != "" && colAI.color != "b")
                            {
                                mapa[f - 1][c - 1] = true;
                            }
                            mapa[f][c] = true;
                        }
                    }
                    else if (c > 0 && f > 0 && c < centroC1 && f < centroF1 && col.color != "" && col.color != "b")
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c > 0 && f == 3 && c < centroC1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c == 0 && f == 0)
                    {
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        if (mapa[f][c])
                        {
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c == 3 && f == 0)
                    {
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c == 0 && f == 3)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c == centroC1 && f > 0 && f < centroF1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c == 0 && f > 0 && f < centroF1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (f == 0 && c > 0 && c < centroC1)
                    {
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    if (!centro)
                    {
                        res = new object[] { partida, false };
                        return res;
                    }
                    c--;
                }
                f--;
            }
            /*sector2*/
            c = centroC1;
            f = centroF2;
            while (f <= N-1)
            {
                c = centroC1;

                while (c >= 0)
                {
                    var col = partida.Filas[f].columnas[c];
                    if (c == centroC1 && f == centroF2)
                    {
                        if (col.color == "" || col.color == "b")
                        {
                            centro = false;
                        }
                        else
                        {
                            var colB = partida.Filas[f + 1].columnas[c];
                            var colI = partida.Filas[f].columnas[c - 1];
                            var colBI = partida.Filas[f + 1].columnas[c - 1];
                            if (colB.color != "" && colB.color != "b")
                            {
                                mapa[f + 1][c] = true;
                            }
                            if (colI.color != "" && colI.color != "b")
                            {
                                mapa[f][c - 1] = true;
                            }
                            if (colBI.color != "" && colBI.color != "b")
                            {
                                mapa[f + 1][c - 1] = true;
                            }
                            mapa[f][c] = true;
                        }
                    }
                    else if (c == 0 && f == centroF2)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c == centroC1 && f == N-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c == 0 && f == N-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c > 0 && f > centroF2 && c < centroC1 && f < N-1 && col.color != "" && col.color != "b")
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c > 0 && f == N-1 && c < centroC1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colI.color != "b" && colAI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colAD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c == centroC1 && f > centroF2 && f < N-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c == 0 && f > centroF2 && f < N-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (f == centroF2 && c > 0 && c < centroC1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }

                    if (!centro)
                    {
                        res = new object[] { partida, false };
                        return res;
                    }
                    c--;
                }
                f++;
            }
            /*sector3*/
            c = centroC2;
            f = centroF2;
            while (f >= 0)
            {
                c = centroC2;
                while (c <= M-1)
                {
                    var col = partida.Filas[f].columnas[c];
                    if (c == centroC2 && f == centroF1)
                    {
                        if (col.color == "" || col.color == "b")
                        {
                            centro = false;
                        }
                        else
                        {
                            var colA = partida.Filas[f - 1].columnas[c];
                            var colD = partida.Filas[f].columnas[c + 1];
                            var colAD = partida.Filas[f - 1].columnas[c + 1];
                            if (colA.color != "" && colA.color != "b")
                            {
                                mapa[f - 1][c] = true;
                            }
                            if (colD.color != "" && colD.color != "b")
                            {
                                mapa[f][c + 1] = true;
                            }
                            if (colAD.color != "" && colAD.color != "b")
                            {
                                mapa[f - 1][c + 1] = true;
                            }
                            mapa[f][c] = true;
                        }
                    }
                    else if (c == centroC2 && f == 0)
                    {
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c == M-1 && f == 0)
                    {
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                        }
                    }
                    else if (c == M-1 && f == centroF1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                        }
                    }
                    else if (c > centroC2 && f > 0 && c < M-1 && f < centroF1 && col.color != "" && col.color != "b")
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c > centroC2 && f == centroF1 && c < M-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c == M-1 && f > 0 && f < centroF1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                        }

                    }
                    else if (c == centroC2 && f > 0 && f < centroF1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (f == 0 && c > 0 && c < M-1)
                    {
                        var colA = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f + 1].columnas[c + 1];
                        var colAI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f + 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }

                    if (!centro)
                    {
                        res = new object[] { partida, false };
                        return res;
                    }
                    c++;
                }
                f--;
            }
            /*sector4*/
            c = centroC2;
            f = centroF2;
            while (f <= N-1)
            {
                c = centroC2;
                while (c <= M-1)
                {
                    var col = partida.Filas[f].columnas[c];
                    if (c == centroC2 && f == centroF2)
                    {
                        if (col.color == "" || col.color == "b")
                        {
                            centro = false;
                        }
                        else
                        {
                            var colB = partida.Filas[f + 1].columnas[c];
                            var colD = partida.Filas[f].columnas[c + 1];
                            var colBD = partida.Filas[f + 1].columnas[c + 1];
                            if (colB.color != "" && colB.color != "b")
                            {
                                mapa[f + 1][c] = true;
                            }
                            if (colD.color != "" && colD.color != "b")
                            {
                                mapa[f][c + 1] = true;
                            }
                            if (colBD.color != "" && colBD.color != "b")
                            {
                                mapa[f + 1][c + 1] = true;
                            }
                            mapa[f][c] = true;
                        }
                    }
                    else if (c == centroC2 && f == N-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }
                    else if (c == M-1 && f == N-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                        }
                    }
                    else if (c == M-1 && f == centroF2)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                        }
                    }
                    else if (c > centroC2 && f > centroF2 && c < M-1 && f < N-1 && col.color != "" && col.color != "b")
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c > centroC2 && f == N-1 && c < M-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colI.color != "b" && colAI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colAD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (c == M-1 && f > 4 && f < N-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                        }

                    }
                    else if (c == centroC2 && f > centroF2 && f < N-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }

                    }
                    else if (f == centroF2 && c > centroC2 && c < M-1)
                    {
                        var colA = partida.Filas[f - 1].columnas[c];
                        var colB = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f - 1].columnas[c + 1];
                        var colAI = partida.Filas[f - 1].columnas[c - 1];
                        var colBD = partida.Filas[f + 1].columnas[c + 1];
                        var colBI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c - 1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c + 1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c + 1] = true; }
                        }
                    }

                    if (!centro)
                    {
                        res = new object[] { partida, false };
                        return res;
                    }
                    c++;
                }
                f++;
            }
            /*Comprobando fichas validas*/
            f = 0;
            c = 0;
            while (f <= N-1)
            {
                c = 0;
                while (c <= M-1)
                {
                    if (mapa[f][c] == true)
                    {
                        if (f == centroF1 && c == centroC1)
                        {
                            c++;
                            continue;
                        }
                        else if (f == centroF1 && c == centroC2)
                        {
                            c++;
                            continue;
                        }
                        else if (f == centroF2 && c == centroC1)
                        {
                            c++;
                            continue;
                        }
                        else if (f == centroF2 && c == centroC2)
                        {
                            c++;
                            continue;
                        }
                        else if (f > 1 && f < N-2 && c < M-2 && c > 1)
                        {
                            var d1 = mapa[f][c + 1];
                            var d2 = mapa[f][c + 2];
                            var i1 = mapa[f][c - 1];
                            var i2 = mapa[f][c - 2];
                            var b1 = mapa[f + 1][c];
                            var b2 = mapa[f + 2][c];
                            var bi1 = mapa[f + 1][c - 1];
                            var bi2 = mapa[f + 2][c - 2];
                            var bd1 = mapa[f + 1][c + 1];
                            var bd2 = mapa[f + 2][c + 2];
                            var ai1 = mapa[f - 1][c - 1];
                            var ai2 = mapa[f - 2][c - 2];
                            var a1 = mapa[f - 1][c];
                            var a2 = mapa[f - 2][c];
                            var ad1 = mapa[f - 1][c + 1];
                            var ad2 = mapa[f - 2][c + 2];
                            if (bi1 && bi2)
                            {
                                c++;
                                continue;
                            }
                            else if (bd1 && bd2)
                            {
                                c++;
                                continue;
                            }
                            else if (b1 && b2)
                            {
                                c++;
                                continue;
                            }
                            else if (a1 && a2)
                            {
                                c++;
                                continue;
                            }
                            else if (ai1 && ai2)
                            {
                                c++;
                                continue;
                            }
                            else if (ad1 && ad2)
                            {
                                c++;
                                continue;
                            }
                            else if (d1 && d2)
                            {
                                c++;
                                continue;
                            }
                            else if (i1 && i2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                        else if (f >= 0 && f <= 1 && c <= 1 && c >= 0)
                        {
                            var d1 = mapa[f][c + 1];
                            var d2 = mapa[f][c + 2];
                            var b1 = mapa[f + 1][c];
                            var b2 = mapa[f + 2][c];
                            var bd1 = mapa[f + 1][c + 1];
                            var bd2 = mapa[f + 2][c + 2];
                            if (bd1 && bd2)
                            {
                                c++;
                                continue;
                            }
                            else if (b1 && b2)
                            {
                                c++;
                                continue;
                            }
                            else if (d1 && d2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                        else if (f >= 0 && f <= 1 && c <= M-1 && c >= M-2)
                        {
                            var i1 = mapa[f][c - 1];
                            var i2 = mapa[f][c - 2];
                            var b1 = mapa[f + 1][c];
                            var b2 = mapa[f + 2][c];
                            var bi1 = mapa[f + 1][c - 1];
                            var bi2 = mapa[f + 2][c - 2];
                            if (bi1 && bi2)
                            {
                                c++;
                                continue;
                            }
                            else if (b1 && b2)
                            {
                                c++;
                                continue;
                            }
                            else if (i1 && i2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                        else if (f >= N-2 && f <= N-1 && c <= 1 && c >= 0)
                        {
                            var i1 = mapa[f][c + 1];
                            var i2 = mapa[f][c + 2];
                            var a1 = mapa[f - 1][c];
                            var a2 = mapa[f - 2][c];
                            var ad1 = mapa[f - 1][c + 1];
                            var ad2 = mapa[f - 2][c + 2];
                            if (a1 && a2)
                            {
                                c++;
                                continue;
                            }
                            else if (ad1 && ad2)
                            {
                                c++;
                                continue;
                            }
                            else if (i1 && i2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                        else if (f >= N-2 && f <= N-1 && c <= M-1 && c >= M-2)
                        {
                            var i1 = mapa[f][c - 1];
                            var i2 = mapa[f][c - 2];
                            var ai1 = mapa[f - 1][c - 1];
                            var ai2 = mapa[f - 2][c - 2];
                            var a1 = mapa[f - 1][c];
                            var a2 = mapa[f - 2][c];
                            if (a1 && a2)
                            {
                                c++;
                                continue;
                            }
                            else if (ai1 && ai2)
                            {
                                c++;
                                continue;
                            }
                            else if (i1 && i2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                        else if (f >= 0 && f <= 1 && c > 1 && c < 6)
                        {
                            var d1 = mapa[f][c + 1];
                            var d2 = mapa[f][c + 2];
                            var i1 = mapa[f][c - 1];
                            var i2 = mapa[f][c - 2];
                            var b1 = mapa[f + 1][c];
                            var b2 = mapa[f + 2][c];
                            var bi1 = mapa[f + 1][c - 1];
                            var bi2 = mapa[f + 2][c - 2];
                            var bd1 = mapa[f + 1][c + 1];
                            var bd2 = mapa[f + 2][c + 2];
                            if (bi1 && bi2)
                            {
                                c++;
                                continue;
                            }
                            else if (bd1 && bd2)
                            {
                                c++;
                                continue;
                            }
                            else if (b1 && b2)
                            {
                                c++;
                                continue;
                            }
                            else if (d1 && d2)
                            {
                                c++;
                                continue;
                            }
                            else if (i1 && i2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                        else if (f >= N-2 && f <= N-1 && c < M-2 && c > 1)
                        {
                            var d1 = mapa[f][c + 1];
                            var d2 = mapa[f][c + 2];
                            var i1 = mapa[f][c - 1];
                            var i2 = mapa[f][c - 2];
                            var ai1 = mapa[f - 1][c - 1];
                            var ai2 = mapa[f - 2][c - 2];
                            var a1 = mapa[f - 1][c];
                            var a2 = mapa[f - 2][c];
                            var ad1 = mapa[f - 1][c + 1];
                            var ad2 = mapa[f - 2][c + 2];
                            if (a1 && a2)
                            {
                                c++;
                                continue;
                            }
                            else if (ai1 && ai2)
                            {
                                c++;
                                continue;
                            }
                            else if (ad1 && ad2)
                            {
                                c++;
                                continue;
                            }
                            else if (d1 && d2)
                            {
                                c++;
                                continue;
                            }
                            else if (i1 && i2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                        else if (f > 1 && f < N-2 && c <= 1 && c >= 0)
                        {
                            var d1 = mapa[f][c + 1];
                            var d2 = mapa[f][c + 2];
                            var b1 = mapa[f + 1][c];
                            var b2 = mapa[f + 2][c];
                            var bd1 = mapa[f + 1][c + 1];
                            var bd2 = mapa[f + 2][c + 2];
                            var a1 = mapa[f - 1][c];
                            var a2 = mapa[f - 2][c];
                            var ad1 = mapa[f - 1][c + 1];
                            var ad2 = mapa[f - 2][c + 2];
                            if (bd1 && bd2)
                            {
                                c++;
                                continue;
                            }
                            else if (b1 && b2)
                            {
                                c++;
                                continue;
                            }
                            else if (a1 && a2)
                            {
                                c++;
                                continue;
                            }
                            else if (ad1 && ad2)
                            {
                                c++;
                                continue;
                            }
                            else if (d1 && d2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                        else if (f > 1 && f < N-2 && c <= M-1 && c >= M-2)
                        {
                            var i1 = mapa[f][c - 1];
                            var i2 = mapa[f][c - 2];
                            var b1 = mapa[f + 1][c];
                            var b2 = mapa[f + 2][c];
                            var bi1 = mapa[f + 1][c - 1];
                            var bi2 = mapa[f + 2][c - 2];
                            var ai1 = mapa[f - 1][c - 1];
                            var ai2 = mapa[f - 2][c - 2];
                            var a1 = mapa[f - 1][c];
                            var a2 = mapa[f - 2][c];
                            if (bi1 && bi2)
                            {
                                c++;
                                continue;
                            }
                            else if (b1 && b2)
                            {
                                c++;
                                continue;
                            }
                            else if (a1 && a2)
                            {
                                c++;
                                continue;
                            }
                            else if (ai1 && ai2)
                            {
                                c++;
                                continue;
                            }
                            else if (i1 && i2)
                            {
                                c++;
                                continue;
                            }
                            else
                            {
                                mapa[f][c] = false;
                            }
                        }
                    }
                    c++;
                }
                f++;
            }
            /*Verificando validez*/
            var i = 0;
            var j = 0;
            while (i < N)
            {
                j = 0;
                while (j < M)
                {
                    if (!mapa[i][j] && partida.Filas[i].columnas[j].color != "" && partida.Filas[i].columnas[j].color != "b")
                    {
                        res = new object[] { partida, false };
                        return res;
                    }
                    else if (!mapa[i][j])
                    {
                        partida.Filas[i].columnas[j].color = "b";
                    }
                    j++;
                }
                i++;
            }
            /*Retorno si todo esta correcto*/
            res = new object[] { partida, true };
            return res;
        }
    }
}