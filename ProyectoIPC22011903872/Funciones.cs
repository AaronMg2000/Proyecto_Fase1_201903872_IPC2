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

        public static PartidaViewModel CrearPartida(DatosModel dato,string color1,string color2,string siguiente){
            PartidaViewModel partida = new PartidaViewModel();
            string[] columnas = { "A", "B", "C", "D", "E", "F", "G", "H" };
            partida.nombre = dato.nombre + 1;
            partida.jugador1 = "jugador1";
            partida.jugador2 = "jugador2";
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
            }
            else
            {
                partida.punteo_jugador2++;
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
            var tiro1 = false;
            var tiro2 = false;
            foreach(var fil in partida.Filas)
            {
                foreach(var col in fil.columnas)
                {
                    if (col.color == "")
                    {
                        tiro1 = true;
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

            foreach (var fil in partida.Filas)
            {
                foreach (var col in fil.columnas)
                {
                    if (col.color == "")
                    {
                        tiro2 = true;
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

            if (!tiro1 && tiro2)
            {
                partida.jugador1 = "pasar tiro";
            }else if(!tiro1 && !tiro2)
            {
                partida.jugador1 = "No hay tiros";
            }
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
            foreach (var col in fila.columnas)  
            {
                if (col.color == color && col.nombre == c)
                {
                    r = true;
                }
                else if (col.color == color && col.nombre != c)
                {
                    r2 = true;
                }
                else if (col.color==color & col.nombre!=c && r2)
                {
                    r2 = true;
                    reg = false;
                }

                if (col.color != "" && col.color != color && col.color != "b" )
                {
                    reg = true;
                }
                else if (col.color == "")
                {
                    reg = false;
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
                    break;
                }
                
                else
                {
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
            }
            /*Vertical*/
            i = 0;
            reg = true;
            r = false;
            r2 = false;
            foreach (var fil in partida.Filas)
            {
                var col = fil.columnas[nc];
                if (col.color == color && fil.nombre==f)
                {
                    r = true;
                }
                else if(col.color==color && fil.nombre != f)
                {
                    r2 = true;
                }
                else if (col.color == color & fil.nombre != f && r2)
                {
                    r2 = true;
                    reg = false;
                }

                if (col.color!=color && fil.nombre!=f && col.color!="b" && col.color!="")  
                {
                    reg = true;
                }
                else if (col.color == "")
                {
                    reg = false;
                    break;
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
                    break;
                }
                
                else
                {
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
            }
            /*Diagonal derecha*/
            i = 0;
            var j = 0;
            var n = 0;
            var ff = 0;
            var cc = 0;
            r = false;
            r2 = false;
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
                }
                else if (col.color == color && col.nombre != c)
                {
                    r2 = true;
                }
                else if (col.color == color & col.nombre != c && r2)
                {
                    r2 = true;
                    reg = false;
                }

                if (col.color != "" && col.color != color && col.color != "b")
                {
                    reg = true;
                }
                else if (col.color == "")
                {
                    reg = false;
                    break;
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
                    break;
                }
                
                else
                {
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
                }
                else if (col.color == color && col.nombre != c)
                {
                    r2 = true;
                }
                else if (col.color == color & col.nombre != c && r2)
                {
                    r2 = true;
                    reg = false;
                }

                if (col.color != "" && col.color != color && col.color != "b")
                {
                    reg = true;
                }
                else if (col.color == "")
                {
                    reg = false;
                    break;
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
                    break;
                }
                
                else
                {
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
    }
}