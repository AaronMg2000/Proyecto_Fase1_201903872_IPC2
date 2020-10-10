using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoIPC22011903872.Models.ViewModels;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.UI.WebControls;

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

        public static PartidaViewModel CrearPartida(DatosModel dato,string color1,string color2,string siguiente,string jugador1,string jugador2){
            PartidaViewModel partida = new PartidaViewModel();
            string[] columnas = { "A", "B", "C", "D", "E", "F", "G", "H" };
            partida.nombre = dato.nombre + 1;
            partida.jugador1 = jugador1;
            partida.jugador2 = jugador2;
            if (jugador2 == "Maquina")
            {
                partida.tipo = "M";
            }
            partida.color_jugador1 = color1;
            partida.color_jugador2 = color2;
            partida.movimientos_1 = 0;
            partida.movimientos_2 = 0;
            partida.punteo_jugador1 = 2;
            partida.punteo_jugador2 = 2;
            partida.siguiente_tiro = siguiente;
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
                    else if (j == 3 && i == 5)
                    {
                        col.color = "negro";
                    }
                    else if (j == 4 && i == 4)
                    {
                        col.color = "negro";
                    }
                    else if (j == 4 && i == 5)
                    {
                        col.color = "blanco";
                    }

                    else if (j == 2 && i == 4 && siguiente == "negro")
                    {
                        col.color = "";
                    }
                    else if (j == 3 && i == 3 && siguiente == "negro")
                    {
                        col.color = "";
                    }
                    else if (j == 5 && i == 5 && siguiente == "negro")
                    {
                        col.color = "";
                    }
                    else if (j == 4 && i == 6 && siguiente == "negro")
                    {
                        col.color = "";
                    }

                    else if (j == 5 && i == 4 && siguiente == "blanco")
                    {
                        col.color = "";
                    }
                    else if (j == 4 && i == 3 && siguiente == "blanco")
                    {
                        col.color = "";
                    }
                    else if (j == 2 && i == 5 && siguiente == "blanco")
                    {
                        col.color = "";
                    }
                    else if (j == 3 && i == 6 && siguiente == "blanco")
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
        
        public static PartidaViewModel AgregarFicha(PartidaViewModel partida,string fila,string columna)
        {
            var color = partida.siguiente_tiro;
            if (partida.color_jugador1 == partida.siguiente_tiro)
            {
                partida.punteo_jugador1++;
                partida.movimientos_1++;
            }
            else
            {
                partida.punteo_jugador2++;
                partida.movimientos_2++;
            }
            if (color=="blanco")
            {
                partida.siguiente_tiro = "negro";
            }
            else
            {
                partida.siguiente_tiro = "blanco";
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
            partida = CambioColor(partida,fila,columna);
            partida = Movimientos(partida);
            partida = Punteos(partida);
            return partida;
        }

        public static PartidaViewModel CambioColor(PartidaViewModel partida, string f, string c)
        {
            var color = "";
            FilaViewModel fila = new FilaViewModel();
            var nc = 0;
            var nf = 0;
            foreach (var fil in partida.Filas)
            {
                if (fil.nombre == f) {
                    fila = fil;
                    foreach (var col in fil.columnas)
                    {
                        if (col.nombre == c)
                        {
                            break;
                        }
                        nc++;
                    }
                    break;
                }
                nf++;
            }

            if (partida.siguiente_tiro == "negro")
            {
                color = "blanco";
            }
            else
            {
                color = "negro";
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
                if (col.color == color && col.nombre == c)
                {
                    r = true;
                    if(r2 && !reg)
                    {
                        r2 = false;
                        i++;
                    }
                    r3 = true;
                }
                else if (col.color == color && col.nombre != c && !r2)
                {
                    r2 = true;
                    r3 = true;
                }
                else if (col.color==color & col.nombre!=c && r2)
                {
                    r2 = true;
                    i = n1;
                    r3 = true;
                    reg = false;
                }

                if (col.color != "" && col.color != color && col.color != "b" )
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
                else if (reg && col.color == color && r && r2)
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
                    if (i<nc)
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
                    if((r2 && !r3))
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
                if (col.color == color && fil.nombre==f)
                {
                    r = true;
                    if (r2 && !reg)
                    {
                        r2 = false;
                        i++;
                    }
                    r3 = true;
                }
                else if(col.color==color && fil.nombre != f && !r2)
                {
                    r2 = true;
                    r3 = true;
                }
                else if (col.color == color & fil.nombre != f && r2)
                {
                    r2 = true;
                    i = n1;
                    r3 = true;
                    reg = false;
                }

                if (col.color!=color && fil.nombre!=f && col.color!="b" && col.color!="")  
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
                else if(reg && col.color==color && r && r2)
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
                    while (u<k)
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
            if (nc < nf)
            {
                j = 0;
                i = nf - nc;
                n = i;
                ff = i;
                cc = j;
            }
            else
            {
                i = 0;
                j = nc - nf;
                n = j;
                cc = j;
                ff = i;
            }
            while (n < 8)
            {
                var col = partida.Filas[ff].columnas[cc];
                if (col.color == color && col.nombre == c)
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
                else if (col.color == color && col.nombre != c && !r2)
                {
                    r2 = true;
                    r3 = true;
                }
                else if (col.color == color & col.nombre != c && r2)
                {
                    r2 = true;
                    i = ff;
                    j = cc;
                    r3 = true;
                    reg = false;
                }

                if (col.color != "" && col.color != color && col.color != "b")
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
                else if (reg && col.color == color && r && r2)
                {
                    var u = 0;
                    var k = 0;
                    var u2 = 0;
                    var k2 = 0;
                    if (j < nc && i<nf)
                    {
                        u = i+1;
                        k = nf;
                        k2 = nc;
                        u2 = j+1;
                    }
                    else
                    {
                        u = nf+1;
                        k = i;
                        u2 = nc+1;
                        k2 = j;
                    }
                    while (u < k && u2<k2)
                    {
                        partida.Filas[u].columnas[u2].color = color;
                        u++;
                        u2++;
                    }
                    if (j < nc && i<nf)
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
            var nc2 = 7 - nc;
            r = false;
            r2 = false;
            r3 = false;
            var m = 0;
            if (nc2 < nf)
            {
                j = (nf-nc2);
                i = 7;
                m = j;
                n = i;
                ff = i;
                cc = j;
            }
            else
            {
                i = 7-(nc2-nf);
                j = 0;
                m = 0;
                n = i;
                cc = j;
                ff = i;
            }
            while (n >=m)
            {
                var col = partida.Filas[ff].columnas[cc];
                    if (col.color == color && col.nombre == c)
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
                else if (col.color == color && col.nombre != c && !r2)
                {
                    r2 = true;
                    r3 = true;
                }
                else if (col.color == color & col.nombre != c && r2)
                {
                    r2 = true;
                    i = ff;
                    j = cc;
                    r3 = true;
                    reg = false;
                }

                if (col.color != "" && col.color != color && col.color != "b")
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
                else if (reg && col.color == color && r && r2)
                {
                    var u = 0;
                    var k = 0;
                    var u2 = 0;
                    var k2 = 0;
                    if(nf>i && nc < j) { 
                        u = nf-1;
                        k = i;
                        k2 = j;
                        u2 = nc+1;
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
        public static PartidaViewModel Movimientos(PartidaViewModel partida)
        {
            foreach(var fil in partida.Filas)
            {
                foreach(var col in fil.columnas)
                {
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
                foreach(var col in fil.columnas)
                {
                    if (col.color == "b")
                    {
                        /*horizontal*/
                        var reg = false;
                        var cr = false;
                        var cd = false;
                        foreach (var c in fil.columnas)
                        {
                            if (c.color != partida.siguiente_tiro && c.color!="b" && c.color!="" && (cd || cr))
                            {
                                reg = true;
                            }
                            else if (c.color == "b" && cd)
                            {
                                cr = false;
                                break;
                            }
                            else if (c.color=="b" && cr && !reg)
                            {
                                cr = false;
                            }
                            else if(c.nombre==col.nombre && cr && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else if(c.color==partida.siguiente_tiro && cd && !reg)
                            {
                                break;
                            }
                            else if ((c.color==partida.siguiente_tiro || c.nombre==col.nombre) && !reg)
                            {
                                cr = true;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (c.color==partida.siguiente_tiro && c.nombre!=col.nombre && reg && !cd)
                            {
                                cr = true;
                                reg = false;
                            }
                            else if((c.color==partida.siguiente_tiro || c.nombre==col.nombre) && cr && cd)
                            {
                                col.color="";
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
                        while (h < 8)
                        {
                            var c = partida.Filas[h].columnas[j];
                            var f = partida.Filas[h];
                            if (c.color != partida.siguiente_tiro && c.color != "b" && c.color != "" && (cd || cr))
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
                            }
                            else if (f.nombre == fil.nombre && cr && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else if (c.color == partida.siguiente_tiro && cd && !reg)
                            {
                                break;
                            }
                            else if ((c.color == partida.siguiente_tiro || f.nombre == fil.nombre) && !reg)
                            {
                                cr = true;
                                if (f.nombre == fil.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (c.color == partida.siguiente_tiro && f.nombre != fil.nombre && reg && !cd)
                            {
                                cr = true;
                                reg = false;
                            }
                            else if ((c.color == partida.siguiente_tiro || f.nombre == fil.nombre) && cr && cd)
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
                        reg = false;
                        cr = false;
                        cd = false;
                        var nc = j;
                        var nf = i;
                        var n = 0;
                        var ff = 0;
                        var cc = 0;
                        if (nc < nf)
                        {
                            cc = 0;
                            ff = nf - nc;
                            n = ff;
                        }
                        else
                        {
                            ff = 0;
                            cc = nc - nf;
                            n = cc;
                        }
                        while (n < 8)
                        {
                            var c = partida.Filas[ff].columnas[cc];
                            if (c.color != partida.siguiente_tiro && c.color != "b" && c.color != "" && (cd || cr))
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
                            }
                            else if (c.nombre == col.nombre && cr && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else if (c.color == partida.siguiente_tiro && cd && !reg)
                            {
                                break;
                            }
                            else if ((c.color == partida.siguiente_tiro || c.nombre == col.nombre) && !reg)
                            {
                                cr = true;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (c.color == partida.siguiente_tiro && c.nombre != col.nombre && reg && !cd)
                            {
                                cr = true;
                                reg = false;
                            }
                            else if ((c.color == partida.siguiente_tiro || c.nombre == col.nombre) && cr && cd)
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
                        var nc2 = 7 - nc;
                        var m = 0;
                        if (nc2 < nf)
                        {
                            cc = (nf - nc2);
                            ff = 7;
                            m = cc;
                            n = ff;
                        }
                        else
                        {
                            ff = 7 - (nc2 - nf);
                            cc = 0;
                            m = 0;
                            n = ff;
                        }
                        while (n >= m)
                        {
                            var c = partida.Filas[ff].columnas[cc];
                            if (c.color != partida.siguiente_tiro && c.color != "b" && c.color != "" && (cd || cr))
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
                            }
                            else if (c.nombre == col.nombre && cr && reg)
                            {
                                col.color = "";
                                break;
                            }
                            else if (c.color == partida.siguiente_tiro && cd && !reg)
                            {
                                break;
                            }
                            else if ((c.color == partida.siguiente_tiro || c.nombre == col.nombre) && !reg)
                            {
                                cr = true;
                                if (c.nombre == col.nombre)
                                {
                                    cd = true;
                                }
                            }
                            else if (c.color == partida.siguiente_tiro && c.nombre != col.nombre && reg && !cd)
                            {
                                cr = true;
                                reg = false;
                            }
                            else if ((c.color == partida.siguiente_tiro || c.nombre == col.nombre) && cr && cd)
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
        public static bool[] CantidadMovimientos(PartidaViewModel partida)
        {
            bool j1=false,j2=false;
            partida = Movimientos(partida);
            foreach (var l in partida.Filas)
            {
                foreach(var c in l.columnas)
                {
                    if (c.color == "")
                    {
                        j1 = true;
                        break;
                    }
                }
            }
            if (partida.siguiente_tiro == "negro")
            {
                partida.siguiente_tiro = "blanco";
            }
            else
            {
                partida.siguiente_tiro = "negro";
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
            if (partida.siguiente_tiro == "negro")
            {
                partida.siguiente_tiro = "blanco";
            }
            else
            {
                partida.siguiente_tiro = "negro";
            }
            partida = Movimientos(partida);
            bool[] i = { j1,j2 };
            return i;
        }
        public static PartidaViewModel Punteos(PartidaViewModel partida)
        {
            partida.punteo_jugador1 = 0;
            partida.punteo_jugador2 = 0;
            foreach(var fil in partida.Filas)
            {
                foreach(var col in fil.columnas)
                {
                    if (col.color == partida.color_jugador1)
                    {
                        partida.punteo_jugador1++;
                    }
                    else if(col.color==partida.color_jugador2)
                    {
                        partida.punteo_jugador2++;
                    }
                }
            }
            return partida;
        }

        public static object[] ComprobarCasillas(PartidaViewModel partida)
        {
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
            var f = 3;
            var c = 3;
            object[] res;
            var centro = true;
            /*sector 1*/
            while (f>=0)
            {
                c = 3;
                while (c >= 0)
                {
                    var col = partida.Filas[f].columnas[c];
                    if(c==3 && f == 3)
                    {
                        if(col.color=="" || col.color == "b")
                        {
                            centro = false;
                        }
                        else
                        {
                            var colA = partida.Filas[f - 1].columnas[c];
                            var colI = partida.Filas[f].columnas[c-1];
                            var colAI = partida.Filas[f - 1].columnas[c - 1];
                            if(colA.color!="" && colA.color != "b")
                            {
                                mapa[f - 1][c] = true;
                            }
                            if (colI.color != "" && colI.color != "b")
                            {
                                mapa[f][c-1] = true;
                            }
                            if (colAI.color != "" && colAI.color != "b")
                            {
                                mapa[f - 1][c-1] = true;
                            }
                            mapa[f][c] = true;
                        }
                    }
                    else if(c>0 && f > 0 && c<3 && f<3 && col.color!="" && col.color!="b")
                    {
                        var colA = partida.Filas[f-1].columnas[c];
                        var colB = partida.Filas[f+1].columnas[c];
                        var colD = partida.Filas[f].columnas[c+1];
                        var colI = partida.Filas[f].columnas[c-1];
                        var colAD = partida.Filas[f-1].columnas[c+1];
                        var colAI = partida.Filas[f-1].columnas[c-1];
                        var colBD = partida.Filas[f+1].columnas[c+1];
                        var colBI = partida.Filas[f+1].columnas[c-1];
                        if (mapa[f][c])
                        {
                            if(colA.color!="b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c-1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c+1] = true; }
                            if (colB.color != "b" && colB.color != "") { mapa[f + 1][c] = true; }
                            if (colBI.color != "b" && colBI.color != "") { mapa[f + 1][c-1] = true; }
                            if (colBD.color != "b" && colBD.color != "") { mapa[f + 1][c+1] = true; }
                            if (colI.color != "b" && colI.color != "") { mapa[f][c-1] = true; }
                            if (colD.color != "b" && colD.color != "") { mapa[f][c+1] = true; }
                        }
                        
                    }
                    else if(c>0 && f==3 && c < 3)
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
                    else if(c==3 && f>0 && f < 3)
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
                    else if(c==0 && f>0 && f < 3)
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
                    else if(f==0 && c>0 && c < 3)
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
            c = 3;
            f = 4;
            while (f <= 7)
            {
                c = 3;
                while (c >= 0)
                {
                    var col = partida.Filas[f].columnas[c];
                    if (c == 3 && f == 4)
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
                    else if (c > 0 && f > 4 && c < 3 && f < 7 && col.color != "" && col.color != "b")
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
                    else if (c > 0 && f == 7 && c < 3)
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
                    else if (c == 3 && f > 4 && f < 7)
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
                    else if (c == 0 && f > 4 && f < 7)
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
                    else if (f == 4 && c > 0 && c < 3)
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
                    c--;
                }
                f++;
            }
            /*sector3*/
            c = 4;
            f = 3;
            while (f >= 0)
            {
                c = 4;
                while (c <= 7)
                {
                    var col = partida.Filas[f].columnas[c];
                    if (c == 4 && f == 3)
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
                    else if (c > 4 && f > 0 && c < 7 && f < 3 && col.color != "" && col.color != "b")
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
                    else if (c > 4 && f == 3 && c < 7)
                    {
                        var colA = partida.Filas[f + 1].columnas[c];
                        var colD = partida.Filas[f].columnas[c + 1];
                        var colI = partida.Filas[f].columnas[c - 1];
                        var colAD = partida.Filas[f + 1].columnas[c + 1];
                        var colAI = partida.Filas[f + 1].columnas[c - 1];
                        if (mapa[f][c])
                        {
                            if (colA.color != "b" && colA.color != "") { mapa[f - 1][c] = true; }
                            if (colAI.color != "b" && colAI.color != "") { mapa[f - 1][c - 1] = true; }
                            if (colAD.color != "b" && colAD.color != "") { mapa[f - 1][c + 1] = true; }
                            if (colI.color != "b" && colAI.color != "") { mapa[f][c - 1] = true; }
                            if (colD.color != "b" && colAD.color != "") { mapa[f][c + 1] = true; }
                        }
                        
                    }
                    else if (c == 7 && f > 0 && f < 3)
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
                    else if (c == 4 && f > 0 && f < 3)
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
                    else if (f == 0 && c > 0 && c < 7)
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
            c = 4;
            f = 4;
            while (f <= 7)
            {
                c = 4;
                while (c <= 7)
                {
                    var col = partida.Filas[f].columnas[c];
                    if (c == 4 && f == 4)
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
                                mapa[f][c + 1] = true;
                            }
                            if (colBI.color != "" && colBI.color != "b")
                            {
                                mapa[f + 1][c + 1] = true;
                            }
                            mapa[f][c] = true;
                        }
                    }
                    else if (c > 0 && f > 4 && c < 3 && f < 7 && col.color != "" && col.color != "b")
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
                    else if (c > 4 && f == 7 && c < 7)
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
                    else if (c == 7 && f > 4 && f < 7)
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
                    else if (c == 4 && f > 4 && f < 7)
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
                    else if (f == 4 && c > 4 && c < 7)
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
                f++;
            }
            /**/
            var i = 0;
            var j = 0;
            while (i < 8)
            {
                j = 0;
                while (j < 8)
                {
                    if (!mapa[i][j])
                    {
                        partida.Filas[i].columnas[j].color = "b";
                    }
                    j++;
                }
                i++;
            }
            res = new object[] { partida, true };
            return res;
        }
    }
}